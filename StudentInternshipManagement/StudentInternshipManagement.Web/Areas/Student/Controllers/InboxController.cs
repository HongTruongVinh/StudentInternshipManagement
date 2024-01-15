using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using StudentInternshipManagement.Models.Constants;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class InboxController : StudentBaseController
    {
        private readonly IGroupService _groupService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public InboxController(IMessageService messageService, IGroupService groupService, IUserService userService)
        {
            _messageService = messageService;
            _groupService = groupService;
            _userService = userService;
        }

        // GET: Student/Inbox
        public ActionResult Index()
        {
            GetUserUnreadMassages();
            var userName = User.Identity.GetUserName();
            var id = _userService.GetByUserName(userName).Id;

            var messages = _messageService.GetReceivedEmail(id);
            ViewBag.UnRead = messages.Count(m => m.Status != MessageStatus.Read);
            return View();
        }

        public PartialViewResult GetMessagePage(int? page, int type)
        {
            var id = User.Identity.GetUserId();
            IQueryable<Message> messages = null;
            var pageSize = 5;
            switch (type)
            {
                case 1:
                    messages = _messageService.GetReceivedEmail(id);
                    break;
                case 2:
                    messages = _messageService.GetSentEmail(id);
                    break;
                case 3:
                    messages = _messageService.GetDraftEmail(id);
                    break;
            }
            var mails = messages.ToPagedList(page ?? 1, pageSize);
            ViewBag.Type = type;
            return PartialView("_MessagePage", mails);
        }

        public ActionResult Write(string teacher)
        {
            GetUserUnreadMassages();
            ViewBag.TeacherList = _groupService.GetByStudent(CurrentStudentId).Select(g => g.Teacher.User.FullName);
            ViewBag.Teacher = teacher;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Write(HttpPostedFileBase file, Message model)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // var extension = Path.GetExtension(file.FileName);
                    var filePath = Server.MapPath($"~/Attachment/{model.SenderId}/");
                    Directory.CreateDirectory($"{filePath}");
                    var physicalPath = Path.Combine(filePath, $"{file.FileName}");
                    file.SaveAs(physicalPath);
                    model.File = $"{file.FileName}";
                }

                model.CreatedAt = DateTime.Now; 
                model.UpdatedAt = DateTime.Now;
                model.CreatedBy = User.Identity.GetUserId();
                model.UpdatedBy = User.Identity.GetUserId();

                model.SenderId = User.Identity.GetUserId();
                model.Sender = _userService.GetById(User.Identity.GetUserId());

                var receiverEmail = model.ReceiverId;// model.ReceiverId nhận được từ view là mail người nhận chứ không phải Id 
                var receiver = _userService.GetByEmail(receiverEmail);
                model.ReceiverId = receiver.Id;
                model.Receiver = receiver;
                model.Status = MessageStatus.Sent;

                ViewBag.Message = _messageService.Add(model) ? "Gửi thành công" : "Gửi thất bại";
            }

            return RedirectToAction("Index");
        }

        public ActionResult View(int? id)
        {
            GetUserUnreadMassages();
            var message = _messageService.GetById(id ?? 1);
            if (message != null)
            {
                if(message.SenderId != User.Identity.GetUserId())//nếu user đọc mail không phải của user này viết thì sẽ cập nhật mail thành đã đọc
                {
                    message.Status = MessageStatus.Read;
                    _messageService.Update(message);
                }
            }
            return View(message);
        }

        public FileResult DownloadAttachedFile(string senderEmail, string fileName)
        {
            try
            {
                string senderId = senderEmail.Split('@')[0];

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath($"~/Attachment/{senderId}/{fileName}"));

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch
            {
                return null;
            }
        }

    }
}