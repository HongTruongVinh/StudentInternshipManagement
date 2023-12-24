using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInternshipManagement.Web.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : TeacherBaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService, IDepartmentService departmentService)
        {
            _teacherService = teacherService;
            _departmentService = departmentService;
        }

        // GET: Student/Teacher
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
    }
}