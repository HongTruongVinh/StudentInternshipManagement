using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    public class TeacherController : AdminBaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService, IDepartmentService departmentService)
        {
            _teacherService = teacherService;
            _departmentService = departmentService;
        }

        public ActionResult Index()
        {
            ViewBag.Departments = _departmentService.GetAll();
            return View();
        }

        public ActionResult Teachers_Read([DataSourceRequest] DataSourceRequest request)
        {
            var viewModels = TeacherViewModel.convertEntitesToListViewModel(_teacherService.GetAll().ToList());

            DataSourceResult result = viewModels.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Teachers_Create([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.TeacherViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var result = AddUserService.AddTeacher(new StudentInternshipManagement.Models.Contexts.WebContext(), viewModel, User.Identity.GetUserId());

                //_teacherService.Add(teacher);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Teachers_Update([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.TeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var teacher = TeacherViewModel.convertViewModelToEntity(viewModel, _teacherService);
                _teacherService.Update(teacher);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Teachers_Destroy([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.TeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var teacher = TeacherViewModel.convertViewModelToEntity(viewModel, _teacherService);
                _teacherService.Delete(teacher);
            }

            return Json(new[] {viewModel}.ToDataSourceResult(request, ModelState));
        }
    }
}