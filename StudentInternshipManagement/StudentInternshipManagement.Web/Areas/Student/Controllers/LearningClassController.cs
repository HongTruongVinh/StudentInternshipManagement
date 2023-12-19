using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services.Implements;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class LearningClassController : StudentBaseController
    {
        private readonly ILearningClassService _learningClassService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public LearningClassController(ILearningClassService learningClassService, IStudentService studentService,
            ISubjectService subjectService, ISemesterService semesterService)
        {
            _learningClassService = learningClassService;
            _studentService = studentService;
            _subjectService = subjectService;
            _semesterService = semesterService;
        }

        public ActionResult Index()
        {
            ViewBag.Subjects = _subjectService.GetAll();
            ViewBag.Semesters = _semesterService.GetAll();
            ViewBag.Students = _studentService.GetAll();
            return null;
        }

        public ActionResult LearningClasses_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = _studentService.GetLearningClassBySemesterList(CurrentStudentId).ToDataSourceResult(request, learningClass =>
                new
                {
                    ClassId = learningClass.Id,
                    learningClass.ClassName,
                    learningClass.SubjectId,
                    learningClass.SemesterId
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            //code gốc: trả về danh sách lớp (những lớp trong kì mới nhất) mà sinh viên thuộc lớp học đó (tức là trong 
            // danh sách sv của lớp đã chứa tên của sv này)
            //var result = _studentService.GetLearningClassBySemesterList(CurrentStudentId);
            // với cách xử lý gốc thì sẽ cần phải thêm chức năng đăng ký lớp học và duyệt lớp học
            // (như kiểu việc đăng ký học phần)




            //code mới do Vinh xử lý: khác với code gốc là trả về danh sách lớp (những  
            //  lớp thuộc khoa của sv này trong kì mới nhất) mà sinh viên không cần phải thuộc những lớp học đó 
            //
            var student = _studentService.GetById(CurrentStudentId);
            var lastestSemester = _semesterService.GetLatest().Id;
            var result = _learningClassService.GetAllLearningClassToRigisterInternship(lastestSemester, student);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTrainingMajorList(int classId, [DataSourceRequest] DataSourceRequest request)
        {
            var subjectId = _learningClassService.GetById(classId).SubjectId;
            var subject = _subjectService.GetById(subjectId);
            var x = subject.TrainingMajors.ToList();
            return Json(subject.TrainingMajors, JsonRequestBehavior.AllowGet);
        }
    }
}