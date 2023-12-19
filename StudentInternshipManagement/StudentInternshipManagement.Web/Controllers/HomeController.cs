using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Unity;

namespace StudentInternshipManagement.Web.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            //_service = UnityConfig.Container.Resolve<INewsService>();

            List<News> products = UnityConfig.Container.Resolve<INewsService>().GetAll().ToList();

            return View(products);
        }
    }
}