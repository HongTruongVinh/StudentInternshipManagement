using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentInternshipManagement.Models.Contexts;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
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

    //public interface IAddUserService
    //{
    //    bool AddTeacher(WebContext context, TeacherViewModel v, string createdBy);
    //}

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
                    Email = v.UserName + "@gm.uit.edu.vn",
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
                    Email = v.UserName + "@gm.uit.edu.vn",
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
                    Email = v.UserName + "@gm.uit.edu.vn",
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

        public static Dictionary<string, string> AddStudentsByList(WebContext context, List<StudentViewModel> listStudentViewModel, string createdBy, string program = "Kỹ sư")
        {
            Dictionary<string,string> results = new Dictionary<string,string>();

            foreach (var student in listStudentViewModel)
            {
                var studentName = student.FullName + " " + student.BirthDate.ToString("dd/MM/yyyy") + " " + student.Phone;

                try
                {
                    bool successful = AddStudent(context, student, createdBy, program = "Kỹ sư");

                    if (successful == true)
                    {
                        results.Add(studentName, "success");
                    }
                    else
                    {
                        results.Add(studentName, "failure");
                    }
                }
                catch 
                {
                    try
                    {
                        results.Add(studentName, "failure"); // Nếu có 2 trường giữ liệu giống hệt nhau và bị lỗi thì 
                                                             // Dictionary sẽ bị lỗi vì trùng key. Nên nếu bị trùng key thì sẽ ko làm gì cả 
                    }
                    catch { }
                }
            }

            return results;
        }

        public static string GenerateStudentUserName(IStudentService studentService)
        {
            string userName = "1000";

            var lastStudent = studentService.GetAll().ToList().LastOrDefault();

            var lastStudentUserName = lastStudent.User.UserName;

            string year = lastStudentUserName.Substring(0, 4);

            string numberStudent = lastStudentUserName.Substring(4);

            if (year == DateTime.Now.ToString("yyyy"))
            {
                userName = (int.Parse(numberStudent) + 1).ToString();
            }
            else
            {
                userName = DateTime.Now.ToString("yyyy") + (int.Parse(userName) + 1).ToString();
            }


            return userName;
        }
    }
}
