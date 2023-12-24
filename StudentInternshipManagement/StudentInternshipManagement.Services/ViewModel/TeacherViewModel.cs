using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class TeacherViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(10)]
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

        [DisplayName("Mã khoa")]
        public int DepartmentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayName("Khoa")]
        public string DepartmentName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public TeacherViewModel() { }

        public TeacherViewModel(Teacher entity) 
        { 
            UserName = entity.User.UserName;
            FullName = entity.User.FullName;
            BirthDate = entity.User.BirthDate;
            Address = entity.User.Address;
            Phone = entity.User.Phone;
            DepartmentId = entity.DepartmentId;
            Avatar = entity.User.Avatar;
            DepartmentName = entity.Department.DepartmentName;
            Email = entity.User.Email;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;
        }

        public static Teacher convertViewModelToEntity(TeacherViewModel v, ITeacherService service)
        {
            Teacher entity = new Teacher();
            entity = service.GetById(v.Id);

            
            entity.User.UserName = v.UserName;
            entity.User.FullName = v.FullName;
            entity.User.BirthDate = v.BirthDate;
            entity.User.Address = v.Address;
            entity.User.Phone = v.Phone;
            entity.DepartmentId = v.DepartmentId;

            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = v.UpdatedBy;
            entity.IsDeleted = v.IsDeleted;

            entity.User.UpdatedAt = DateTime.Now;
            entity.User.UpdatedBy = v.UpdatedBy;
            entity.IsDeleted = v.IsDeleted;

            return entity;
        }

        public static List<TeacherViewModel> convertEntitesToListViewModel(List<Teacher> entities)
        {
            List<TeacherViewModel> listViewModel = new List<TeacherViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new TeacherViewModel(item));
            }

            return listViewModel;
        }

        public static List<Teacher> convertViewModelsToListEntity(List<TeacherViewModel> viewModels, ITeacherService service)
        {
            List<Teacher> listEntity = new List<Teacher>();

            foreach (var item in viewModels)
            {
                listEntity.Add(TeacherViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }

    }
}
