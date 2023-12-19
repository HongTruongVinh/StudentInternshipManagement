using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Services;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : AdminBaseController
    {
        private readonly IAdminService _adminService;
        private readonly IDepartmentService _departmentService;

        public AdminController(IAdminService adminService, IDepartmentService departmentService)
        {
            _adminService = adminService;
            _departmentService = departmentService;
        }

        public ActionResult Index()
        {
            ViewBag.Departments = _departmentService.GetAll();

            return View();
        }

        public ActionResult Admins_Read([DataSourceRequest] DataSourceRequest request)
        {
            //DataSourceResult result = _adminService.GetAll().ToDataSourceResult(request);

            var viewModels = AdminViewModel.convertEntitiesToListViewModel(_adminService.GetAll().ToList());

            DataSourceResult result = viewModels.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admins_Create([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.AdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //_adminService.Add(admin);
                var result = AddUserService.AddAdmin(new StudentInternshipManagement.Models.Contexts.WebContext(), viewModel, User.Identity.GetUserId());
            }

            return Json(new[] {viewModel}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admins_Update([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.AdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var admin = AdminViewModel.convertViewModelToEntity(viewModel, _adminService);
                _adminService.Update(admin);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admins_Destroy([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.AdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var admin = AdminViewModel.convertViewModelToEntity(viewModel, _adminService);
                _adminService.Delete(admin);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }
    }
}