using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Hangfire.Dashboard;
using Hangfire;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Unity;
using Unity.Lifetime;
using StudentInternshipManagement.Models.Contexts;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Kendo.Mvc.Extensions;

namespace StudentInternshipManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserService _userService;

        
        public BaseController()
        {
            //var initial = new DataInitializer(new WebContext()); // Dòng lệnh chạy seed

            //if(UnityConfig.Container.IsRegistered<DashboardOptions>())

            UnityConfig.Container.RegisterInstance<DashboardOptions>(new DashboardOptions()
            {
                Authorization = new[] { UnityConfig.Container.Resolve<IDashboardAuthorizationFilter>() }
            }, new SingletonLifetimeManager());

            _userService = UnityConfig.Container.Resolve<IUserService>();

            //if(_userService.GetAll().Count() == 0)
            //{
            //    var initial = new DataInitializer(new WebContext()); // nếu trong database chưa có dữ liệu trong bảng user nào thì chạy seed 
            //}

        }

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }

        public string CurrentUserId => User.Identity.GetUserId();

        public ApplicationUser CurrentUser
        {
            get
            {
                Initialize(HttpContext.RequestContext());
                string id = User.Identity.GetUserId();
                return _userService.GetById(id);
            }
        }

        public string CurrentRole
        {
            get
            {
                return ((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).FirstOrDefault();
            }
        }

        protected IUserService UserService => _userService;



        /// <summary>
        /// Hàm lưu lại HttpContext từ controller con kế thừa BaseController
        /// </summary>
        public new HttpContextBase HttpContext
        {
            get
            {
                HttpContextWrapper context =
                    new HttpContextWrapper(System.Web.HttpContext.Current);
                return (HttpContextBase)context;
            }
        }

        /// <summary>
        /// Hàm chống User trong System.Web.Mvc.Controller bị null
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Now you can access the HttpContext and User
            if (User.Identity.IsAuthenticated)
            {

            }
        }
    }
}