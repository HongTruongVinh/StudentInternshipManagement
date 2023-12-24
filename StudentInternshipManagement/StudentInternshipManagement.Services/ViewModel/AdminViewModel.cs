using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.BuilderProperties;
using StudentInternshipManagement.Services.Implements;
using System.Security.Policy;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class AdminViewModel : BaseViewModel
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
        [UIHint("DepartmentTemplate")]
        public int DepartmentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayName("Khoa")]
        public string DepartmentName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }



        public AdminViewModel() { }

        public AdminViewModel(Admin entity)
        {
            UserName = entity.User.UserName;
            FullName = entity.User.FullName;
            BirthDate = entity.User.BirthDate;
            Address = entity.User.Address;
            Phone = entity.User.Phone;
            Avatar = entity.User.Avatar;
            Email = entity.User.Email;

            DepartmentId = entity.DepartmentId;
            UserId = entity.UserId;
            DepartmentName = entity.Department.DepartmentName;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;
        }

        public static Admin convertViewModelToEntity(AdminViewModel v, IAdminService service)
        {
            Admin entity = new Admin();
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

            //entity.User.BirthDate = DateTime.ParseExact(v.UserBirthDate, "MM/dd/yyyy",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            return entity;
        }

        public static List<AdminViewModel> convertEntitiesToListViewModel(List<Admin> entities)
        {
            List<AdminViewModel> listViewModel = new List<AdminViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new AdminViewModel(item));
            }

            return listViewModel;
        }

        public static List<Admin> convertViewModelsToListEntities(List<AdminViewModel> viewModels, IAdminService service)
        {
            List<Admin> listEntity = new List<Admin>();

            foreach (var item in viewModels)
            {
                listEntity.Add(AdminViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }
    }
}
