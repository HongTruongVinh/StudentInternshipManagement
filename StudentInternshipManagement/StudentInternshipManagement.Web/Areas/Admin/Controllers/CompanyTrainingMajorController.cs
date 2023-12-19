using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyTrainingMajorController : AdminBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyTrainingMajorService _companyTrainingMajorService;
        private readonly ITrainingMajorService _trainingMajorService;

        public CompanyTrainingMajorController(ICompanyTrainingMajorService companyTrainingMajorService,
            ITrainingMajorService trainingMajorService, ICompanyService companyService)
        {
            _companyTrainingMajorService = companyTrainingMajorService;
            _trainingMajorService = trainingMajorService;
            _companyService = companyService;
        }

        public ActionResult Index()
        {
            ViewBag.Companies = _companyService.GetAll();
            ViewBag.TrainingMajors = _trainingMajorService.GetAll();
            return View();
        }

        public ActionResult CompanyTrainingMajors_Read([DataSourceRequest] DataSourceRequest request, int? companyId,
            int? majorId)
        {
            IQueryable<CompanyTrainingMajor> datasource;

            if (companyId != null)
                datasource = _companyTrainingMajorService.GetByCompany(companyId.Value);
            else if (majorId != null)
                datasource = _companyTrainingMajorService.GetByTrainingMajor(majorId.Value);
            else
                datasource = _companyTrainingMajorService.GetAll();

            var listViewModel = CompanyTrainingMajorViewModel.convertEntitesToListViewModel(datasource.ToList());

            DataSourceResult result = listViewModel.ToDataSourceResult(request, companyTrainingMajor => new
            {
                companyTrainingMajor.TrainingMajorName,
                companyTrainingMajor.Id,
                companyTrainingMajor.CompanyId,
                companyTrainingMajor.TrainingMajorId,

                companyTrainingMajor.TotalTraineeCount,
                companyTrainingMajor.AvailableTraineeCount,
                companyTrainingMajor.CreatedAt,
                companyTrainingMajor.CreatedBy,
                companyTrainingMajor.UpdatedAt,
                companyTrainingMajor.UpdatedBy,
                companyTrainingMajor.IsDeleted
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompanyTrainingMajors_Create([DataSourceRequest] DataSourceRequest request,
            CompanyTrainingMajor companyTrainingMajor)
        {
            if (ModelState.IsValid)
            {
                companyTrainingMajor.CreatedAt = DateTime.Now;
                companyTrainingMajor.UpdatedAt = DateTime.Now;
                companyTrainingMajor.CreatedBy = User.Identity.GetUserId();
                _companyTrainingMajorService.Add(companyTrainingMajor);
            }

            return Json(new[] {companyTrainingMajor}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompanyTrainingMajors_Update([DataSourceRequest] DataSourceRequest request,
            CompanyTrainingMajor companyTrainingMajor)
        {
            if (ModelState.IsValid)
            {
                companyTrainingMajor.UpdatedBy = User.Identity.GetUserId();
                _companyTrainingMajorService.Update(companyTrainingMajor);
            }

            return Json(new[] {companyTrainingMajor}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompanyTrainingMajors_Destroy([DataSourceRequest] DataSourceRequest request,
            CompanyTrainingMajor companyTrainingMajor)
        {
            if (ModelState.IsValid)
            {
                _companyTrainingMajorService.Delete(companyTrainingMajor);
            }

            return Json(new[] {companyTrainingMajor}.ToDataSourceResult(request, ModelState));
        }
    }
}