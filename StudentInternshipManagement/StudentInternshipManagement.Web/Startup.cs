﻿using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using StudentInternshipManagement.Models.Contexts;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Web;
using Unity;
using Unity.Lifetime;
using System.Reflection;
using Autofac;

[assembly: OwinStartup(typeof(Startup))]
namespace StudentInternshipManagement.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            

            UnityConfig.Container.RegisterInstance(app.GetDataProtectionProvider());

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationSignInManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(DependencyResolver.Current.GetService<CookieAuthenticationOptions>());

            GlobalConfiguration.Configuration.UseSqlServerStorage("StudentInternshipManagement");
            app.UseHangfireDashboard("/jobs", UnityConfig.Container.Resolve<DashboardOptions>());
            app.UseHangfireServer();
        }

    }
}
