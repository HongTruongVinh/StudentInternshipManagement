using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Web.Controllers;
using System.Linq;
using Unity;

namespace StudentInternshipManagement.Web.Areas.Teacher.Controllers
{
    public class TeacherBaseController : BaseController
    {
        private readonly ITeacherService _teacherService;
        private readonly IMessageService _messageService;

        public TeacherBaseController()
        {
            _teacherService = UnityConfig.Container.Resolve<ITeacherService>();
            _messageService = UnityConfig.Container.Resolve<IMessageService>();
        }

        public TeacherBaseController(ITeacherService teacherService, IMessageService messageService)
        {
            _teacherService = teacherService;
            _messageService = messageService;
        }

        public int CurrentTeacherId => CurrentTeacher.Id;

        public Models.Entities.Teacher CurrentTeacher
        {
            get
            {

                var userName = CurrentUser.UserName;
                return _teacherService.GetByUserName(userName);
            }
        }

        protected ITeacherService TeacherService => _teacherService;

        public void GetUserUnreadMassages()
        {
            ViewBag.UserMessages = _messageService.GetReceivedEmail(User.Identity.GetUserId()).Where(x=>x.Status == Models.Constants.MessageStatus.Sent).ToList();
        }
    }
}