using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;

namespace StudentInternshipManagement.Web.Areas.Teacher.Controllers
{
    public class GroupController : TeacherBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IGroupService _groupService;
        private readonly ILearningClassService _learningClassService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ITrainingMajorService _trainingMajorService;

        public GroupController(IGroupService groupService, ITrainingMajorService trainingMajorService,
            ILearningClassService learningClassService, ICompanyService companyService, IStudentService studentService,
            ITeacherService teacherService)
        {
            _groupService = groupService;
            _trainingMajorService = trainingMajorService;
            _learningClassService = learningClassService;
            _companyService = companyService;
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public ActionResult Index()
        {
            GetUserUnreadMassages();
            ViewBag.Companies = _companyService.GetAll();
            ViewBag.TrainingMajors = _trainingMajorService.GetAll();
            ViewBag.Students = _studentService.GetAll();
            ViewBag.Teachers = _teacherService.GetAll();
            ViewBag.Classes = _learningClassService.GetAll();
            return View();
        }

        public ActionResult Groups_Read([DataSourceRequest] DataSourceRequest request)
        {
            var teacher = _teacherService.GetByUserName(User.Identity.GetUserName());
            var groups = _groupService.GetByTeacher(teacher.Id);
            var result = groups.ToDataSourceResult(request, group => new
            {
                group.Id,
                group.GroupName,
                group.ClassId,
                group.CompanyId,
                group.TrainingMajorId,
                group.LeaderId,
                group.TeacherId
            });

            return Json(result);
        }

        public ActionResult GetStudentList(int groupId, [DataSourceRequest] DataSourceRequest request)
        {
            var listStudents = StudentViewModel.convertEntitiesToListViewModel(_groupService.GetById(groupId).Members.ToList());

            var result = listStudents.ToDataSourceResult(request, student => new
            {
                student.Id,
                student.UserName,
                student.FullName,
                student.BirthDate,
                student.Address,
                student.Email,
                student.Phone,
                student.Cpa,
                student.ClassId
            });

            return Json(result);
        }
    }
}