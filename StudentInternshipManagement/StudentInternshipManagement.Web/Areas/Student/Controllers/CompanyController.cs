using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;

namespace StudentInternshipManagement.Web.Areas.Student.Controllers
{
    [Authorize (Roles="Student")]
    public class CompanyController : StudentBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly ITrainingMajorService _trainingMajorService;
        private readonly ICompanyTrainingMajorService _companyTrainingMajorService;

        public CompanyController(ICompanyService companyService, ITrainingMajorService trainingMajorService, ICompanyTrainingMajorService companyTrainingMajorService)
        {
            _companyService = companyService;
            _trainingMajorService = trainingMajorService;
            _companyTrainingMajorService = companyTrainingMajorService;

        }

        public ActionResult Index()
        {
            ViewBag.TrainingMajors = _trainingMajorService.GetAll();

            return View();
        }

        public ActionResult Companies_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = _companyService.GetAll().ToDataSourceResult(request, company => new
            {
                company.Id,
                company.CompanyName,
                company.CompanyDescription,
                company.Address,
                company.Email,
                company.Phone,
                company.CreatedAt,
                company.CreatedBy,
                company.UpdatedAt,
                company.UpdatedBy,
                company.IsDeleted
            });

            return Json(result);
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
    }
}
