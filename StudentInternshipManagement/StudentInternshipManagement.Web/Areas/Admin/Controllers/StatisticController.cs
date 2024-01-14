using StudentInternshipManagement.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class StatisticController : AdminBaseController
    {


        public StatisticController() 
        {

        }
        
        public ActionResult Index()
        {
            ViewBag.Data = "Value,Value1,Value2,Value3"; //list of strings that you need to show on the chart. as mentioned in the example from c-sharpcorner
            ViewBag.ObjectName = "Test,Test1,Test2,Test3";

            ViewBag.Test = new List<ChartItemViewModel>()
            {
                new ChartItemViewModel() { Name = "1", Number = 15, Percent = 20, Color = "#9de219" },
                new ChartItemViewModel() { Name = "1", Number = 15, Percent = 20, Color = "#90cc38"  },
                new ChartItemViewModel() { Name = "1", Number = 30, Percent = 20, Color = "#068c35"  },
                new ChartItemViewModel() { Name = "1", Number = 30, Percent = 20, Color = "#006634"  },
            };

            return View();
        }


    }
}