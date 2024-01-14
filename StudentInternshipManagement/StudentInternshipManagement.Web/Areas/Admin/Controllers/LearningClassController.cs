using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LearningClassController : AdminBaseController
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
            ViewBag.LearningClasses = _learningClassService.GetAll();

            return View();
        }

        public ActionResult LearningClasses_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _learningClassService.GetAll().ToDataSourceResult(request, learningClass => new
            {
                learningClass.Id,
                learningClass.ClassName,
                learningClass.SubjectId,
                learningClass.SemesterId,
                learningClass.CreatedAt,
                learningClass.CreatedBy,
                learningClass.UpdatedAt,
                learningClass.UpdatedBy,
                learningClass.IsDeleted
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LearningClasses_Create([DataSourceRequest] DataSourceRequest request,
            LearningClass learningClass)
        {
            if (ModelState.IsValid)
            {
                var entity = new LearningClass
                {
                    ClassName = learningClass.ClassName,
                    SubjectId = learningClass.SubjectId,
                    SemesterId = learningClass.SemesterId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId(),
                    UpdatedAt = DateTime.Now,
                };

                _learningClassService.Add(entity);
            }

            var result = _learningClassService.GetAll().Where(x => x.ClassName == learningClass.ClassName).FirstOrDefault();

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LearningClasses_Update([DataSourceRequest] DataSourceRequest request,
            LearningClass learningClass)
        {
            if (ModelState.IsValid)
            {
                _learningClassService.Update(learningClass);
            }

            return Json(new[] {learningClass}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LearningClasses_Destroy([DataSourceRequest] DataSourceRequest request,
            LearningClass learningClass)
        {
            if (ModelState.IsValid)
            {
                _learningClassService.Delete(learningClass);
            }

            return Json(new[] {learningClass}.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetStudentList(int classId, [DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _learningClassService.GetById(classId).LearningClassStudents.ToDataSourceResult(
                request, student => new
                {
                    student.StudentId,
                    student.ClassId,
                    student.MidTermPoint,
                    student.EndTermPoint,
                    student.TotalPoint
                });

            return Json(result);
        }

        public ActionResult GetTrainingMajorList(int classId, [DataSourceRequest] DataSourceRequest request)
        {
            Subject subject = _subjectService.GetById(_learningClassService.GetById(classId).SubjectId);
            DataSourceResult result = subject.TrainingMajors.ToDataSourceResult(request, trainingMajor => new
            {
                TrainingMajorId = trainingMajor.Id,
                trainingMajor.TrainingMajorName,
                trainingMajor.SubjectId
            });

            return Json(result);
        }

        /// <summary>
        /// Hàm này phục vụ cho View Grade 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllLearningClasses(int SemesterId)
        {
            var result = _learningClassService.GetAll().Where(x=>x.SemesterId == SemesterId).Select(x => new
            {
                LearningClassId = x.Id,
                ClassName = x.ClassName,
                SemesterId = x.SemesterId,
                x.SubjectId,
                x.Subject.SubjectName,
            });
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}