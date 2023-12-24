using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using StudentInternshipManagement.Services.Implements;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class StudentViewModel : BaseViewModel
    {
        
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }

        //[Required]
        [MaxLength(50)]
        [ScaffoldColumn(false)]
        [DisplayName("Ảnh")]
        [DefaultValue("avatar.png")]
        public string Avatar { get; set; }

        [Required]
        [Range(0, 10)]
        [DisplayName("Điểm trung bình")]
        public float Cpa { get; set; }

        [DisplayName("Lớp học")]
        [UIHint("StudentClassTemplate")]
        public int ClassId { get; set; }

        [DisplayName("Mã sinh viên")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayName("Chương trình đào tạo")]
        public string Program { get; set; }

        [DisplayName("Lớp đào tạo")]
        public string ClassName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }


        public StudentViewModel() { }

        public StudentViewModel(Student entity)
        {
            UserName = entity.User.UserName; 
            FullName = entity.User.FullName;
            BirthDate = entity.User.BirthDate;
            Address = entity.User.Address;
            Phone = entity.User.Phone;
            Avatar = entity.User.Avatar;
            Email = entity.User.Email;

            Cpa = (entity.Cpa * 10) / 4;
            ClassId = entity.ClassId;
            UserId = entity.UserId;
            Program = entity.Program;
            ClassName = entity.Class.ClassName;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;
        }

        public static Student convertViewModelToEntity(StudentViewModel v, IStudentService service)
        {
            Student entity = new Student();
            entity = service.GetById(v.Id);

            entity.User.UserName = v.UserName;
            entity.User.FullName = v.FullName;
            entity.User.BirthDate = v.BirthDate;
            entity.User.Address = v.Address;
            entity.User.Phone = v.Phone;
            entity.Cpa = (v.Cpa*4)/10;
            entity.ClassId = v.ClassId;

            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = v.UpdatedBy;
            entity.IsDeleted = v.IsDeleted;

            entity.User.UpdatedAt = DateTime.Now;
            entity.User.UpdatedBy = v.UpdatedBy;
            entity.IsDeleted = v.IsDeleted;

            //entity.User.BirthDate = DateTime.ParseExact(v.UserBirthDate, "MM/dd/yyyy",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            return entity;
        }

        public static List<StudentViewModel> convertEntitiesToListViewModel(List<Student> entities)
        {
            List<StudentViewModel> listViewModel = new List<StudentViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new StudentViewModel(item));
            }

            return listViewModel;
        }

        public static List<Student> convertViewModelsToListEntities(List<StudentViewModel> viewModels, IStudentService service)
        {
            List<Student> listEntity = new List<Student>();

            foreach (var item in viewModels)
            {
                listEntity.Add(StudentViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }

        
    }
}
