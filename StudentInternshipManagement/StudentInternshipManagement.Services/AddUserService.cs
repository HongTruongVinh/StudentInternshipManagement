using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentInternshipManagement.Models.Contexts;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services
{
    enum MinMaxStudentId
    {
        Min = 1001,
        Max = 9999
    }

    public class AddUserService
    {
        public AddUserService() { }

        public static bool AddTeacher(WebContext context, TeacherViewModel v, string createdBy)
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var user = new ApplicationUser
                {

                    UserName = v.UserName,
                    Email = v.UserName + "@yopmail.com",
                    Avatar = "DefaultAvatarTeacher.png",
                    FullName = v.FullName,
                    Address = v.Address,
                    BirthDate = v.BirthDate,
                    Phone = v.Phone,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                };

                if (userManager.FindByEmail(user.Email) != null)
                {
                    if (v.UserName == userManager.FindByEmail(user.Email).UserName)
                    {
                        return false;
                    }
                }

                userManager.Create(user, "Ab=123456789");

                var get_user = userManager.FindByEmail(user.Email);

                userManager.AddToRole(get_user.Id, "Teacher");

                var newTeacher = new Teacher()
                {
                    Department = context.Departments.Where(d => d.Id == v.DepartmentId).FirstOrDefault(),
                    DepartmentId = v.DepartmentId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user.Id,
                    CreatedBy = createdBy,
                };

                context.Teachers.Add(newTeacher);

                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddAdmin(WebContext context, AdminViewModel v, string createdBy)
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (v.UserName == "" || v.UserName == null)
                {
                    v.UserName = DateTime.Now.ToString("yyyy") + new Random().Next((int)MinMaxStudentId.Min, (int)MinMaxStudentId.Max);
                }

                var user = new ApplicationUser
                {
                    UserName = v.UserName,
                    Email = v.UserName + "@yopmail.com",
                    Avatar = "DefaultAvatarStudent.png",
                    FullName = v.FullName,
                    Address = v.Address,
                    BirthDate = v.BirthDate,
                    Phone = v.Phone,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                };

                if (userManager.FindByEmail(user.Email) != null)
                {
                    if (v.UserName == userManager.FindByEmail(user.Email).UserName)
                    {
                        return false;
                    }
                }

                userManager.Create(user, "Ab=123456789");

                var get_user = userManager.FindByEmail(user.Email);

                userManager.AddToRole(get_user.Id, "Admin");

                var newStudent = new Admin()
                {
                    Department = context.Departments.Where(d => d.Id == v.DepartmentId).FirstOrDefault(),
                    DepartmentId = v.DepartmentId,

                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user.Id,
                    CreatedBy = createdBy,
                };

                context.Admins.Add(newStudent);

                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;

            }
        }

        public static bool AddStudent(WebContext context, StudentViewModel v, string createdBy, string program = "Kỹ sư")
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (v.UserName == "" || v.UserName == null)
                {
                    v.UserName = DateTime.Now.ToString("yyyy") + new Random().Next((int)MinMaxStudentId.Min, (int)MinMaxStudentId.Max);
                }

                var user = new ApplicationUser
                {
                    UserName = v.UserName,
                    Email = v.UserName + "@yopmail.com",
                    Avatar = "DefaultAvatarStudent.png",
                    FullName = v.FullName,
                    Address = v.Address,
                    BirthDate = v.BirthDate,
                    Phone = v.Phone,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                };

                if (userManager.FindByEmail(user.Email) != null)
                {
                    if (v.UserName == userManager.FindByEmail(user.Email).UserName)
                    {
                        return false;
                    }
                }

                userManager.Create(user, "Ab=123456789");

                var get_user = userManager.FindByEmail(user.Email);

                userManager.AddToRole(get_user.Id, "Student");

                var newStudent = new Student()
                {
                    Cpa = (v.Cpa * 4) / 10,
                    ClassId = v.ClassId,
                    
                    Program = program,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user.Id,
                    CreatedBy = createdBy,
                };

                context.Students.Add(newStudent);

                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
