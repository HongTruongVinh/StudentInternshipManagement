using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class InternshipController : StudentBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IGroupService _groupService;
        private readonly IInternshipService _internshipService;
        private readonly ILearningClassService _learningClassService;
        private readonly IStudentService _studentService;
        private readonly ITrainingMajorService _trainingMajorService;
        private readonly ICompanyTrainingMajorService _companyTrainingMajorService;

        public InternshipController(IInternshipService internshipService, IStudentService studentService,
            ILearningClassService learningClassService, ICompanyService companyService,
            ITrainingMajorService trainingMajorService, IGroupService groupService, ICompanyTrainingMajorService companyTrainingMajorService)
        {
            _internshipService = internshipService;
            _studentService = studentService;
            _learningClassService = learningClassService;
            _companyService = companyService;
            _trainingMajorService = trainingMajorService;
            _groupService = groupService;
            _companyTrainingMajorService = companyTrainingMajorService;
        }

        public ActionResult Index()
        {
            GetUserUnreadMassages();
            ViewBag.Students = _studentService.GetAll();
            ViewBag.LearningClasses = _learningClassService.GetAll();
            ViewBag.Companies = _companyService.GetAll();
            ViewBag.TrainingMajors = _trainingMajorService.GetAll();
            return View();
        }

        public ActionResult Register()
        {
            GetUserUnreadMassages();
            if (IsInProcessRegistIntership())
            {
                return RedirectToAction("InProcessRegistIntership", "Notification");
            }

            ViewBag.Students = _studentService.GetAll();
            ViewBag.LearningClasses = _learningClassService.GetAll();
            ViewBag.Companies = _companyService.GetAll();
            ViewBag.TrainingMajors = _trainingMajorService.GetAll();

            var student = _studentService.GetByUserName(User.Identity.GetUserName());

            ViewBag.StudentName = student.User.FullName;
            ViewBag.DepartmentStudent = student.Class.Department.DepartmentName;

            return View();
        }

        public ActionResult Internships_Read([DataSourceRequest] DataSourceRequest request)
        {
            var internships = _internshipService.GetByStudent(CurrentStudentId);

            var result = internships.ToDataSourceResult(request, internship => new
            {
                internship.Id,
                internship.RegistrationDate,
                internship.Status,
                Student = internship.Student.User.FullName,
                Class = internship.Class.ClassName,
                Semester = internship.Class.SemesterId,
                Company = internship.Major.Company.CompanyName,
                TrainingMajor = internship.Major.TrainingMajor.TrainingMajorName,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.MidTermPoint,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.EndTermPoint,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.TotalPoint,
                Group = _groupService.GetByInternship(internship)?.GroupName,
                Teacher = _groupService.GetByInternship(internship)?.Teacher.User.FullName
            });

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Internships_Create(Internship internship)
        {
            if (ModelState.IsValid)
            {
                // StudentId gửi từ View tới server là mã sinh viên (username) chứ không phải Id của sinh viên 
                // trong table sinh viên cho nên phải sửa lại mới có thể lưu được
                internship.StudentId = _studentService.GetByUserName(User.Identity.GetUserName()).Id;

                internship.Major = _companyTrainingMajorService.GetById(internship.TrainingMajorId); //Nếu không gán dữ liệu 
                // cho thuộc tính này thì cột Major_Id trong CSDL sẽ bị null và khi load danh sách lên sẽ lỗi 
                
                internship.CreatedAt = DateTime.Now;
                internship.UpdatedAt = DateTime.Now;
                internship.CreatedBy = User.Identity.GetUserId();
                internship.UpdatedBy = User.Identity.GetUserId();

                ViewBag.Message = _internshipService.Add(internship) ? "Thành công" : "Thất bại";
            }
            return RedirectToAction("Index");
        }

        bool IsInProcessRegistIntership()
        {
            var student = _studentService.GetByUserName(User.Identity.GetUserName());
            var listStudentIntership = _internshipService.GetByStudent(student.Id).ToList();
            var countInProcess = listStudentIntership.Where(x => x.Status == Models.Constants.InternshipStatus.Registered).ToList().Count;
            if (countInProcess > 0)
            {
                return true;
            }
            return false;
        }
    }
}