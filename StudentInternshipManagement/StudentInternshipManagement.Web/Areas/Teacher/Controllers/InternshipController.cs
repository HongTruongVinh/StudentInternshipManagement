using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;

namespace StudentInternshipManagement.Web.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class InternshipController : TeacherBaseController
    {
        private readonly ILearningClassStudentService _learningClassStudentService;
        private readonly IStudentService _studentService;
        private readonly ILearningClassService _learningClassService;

        public InternshipController(ILearningClassStudentService learningClassStudentService, IStudentService studentService, ILearningClassService learningClassService)
        {
            _learningClassStudentService = learningClassStudentService;
            _studentService = studentService;
            _learningClassService = learningClassService;
        }

        public ActionResult Index()
        {
            //ViewBag.Students = _studentService.GetAll();
            //ViewBag.LearningClasses = _learningClassService.GetAll();
            return View();
        }

        public ActionResult LearningClassStudents_Read([DataSourceRequest]DataSourceRequest request)
        {
            var listLearningClassStudent = LearningClassStudentViewModel.convertEntitiesToListViewModel(_learningClassStudentService.GetByTeacher(CurrentTeacherId).ToList());

            DataSourceResult result = listLearningClassStudent.ToDataSourceResult(request, learningClassStudent => new {
                Id = learningClassStudent.Id,
                StudentCode = learningClassStudent.StudentCode,
                FullName = learningClassStudent.FullName,
                ClassName = learningClassStudent.ClassName,
                ClassId = learningClassStudent.ClassId,
                StudentId = learningClassStudent.StudentId,
                MidTermPoint = learningClassStudent.MidTermPoint,
                EndTermPoint = learningClassStudent.EndTermPoint,
                TotalPoint = learningClassStudent.TotalPoint,
                CreatedAt = learningClassStudent.CreatedAt,
                UpdatedAt = learningClassStudent.UpdatedAt,
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LearningClassStudents_Update([DataSourceRequest]DataSourceRequest request, LearningClassStudentViewModel learningClassStudent)
        {
            if (ModelState.IsValid)
            {
                //var entity = new LearningClassStudent
                //{
                //    ClassId = learningClassStudent.ClassId,
                //    StudentId = learningClassStudent.StudentId,
                //    MidTermPoint = learningClassStudent.MidTermPoint,
                //    EndTermPoint = learningClassStudent.EndTermPoint,
                //    TotalPoint = learningClassStudent.TotalPoint
                //};
                //entity.TotalPoint = 0.3f * entity.MidTermPoint + 0.7f * entity.EndTermPoint;
                //if (entity.TotalPoint != null)
                //    entity.TotalPoint = (float) Math.Round(entity.TotalPoint.Value, 1);

                var entity = _learningClassStudentService.GetById(learningClassStudent.Id);

                entity.MidTermPoint = learningClassStudent.MidTermPoint;
                entity.EndTermPoint = learningClassStudent.EndTermPoint;
                entity.TotalPoint = 0.3f * entity.MidTermPoint + 0.7f * entity.EndTermPoint;
                if (entity.TotalPoint != null)
                    entity.TotalPoint = (float)Math.Round(entity.TotalPoint.Value, 1);

                _learningClassStudentService.Update(entity);
            }

            return Json(new[] { learningClassStudent }.ToDataSourceResult(request, ModelState));
        }
    }
}
