using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProfileController : StudentBaseController
    {
        private readonly IStudentService _studentService;

        public ProfileController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student/Profile
        public ActionResult Index()
        {
            GetUserUnreadMassages();
            var student = _studentService.GetById(CurrentStudentId);
            ViewBag.Email = CurrentUser.Email;
            var studentViewModel = new StudentViewModel(student);
            return View(studentViewModel);
        }

        public ActionResult Edit()
        {
            GetUserUnreadMassages();
            var student = _studentService.GetById(CurrentStudentId);
            var studentViewModel = new StudentViewModel(student);
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            viewModel.UpdatedBy = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var model = StudentViewModel.convertViewModelToEntity(viewModel, _studentService);
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Images/avatars/"), $"{model.User.Avatar}{extension}");
                    file.SaveAs(physicalPath);
                    model.User.Avatar = $"{model.User.UserName}{extension}";
                }

                ViewBag.Message = _studentService.Update(model) ? "Thành công" : "Thất bại";
            }

            return RedirectToAction("Index");
        }
    }
}