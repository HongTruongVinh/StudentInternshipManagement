using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class NotificationController : StudentBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IGroupService _groupService;
        private readonly IInternshipService _internshipService;
        private readonly ILearningClassService _learningClassService;
        private readonly IStudentService _studentService;
        private readonly ITrainingMajorService _trainingMajorService;

        public NotificationController(IInternshipService internshipService, IStudentService studentService,
            ILearningClassService learningClassService, ICompanyService companyService,
            ITrainingMajorService trainingMajorService, IGroupService groupService)
        {
            _internshipService = internshipService;
            _studentService = studentService;
            _learningClassService = learningClassService;
            _companyService = companyService;
            _trainingMajorService = trainingMajorService;
            _groupService = groupService;
        }

        // GET: Student/Notification
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InProcessRegistIntership()
        {
            var student = _studentService.GetByUserName(User.Identity.GetUserName());

            ViewBag.StudentName = student.User.FullName;

            return View();
        }
    }
}