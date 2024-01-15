using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticController : AdminBaseController
    {
        private readonly ILearningClassService _learningClassService;
        private readonly ISemesterService _semesterService;

        public StatisticController(ILearningClassService learningClassService, ISemesterService semesterService)
        {
            _learningClassService = learningClassService;
            _semesterService = semesterService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDataChart(int semesterId)
        {
            try
            {
                if(semesterId == -1)
                {
                    semesterId = _semesterService.GetLatest().Id;
                }

                List<float?> points = new List<float?>();

                var leaningClasses = _learningClassService.GetAll().Where(x => x.SemesterId == semesterId).ToList();

                foreach (var leaningClass in leaningClasses)
                {
                    var learningClassPoints = leaningClass.LearningClassStudents.ToList().Select(s => new { s.TotalPoint }).ToList();
                    foreach (var item in learningClassPoints)
                    {
                        points.Add(item.TotalPoint);
                    }
                }
                
                var pieChartData = ChartItemViewModel.CreatePieChartData(points);

                var barChartData = ChartItemViewModel.CreateBarChartData(points);

                if (points.Count != 0)
                {
                    return Json(new
                    {
                        status = true,
                        pieChartData = pieChartData,
                        barChartData = barChartData,
                        semesterId = semesterId,
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        } 
    }
}