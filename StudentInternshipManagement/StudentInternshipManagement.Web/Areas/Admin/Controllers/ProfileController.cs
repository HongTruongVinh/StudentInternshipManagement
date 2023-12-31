﻿using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileController : AdminBaseController
    {
        private readonly IAdminService _adminService;

        public ProfileController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: Student/Profile
        public ActionResult Index()
        {
            global::StudentInternshipManagement.Models.Entities.Admin admin = _adminService.GetById(CurrentAdminId);
            ViewBag.Email = CurrentUser.Email;
            return View(admin);
        }

        public ActionResult Edit()
        {
            global::StudentInternshipManagement.Models.Entities.Admin admin = _adminService.GetById(CurrentAdminId);
            var adminViewModel = new AdminViewModel(admin);
            return View(adminViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, global::StudentInternshipManagement.Services.ViewModel.AdminViewModel viewModel)
        {
            viewModel.UpdatedBy = CurrentUser.Id;
            if (ModelState.IsValid)
            {
                var model = AdminViewModel.convertViewModelToEntity(viewModel, _adminService);

                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Images/avatars/"),
                        $"{model.User.UserName}{extension}");
                    file.SaveAs(physicalPath);
                    model.User.Avatar = $"{model.User.UserName}{extension}";
                }

                ViewBag.Message = _adminService.Update(model) ? "Thành công" : "Thất bại";
            }

            return RedirectToAction("Index");
        }
    }
}