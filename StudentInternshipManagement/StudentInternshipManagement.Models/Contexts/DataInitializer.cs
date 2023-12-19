using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentInternshipManagement.Models.Constants;
using StudentInternshipManagement.Models.Entities;

namespace StudentInternshipManagement.Models.Contexts
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<WebContext>
    {
        public DataInitializer(WebContext context)
        {
            Seed(context);
        }
        protected override void Seed(WebContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("Teacher"));
            roleManager.Create(new IdentityRole("Student"));

            var user = new ApplicationUser
            {
                UserName = "20131070",
                Email = "20131070@yopmail.com",
                Avatar = "20131070.png",
                FullName = "Trần Văn Đức",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user, "Ab=123456789");

            var get_user = userManager.FindByEmail(user.Email);

            userManager.AddToRole(get_user.Id, "Student");

            var user2 = new ApplicationUser
            {
                UserName = "20130325",
                Email = "20130325@yopmail.com",
                Avatar = "20130325.png",
                FullName = "Cao Thị Ngân",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user2, "Ab=123456789");

            var get_user2 = userManager.FindByEmail(user2.Email);

            userManager.AddToRole(get_user2.Id, "Student");

            var user3 = new ApplicationUser
            {
                UserName = "20134713",
                Email = "20134713@yopmail.com",
                Avatar = "20134713.png",
                FullName = "Trần Danh Hoàn",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user3, "Ab=123456789");

            var get_user3 = userManager.FindByEmail(user3.Email);

            userManager.AddToRole(get_user3.Id, "Student");

            var user4 = new ApplicationUser
            {
                UserName = "20133847",
                Email = "20133847@yopmail.com",
                Avatar = "20133847.png",
                FullName = "Trần Đức Mạnh",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user4, "Ab=123456789");

            var get_user4 = userManager.FindByEmail(user4.Email);

            userManager.AddToRole(get_user4.Id, "Student");

            var user5 = new ApplicationUser
            {
                UserName = "20132820",
                Email = "20132820@yopmail.com",
                Avatar = "20132820.png",
                FullName = "Hà Văn Hoàn",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user5, "Ab=123456789");

            var get_user5 = userManager.FindByEmail(user5.Email);

            userManager.AddToRole(get_user5.Id, "Student");

            var user6 = new ApplicationUser
            {
                UserName = "20132231",
                Email = "20132231@yopmail.com",
                Avatar = "20132231.png",
                FullName = "Phạm Anh Tân",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user6, "Ab=123456789");

            var get_user6 = userManager.FindByEmail(user6.Email);

            userManager.AddToRole(get_user6.Id, "Student");

            var user7 = new ApplicationUser
            {
                UserName = "20130707",
                Email = "20130707@yopmail.com",
                Avatar = "20130707.png",
                FullName = "Triệu Văn Dũng",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user7, "Ab=123456789");

            var get_user7 = userManager.FindByEmail(user7.Email);

            userManager.AddToRole(get_user7.Id, "Student");

            var user8 = new ApplicationUser
            {
                UserName = "20132558",
                Email = "20132558@yopmail.com",
                Avatar = "20132558.png",
                FullName = "Mai Thị Giang",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user8, "Ab=123456789");

            var get_user8 = userManager.FindByEmail(user8.Email);

            userManager.AddToRole(get_user8.Id, "Student");

            var user9 = new ApplicationUser
            {
                UserName = "20134579",
                Email = "20134579@yopmail.com",
                Avatar = "20134579.png",
                FullName = "Nguyễn Thúc Huynh",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            userManager.Create(user9, "Ab=123456789");

            var get_user9 = userManager.FindByEmail(user9.Email);

            userManager.AddToRole(get_user9.Id, "Student");

            var user10 = new ApplicationUser
            {
                UserName = "TrungLD",
                Email = "TrungLD@yopmail.com",
                Avatar = "TrungLD.png",
                FullName = "Lê Đức Trung",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            userManager.Create(user10, "Ab=123456789");

            var get_user10 = userManager.FindByEmail(user10.Email);

            userManager.AddToRole(get_user10.Id, "Teacher");

            var user12 = new ApplicationUser
            {
                UserName = "Admin",
                Email = "Admin@yopmail.com",
                Avatar = "Admin.png",
                FullName = "Admin 1",
                Address = "MK",
                BirthDate = new DateTime(2002, 5, 14),
                Phone = "0123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            userManager.Create(user12, "Ab=123456789");

            var get_user12 = userManager.FindByEmail(user12.Email);

            userManager.AddToRole(get_user12.Id, "Admin");

            var semesters = new List<Semester>
            {
                new Semester
                {
                    Id = 20182,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (Semester item in semesters) context.Semesters.Add(item);

            var departments = new List<Department>
            {
                new Department
                {
                    DepartmentName = "CNTT",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (Department item in departments) context.Departments.Add(item);

            var studenClasses = new List<StudentClass>
            {
                new StudentClass
                {
                    ClassName = "CNTT 2.01",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new StudentClass
                {
                    ClassName = "CNTT 2.02",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new StudentClass
                {
                    ClassName = "CNTT 2.03",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new StudentClass
                {
                    ClassName = "CNTT 2.04",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (StudentClass item in studenClasses) context.StudentClasses.Add(item);

            var students = new List<Student>
            {
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user2.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user3.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user4.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user5.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[2],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user6.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[2],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user7.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user8.Id,
                },
                new Student
                {
                    Cpa = 3.0f,
                    Program = "Kỹ sư",
                    Class = studenClasses[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user9.Id,
                }
            };

            foreach (Student item in students) context.Students.Add(item);

            var admins = new List<Admin>
            {
                new Admin
                {
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user12.Id,
                }
            };

            foreach (Admin item in admins) context.Admins.Add(item);

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = get_user10.Id,
                }
            };

            foreach (Teacher item in teachers) context.Teachers.Add(item);

            var subjects = new List<Subject>
            {
                new Subject
                {
                    SubjectCode = "IT1110",
                    SubjectName = "Thực tập kỹ thuật",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new Subject
                {
                    SubjectCode = "IT3220",
                    SubjectName = "Thực tập tốt nghiệp",
                    Department = departments[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                }
            };

            foreach (Subject item in subjects) context.Subjects.Add(item);

            var learningClasses = new List<LearningClass>
            {
                new LearningClass
                {
                    ClassName = "IT1110 - 1",
                    Subject = subjects[0],
                    Semester = semesters[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new LearningClass
                {
                    ClassName = "IT3220 - 1",
                    Subject = subjects[1],
                    Semester = semesters[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                }
            };

            foreach (LearningClass item in learningClasses) context.LearningClasses.Add(item);

            var learningClassStudents = new List<LearningClassStudent>
            {
                new LearningClassStudent
                {
                    Class = learningClasses[0],
                    Student = students[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[1],
                    Student = students[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[1],
                    Student = students[2],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[0],
                    Student = students[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[0],
                    Student = students[4],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[0],
                    Student = students[5],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[1],
                    Student = students[6],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[1],
                    Student = students[7],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new LearningClassStudent
                {
                    Class = learningClasses[1],
                    Student = students[8],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (LearningClassStudent item in learningClassStudents) context.LearningClassStudents.Add(item);

            var majors = new List<TrainingMajor>
            {
                new TrainingMajor
                {
                    TrainingMajorName = "C",
                    Subject = subjects[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new TrainingMajor
                {
                    TrainingMajorName = "Java",
                    Subject = subjects[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new TrainingMajor
                {
                    TrainingMajorName = "Dev",
                    Subject = subjects[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new TrainingMajor
                {
                    TrainingMajorName = "Tester",
                    Subject = subjects[1],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (TrainingMajor item in majors) context.TrainingMajors.Add(item);

            var companies = new List<Company>
            {
                new Company
                {
                    CompanyName = "FPT",
                    CompanyDescription = "FPT",
                    Address = "Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức",
                    Email = "fpt@yopmail.com",
                    Phone = "0123456789",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Company
                {
                    CompanyName = "VNPT",
                    CompanyDescription = "VNPT",
                    Address = "142 Điện Biên Phủ, Phường Dakao, Quận 1, Tp. Hồ Chí Minh",
                    Email = "misa@yopmail.com",
                    Phone = "0123456789",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Company
                {
                    CompanyName = "VNG",
                    CompanyDescription = "VNG",
                    Address = "Z06 Số 13, Tân Thuận Đông, Quận 7, Thành phố Hồ Chí Minh",
                    Email = "bkav@yopmail.com",
                    Phone = "0123456789",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (Company item in companies) context.Companies.Add(item);

            var companyMajors = new List<CompanyTrainingMajor>
            {
                new CompanyTrainingMajor
                {
                    Company = companies[0],
                    TrainingMajor = majors[0],
                    AvailableTraineeCount = 10,
                    TotalTraineeCount = 10,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CompanyTrainingMajor
                {
                    Company = companies[0],
                    TrainingMajor = majors[1],
                    AvailableTraineeCount = 10,
                    TotalTraineeCount = 10,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CompanyTrainingMajor
                {
                    Company = companies[1],
                    TrainingMajor = majors[0],
                    AvailableTraineeCount = 5,
                    TotalTraineeCount = 5,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CompanyTrainingMajor
                {
                    Company = companies[2],
                    TrainingMajor = majors[2],
                    AvailableTraineeCount = 5,
                    TotalTraineeCount = 5,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CompanyTrainingMajor
                {
                    Company = companies[2],
                    TrainingMajor = majors[3],
                    AvailableTraineeCount = 5,
                    TotalTraineeCount = 5,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (CompanyTrainingMajor item in companyMajors) context.CompanyTrainingMajors.Add(item);

            var internships = new List<Internship>
            {
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[0],
                    Class = learningClasses[0],
                    Major = companyMajors[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[1],
                    Class = learningClasses[1],
                    Major = companyMajors[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[2],
                    Class = learningClasses[1],
                    Major = companyMajors[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[3],
                    Class = learningClasses[0],
                    Major = companyMajors[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[4],
                    Class = learningClasses[0],
                    Major = companyMajors[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[5],
                    Class = learningClasses[0],
                    Major = companyMajors[0],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[6],
                    Class = learningClasses[1],
                    Major = companyMajors[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[7],
                    Class = learningClasses[1],
                    Major = companyMajors[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Internship
                {
                    RegistrationDate = DateTime.Now,
                    Status = InternshipStatus.Registered,
                    Student = students[8],
                    Class = learningClasses[1],
                    Major = companyMajors[3],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            foreach (Internship item in internships) context.Internships.Add(item);


            var news = new List<News>
            {
                new News
                {
                    Title = "Thông báo về việc đăng ký đồ án tốt nghiệp kỳ hè 2023",
                    Content =
                        "Viện CNTT&TT thông báo về việc đăng ký đồ án tốt nghiệp (ĐATN) kỳ hè 2023 như sau: Sinh viên tất cả các hệ đào tạo (bao gồm cả CNCN) chỉ có thể đăng ký làm ĐATN trong kỳ hè với …",
                    Time = new DateTime(2023, 5, 18),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Chương trình thực tập tại Trung tâm SVMC – Samsung Electronic Việt Nam",
                    Content =
                        "I. SVMC Internship Program là gì? Trong khuôn khổ hợp tác giữa Samsung Electronics Việt Nam và các trường Đại học/ Học viện lớn tại miền Bắc, SVMC Internship Program là chương trình thiết thực và ý nghĩa nhằm chuẩn …",
                    Time = new DateTime(2023, 5, 9),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Thông báo hoàn thiện giấy tờ TTDN kỳ hè 2023",
                    Content =
                        "Để kết thúc thực tập 2023, các bạn sinh viên lưu ý liên lạc sớm với giao viên phụ trách để phối hợp hoàn thiện giấy tờ. 1 – Bảng đánh giá sinh viên thực tập do công ty thực …",
                    Time = new DateTime(2023, 5, 7),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Kế hoạch bảo vệ Đồ án tốt nghiệp học kỳ 2 2022-2023",
                    Content =
                        "Viện CNTT&TT xin thông báo về kế hoạch bảo vệ Đồ án tốt nghiệp (ĐATN) của học kỳ 2023 như sau: 1. Về cách thức tổ chức: – Bộ môn chuyên môn: Tổ chức thu quyển, phản biện, và chấm …",
                    Time = new DateTime(2023, 5, 2),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Thông báo số 1 – thực tập doanh nghiệp học kỳ hè 2023",
                    Content =
                        "Các bạn sinh viên tham khảo qui trình Thực tập doanh nghiệp để nắm rõ các thủ tục cần thực hiện. Sinh viên đăng ký nơi thực tập theo mẫu tại đây. Thời hạn hoàn thành đăng ký: 20/05/2023 Thông tin các công …",
                    Time = new DateTime(2023, 4, 27),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Kế hoạch bảo vệ luận văn cao học đợt 21/4/2023",
                    Content =
                        "B.1 Các công việc đăng ký bảo vệ: STT Công việc cần thực hiện Thời hạn hoàn thành 1.1 Học viên thống nhất với GVHD nội dung chi tiết của luận văn, tên đề tài và nộp đăng ký bảo …",
                    Time = new DateTime(2023, 4, 6),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                },
                new News
                {
                    Title = "Khóa học lập trình robot hợp tác với Viện Công nghệ Shibaura",
                    Content =
                        "Trong khuôn khổ các hoạt động hợp tác giữa ĐHBK Hà Nội và Viện Công nghệ Shibaura, Nhật Bản, Viện CNTT&TT sẽ tổ chức khóa học lập trình robot từ 1/3/2018-13/3/2018. Tham gia lớp học sẽ có 27 sinh viên …",
                    Time = new DateTime(2023, 4, 2),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = get_user10.Id,
                }
            };

            foreach (News item in news) context.Newses.Add(item);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}