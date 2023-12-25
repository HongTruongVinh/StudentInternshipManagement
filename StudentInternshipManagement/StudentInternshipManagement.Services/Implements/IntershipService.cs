using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using StudentInternshipManagement.Models.Constants;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Repositories.Implements;

namespace StudentInternshipManagement.Services.Implements
{
    #region Interface

    public interface IInternshipService : IGenericService<Internship>
    {
        void AssignInternship();
        void CreateGroup();
        IQueryable<Internship> GetByLatestSemester();
        IQueryable<Internship> GetBySemester(int semesterId);
        void ProcessRegistration();
        IQueryable<Internship> GetByStudent(int studentId);
    }

    #endregion

    #region Class

    public class InternshipService : GenericService<Internship>, IInternshipService
    {
        private readonly IEmailService _emailService;
        private readonly ISemesterService _semesterService;

        public InternshipService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public InternshipService(IUnitOfWork unitOfWork, ISemesterService semesterService, IEmailService emailService) :
            base(unitOfWork)
        {
            _semesterService = semesterService;
            _emailService = emailService;
        }

        public IQueryable<Internship> GetByStudent(int studentId)
        {
            return UnitOfWork.Repository<Internship>().TableNoTracking.Where(i => i.StudentId == studentId);
        }

        public IQueryable<Internship> GetBySemester(int semesterId)
        {
            return UnitOfWork.Repository<Internship>().TableNoTracking.Where(i => i.Class.SemesterId == semesterId);
        }

        public IQueryable<Internship> GetByLatestSemester()
        {
            int semesterId = _semesterService.GetLatest().Id;
            return GetBySemester(semesterId);
        }

        public void ProcessRegistration()
        {
            AssignInternship();
            //CreateGroup(); // Vinh bỏ tạm dòng code này vì hàm CreateGroup sẽ được gọi luôn trong trong hàm AssignInternship trên 
            //_emailService.SendProcessEmail(); // Vinh bỏ tạm dòng code này vì tạm thời chưa xử lý gửi thông báo đến mail 

            //vinh thêm dòng code sau để db lưu dữ liệu
            UnitOfWork.Commit(); 
        }

        public void AssignInternship()
        {
            //Vinh tạo ra list này để lưu lại những nv đã đc xử lý thành công và truyền list này vào hàm CreateGroup mới 
            var listSuccessInterships = new List<Internship>();

            //var lateRegisteredInternships = new List<Internship>();
            //List<CompanyTrainingMajor> leftMajors =
            //    UnitOfWork.Repository<CompanyTrainingMajor>().TableNoTracking.ToList();

            var lateRegisteredInternships = new List<Internship>();
            List<CompanyTrainingMajor> leftMajors =
                UnitOfWork.Repository<CompanyTrainingMajor>().TableNoTracking.ToList();

            // code gốc lấy ra danh sách tất cả các nguyện vọng trong database, nó bao gồm cả các nv có trạng thái Success, Failed, Done, Registered 
            //var listInterships = GetByLatestSemester().OrderByDescending(i => i.RegistrationDate).ToList();

            // Vinh sửa: chỉ lấy ra những nv có tạng thái là Registered (tức là những nv đang chờ xử lý),  còn các nv khác như Success, Failed, Done sẽ không còn lấy lên xử lý
            var listInterships = GetByLatestSemester().Where(i => i.Status == InternshipStatus.Registered).OrderByDescending(i => i.RegistrationDate).ToList();

            foreach (Internship item in listInterships)
                if (item.Major.AvailableTraineeCount > 0) // Nếu nguyện vọng này vẫn còn slot thực tập thì xử lý 
                {
                    item.Status = InternshipStatus.Success; // chuyển trạng thái nv thành success
                    item.Major.AvailableTraineeCount--;     // giảm slot thực tập đi 1 

                    //vinh bổ sung 2 dòng code sau
                    item.TrainingMajorId = item.Major.Id;
                    item.CompanyId = item.Major.CompanyId;

                    UnitOfWork.Repository<Internship>().Update(item);

                    // Vinh them cac dong code sau
                    listSuccessInterships.Add(UnitOfWork.Repository<Internship>().GetById(item.Id)); // thêm vào ds cách sv đăng ký nv thành công 

                    var learningClassStudent = new LearningClassStudent() // thêm sinh viên vào lớp đã đăng ký trong nv 
                    {
                        ClassId = item.ClassId,
                        StudentId = item.StudentId,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,

                        Student = UnitOfWork.Repository<Student>().Table.Where(s => s.Id == item.StudentId).FirstOrDefault(),
                        Class = UnitOfWork.Repository<LearningClass>().Table.Where(s => s.Id == item.ClassId).FirstOrDefault(),
                    };

                    UnitOfWork.Repository<LearningClassStudent>().Add(learningClassStudent);
                }
                else // Nếu nguyện vọng này hết slot thực tập thì xử lý
                {
                    lateRegisteredInternships.Add(item); // thêm vào list những nv sẽ được xử lý sau  
                    leftMajors.Remove(item.Major);       // loại những định hướng mà công ty đang tuyển đã hết slot 
                }

            foreach (Internship item in lateRegisteredInternships)
            {
                CompanyTrainingMajor major = leftMajors.FirstOrDefault(m => m.TrainingMajorId == item.TrainingMajorId); // lấy danh sách
                // các sự tuyển dụng mà công ty khác đang tuyển (định hướng tuyện dụng của công ty trùng với định hướng mà  
                // cv muốn đăng ký 


                if (major != null) // nếu tồn tại 1 công ty như điều kiện trên thì sẽ chuyển sv này sang công ty khác (công ty khác với nv của sv)
                {
                    item.TrainingMajorId = major.TrainingMajorId;
                    item.CompanyId = major.CompanyId;
                    item.Status = InternshipStatus.Success;
                    major.AvailableTraineeCount--;
                    if (major.AvailableTraineeCount == 0) leftMajors.Remove(major);

                    //vinh bổ sung 2 dòng code sau
                    item.TrainingMajorId = item.Major.Id;
                    item.CompanyId = item.Major.CompanyId;

                    UnitOfWork.Repository<Internship>().Update(item);
                    lateRegisteredInternships.Remove(item);

                    // Vinh them cac dong code sau
                    listSuccessInterships.Add(UnitOfWork.Repository<Internship>().GetById(item.Id));
                    // Lý do thêm dòng code trên: thêm vào ds cách nv được xử lý
                    // thành công để để lúc nữa CreateGroup dựa trên những nv vọng này.
                    // Tại sao lại là: listSuccessInterships.Add(UnitOfWork.Repository<Internship>().GetById(item.Id))
                    // chứ không phải listSuccessInterships.Add(item) vì những intership này là thực thể riêng biệt ánh xạ
                    // với các intership dưới database chứ không phải là các thực thể dưới db. Cho nên việc thay đổi trên item
                    // thì các intership dưới db vẫn không có thay đổi vì vậy ta phải lấy nguyên thực thể intership dưới db lên
                    // bằng câu lệnh listSuccessInterships.Add(UnitOfWork.Repository<Internship>().GetById(item.Id))
                    // để lúc nữa CreateGroup dựa trên những thực thể intership dưới db chứ không phải dựa trên những 
                    // intership (là item) bản sao này




                    var learningClassStudent = new LearningClassStudent() // thêm sinh viên vào lớp đã đăng ký trong nv 
                    {
                        ClassId = item.ClassId,
                        StudentId = item.StudentId,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,

                        Student = UnitOfWork.Repository<Student>().Table.Where(s => s.Id == item.StudentId).FirstOrDefault(),
                        Class = UnitOfWork.Repository<LearningClass>().Table.Where(s => s.Id == item.ClassId).FirstOrDefault(),
                    };

                    UnitOfWork.Repository<LearningClassStudent>().Add(learningClassStudent);
                }
                else // ngược lại, nếu ko còn công ty nào tuyển định hướng đúng với nv của sv thì sẽ xếp sv vào 1 công ty bất kỳ đang tuyển môn học của sv 
                {
                    CompanyTrainingMajor randomMajor =
                        leftMajors.FirstOrDefault(m => m.TrainingMajor.SubjectId == item.Class.SubjectId);
                    if (randomMajor != null)
                    {
                        item.TrainingMajorId = randomMajor.TrainingMajorId;
                        item.CompanyId = randomMajor.CompanyId;
                        item.Status = InternshipStatus.Success;
                        randomMajor.AvailableTraineeCount--;
                        if (randomMajor.AvailableTraineeCount == 0) leftMajors.Remove(randomMajor);

                        //vinh bổ sung 2 dòng code sau
                        item.TrainingMajorId = item.Major.Id;
                        item.CompanyId = item.Major.CompanyId;

                        UnitOfWork.Repository<Internship>().Update(item);
                        lateRegisteredInternships.Remove(item);

                        // Vinh them cac dong code sau
                        listSuccessInterships.Add(UnitOfWork.Repository<Internship>().GetById(item.Id));  // thêm vào ds cách sv đăng ký nv thành công 

                        var learningClassStudent = new LearningClassStudent() // thêm sinh viên vào lớp đã đăng ký trong nv 
                        {
                            ClassId = item.ClassId,
                            StudentId = item.StudentId,
                            IsDeleted = false,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,

                            Student = UnitOfWork.Repository<Student>().Table.Where(s => s.Id == item.StudentId).FirstOrDefault(),
                            Class = UnitOfWork.Repository<LearningClass>().Table.Where(s => s.Id == item.ClassId).FirstOrDefault(),
                        };

                        UnitOfWork.Repository<LearningClassStudent>().Add(learningClassStudent);
                    }
                }
            }

            //cho tất cả các sv chưa được xếp vào công ty nào có 1 trong các điều kiện như trên thì sẽ cập nhật đăng ký thất bại 
            foreach (Internship item in lateRegisteredInternships)
            {
                item.Status = InternshipStatus.Failed;
                UnitOfWork.Repository<Internship>().Update(item);
            }

            //UnitOfWork.Commit(); // Thêm dòng này để database được lưu. Nếu không, khi mà
            // thực hiện CreateGroup, ta lấy groupByMajors như function bên dưới
            // thì sẽ không thể lấy được các Internship ở trên 
            //  vừa cập nhật  status = Success
            // Tuy nhiên nếu thêm dòng code này thì sẽ bị duplicate student 
            // Với code mới như hiện tại thì không cần

            CreateGroup(listSuccessInterships);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void CreateGroup()
        {
            // lấy list các nguyện vọng có trạng thái success (những nv đã đăng ký thành công)
            // và nhóm chúng lại thành các nhóm có cùng sự tuyển dụng
            List<IGrouping<CompanyTrainingMajor, Internship>> groupByMajors = GetByLatestSemester()
                .Where(i => i.Status == InternshipStatus.Success)
                .GroupBy(i => i.Major).ToList();

            IQueryable<Teacher> teachers = UnitOfWork.Repository<Teacher>().TableNoTracking;

            // các giáo viên gán với số lượng nhóm còn có thể hưỡng dẫn
            Dictionary<Teacher, int> teacherAssign = teachers.ToDictionary(t => t, t => InternshipConstants.GroupsPerTeacher);

            foreach (IGrouping<CompanyTrainingMajor, Internship> item in groupByMajors) // duyệt qua list nguyện vọng đăng ký cùng sự tuyển dụng
            {
                List<Internship> members = item.Select(i => i).ToList();
                var groups = new List<List<Internship>>();
                for (var i = 0; i < members.Count; i += InternshipConstants.StudentsPerGroups)
                    groups.Add(members.GetRange(i, Math.Min(InternshipConstants.StudentsPerGroups, members.Count - i)));
                //while (members.Any())
                //{
                //    groups.Add(members.Take(5).ToList());
                //    members = members.Skip(5);
                //}

                var groupId = 1;
                foreach (List<Internship> groupItem in groups) // duyệt qua list các nhóm đã được tạo ở trên 
                {
                    //đặt các giá trị cho các nhóm vừa tạo ở trên 
                    var group = new Group
                    {
                        GroupName =
                            $"{groupItem.FirstOrDefault().Major.Company.CompanyName}-{groupItem.FirstOrDefault().Major.TrainingMajor.TrainingMajorName}-{groupId}",
                        ClassId = groupItem.FirstOrDefault().Class.Id,
                        CompanyId = groupItem.FirstOrDefault().Major.CompanyId,
                        TrainingMajorId = groupItem.FirstOrDefault().Major.TrainingMajorId,
                        Members = groupItem.Select(g => g.Student).ToList(),
                        LeaderId = groupItem.OrderByDescending(g => g.Student.Cpa).FirstOrDefault().Student.Id // người có điểm cao nhất sẽ làm trưởng nhóm 
                    };

                    Teacher teacher = teacherAssign.FirstOrDefault(t =>
                        t.Key.Department.Id == groupItem.FirstOrDefault().Class.Subject.Department.Id 
                        && t.Value > 0 ).Key; // lấy ra người thầy mà thuộc khoa X == lớp của môn học cũng phải thuộc khoa X 

                    group.TeacherId = teacher.Id;

                    teacherAssign[teacher]--;

                    group.CreatedAt = DateTime.Now;
                    group.UpdatedAt = DateTime.Now;

                    UnitOfWork.Repository<Group>().Add(group);
                    groupId++;
                }
            }
        }

        /// <summary>
        /// Vinh tạo ra hàm này thay thế hàm cũ vì hàm cũ không lấy được những Internship có status success vừa được cập nhật
        /// </summary>
        /// <param name="internships"></param>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void CreateGroup(List<Internship> internships)
        {
            // lấy list các nguyện vọng có trạng thái success (những nv đã đăng ký thành công)
            // và nhóm chúng lại thành các nhóm có cùng sự tuyển dụng
            List<IGrouping<CompanyTrainingMajor, Internship>> groupByMajors = internships.GroupBy(i => i.Major).ToList();

            IQueryable<Teacher> teachers = UnitOfWork.Repository<Teacher>().TableNoTracking;

            // các giáo viên gán với số lượng nhóm còn có thể hưỡng dẫn
            Dictionary<Teacher, int> teacherAssign = teachers.ToDictionary(t => t, t => InternshipConstants.GroupsPerTeacher);

            foreach (IGrouping<CompanyTrainingMajor, Internship> item in groupByMajors) // duyệt qua list nguyện vọng đăng ký cùng sự tuyển dụng
            {
                List<Internship> members = item.Select(i => i).ToList();
                var groups = new List<List<Internship>>();
                for (var i = 0; i < members.Count; i += InternshipConstants.StudentsPerGroups)
                    groups.Add(members.GetRange(i, Math.Min(InternshipConstants.StudentsPerGroups, members.Count - i)));
                //while (members.Any())
                //{
                //    groups.Add(members.Take(5).ToList());
                //    members = members.Skip(5);
                //}

                var groupId = 1;
                foreach (List<Internship> groupItem in groups) // duyệt qua list các nhóm đã được tạo ở trên 
                {
                    //đặt các giá trị cho các nhóm vừa tạo ở trên 
                    var group = new Group
                    {
                        GroupName =
                            $"{groupItem.FirstOrDefault().Major.Company.CompanyName}-{groupItem.FirstOrDefault().Major.TrainingMajor.TrainingMajorName}-{groupId}",
                        ClassId = groupItem.FirstOrDefault().Class.Id,
                        CompanyId = groupItem.FirstOrDefault().Major.CompanyId,
                        TrainingMajorId = groupItem.FirstOrDefault().Major.TrainingMajorId,
                        Members = groupItem.Select(g => g.Student).ToList(),
                        LeaderId = groupItem.OrderByDescending(g => g.Student.Cpa).FirstOrDefault().Student.Id, // người có điểm cao nhất sẽ làm trưởng nhóm 
                        Major = groupItem.FirstOrDefault().Major,
                    };

                    Teacher teacher = teacherAssign.FirstOrDefault(t =>
                        t.Key.Department.Id == groupItem.FirstOrDefault().Class.Subject.Department.Id
                        && t.Value > 0).Key; // lấy ra người thầy mà thuộc khoa X == lớp của môn học cũng phải thuộc khoa X 

                    group.TeacherId = teacher.Id;

                    group.Teacher = UnitOfWork.Repository<Teacher>().Table.Where(t => t.Id == teacher.Id).FirstOrDefault();

                    teacherAssign[teacher]--;

                    group.CreatedAt = DateTime.Now;
                    group.UpdatedAt = DateTime.Now;

                    UnitOfWork.Repository<Group>().Add(group);
                    groupId++;
                }
            }
        }
    }

    #endregion

}