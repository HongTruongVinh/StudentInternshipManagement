using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : AdminBaseController
    {
        private readonly ILearningClassService _learningClassService;
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, IStudentClassService studentClassService,
            ILearningClassService learningClassService)
        {
            _studentService = studentService;
            _studentClassService = studentClassService;
            _learningClassService = learningClassService;
        }

        public ActionResult Index()
        {
            ViewBag.StudentClasses = _studentClassService.GetAll();
            ViewBag.LearningClasses = _learningClassService.GetAll();

            return View();
        }

        public ActionResult Students_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<StudentViewModel> studentViewModels = StudentViewModel.convertEntitiesToListViewModel(_studentService.GetAll().ToList());

            DataSourceResult result = studentViewModels.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Create([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //var student = StudentViewModel.convertViewModelToEntity(studentViewModel, _studentService);
                //_studentService.Add(student);//Hàm cũ hoạt động bị lỗi

                var result = AddUserService.AddStudent(new StudentInternshipManagement.Models.Contexts.WebContext(), viewModel, User.Identity.GetUserId());
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Update([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var student = StudentViewModel.convertViewModelToEntity(viewModel, _studentService);

                _studentService.Update(student);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Destroy([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = StudentViewModel.convertViewModelToEntity(viewModel, _studentService);

                _studentService.Delete(student);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetLearningClassList([DataSourceRequest] DataSourceRequest request, int studentId)
        {

            DataSourceResult result = _studentService.GetById(studentId).LearningClassStudents.ToDataSourceResult(
                request, student => new
                {
                    student.StudentId,
                    student.ClassId,
                    student.MidTermPoint,
                    student.EndTermPoint,
                    student.TotalPoint,
                    student.CreatedAt,
                    student.CreatedBy,
                    student.UpdatedAt,
                    student.UpdatedBy,
                    student.IsDeleted
                });

            return Json(result);
        }
    }
}