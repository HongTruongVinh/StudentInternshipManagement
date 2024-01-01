using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Description;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Unity;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    public class StudentBaseController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IMessageService _messageService;

        public StudentBaseController()
        {
            _studentService = UnityConfig.Container.Resolve<IStudentService>();
            _messageService = UnityConfig.Container.Resolve<IMessageService>();
        }

        public StudentBaseController(IStudentService studentService, IMessageService messageService)
        {
            _studentService = studentService;
            _messageService = messageService;
        }

        public int CurrentStudentId => CurrentStudent.Id;

        public Models.Entities.Student CurrentStudent
        {
            get
            {
                var userName = CurrentUser.UserName;
                
                var currentStudent = _studentService.GetByUserName(userName);

                return currentStudent;
            }
        }

        protected IStudentService StudentService => _studentService;

        public void GetUserUnreadMassages()
        {
            ViewBag.UserMessages = _messageService.GetReceivedEmail(User.Identity.GetUserId()).Where(x => x.Status == Models.Constants.MessageStatus.Sent).ToList();
        }
    }
}