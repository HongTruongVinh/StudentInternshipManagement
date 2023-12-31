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
    public class LearningClassStudentViewModel: BaseViewModel
    {
        //[Key]
        //[Column(Order = 1)]
        [DisplayName("Lớp")]
        public int ClassId { get; set; }

        //[Key]
        //[Column(Order = 2)]
        [DisplayName("Sinh viên")]
        public int StudentId { get; set; }

        [Range(0, 10)]
        [DisplayName("Điểm giữa kỳ")]
        public float? MidTermPoint { get; set; }

        [Range(0, 10)]
        [DisplayName("Điểm cuối kỳ")]
        public float? EndTermPoint { get; set; }

        [Range(0, 10)]
        [DisplayName("Điểm tổng kết")]
        public float? TotalPoint { get; set; }

        [DisplayName("Mã sinh viên")]
        public string StudentCode { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }

        [DisplayName("Lớp")]
        public string ClassName { get; set; }

        public LearningClassStudentViewModel() { }

        public LearningClassStudentViewModel(LearningClassStudent entity)
        {
            StudentCode = entity.Student.User.UserName;
            FullName = entity.Student.User.FullName;
            ClassId = entity.ClassId;
            StudentId = entity.StudentId;
            MidTermPoint = entity.MidTermPoint;
            EndTermPoint = entity.EndTermPoint;
            TotalPoint = entity.TotalPoint;
            ClassName = entity.Class.ClassName;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;
        }

        public static LearningClassStudent convertViewModelToEntity(LearningClassStudentViewModel v, ILearningClassStudentService service)
        {
            LearningClassStudent entity = new LearningClassStudent();
            entity = service.GetById(v.Id);

            entity.MidTermPoint = v.MidTermPoint;
            entity.EndTermPoint = v.EndTermPoint;
            entity.TotalPoint = v.TotalPoint;

            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = v.UpdatedBy;
            entity.IsDeleted = v.IsDeleted;

            //entity.User.BirthDate = DateTime.ParseExact(v.UserBirthDate, "MM/dd/yyyy",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            return entity;
        }

        public static List<LearningClassStudentViewModel> convertEntitiesToListViewModel(List<LearningClassStudent> entities)
        {
            List<LearningClassStudentViewModel> listViewModel = new List<LearningClassStudentViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new LearningClassStudentViewModel(item));
            }

            return listViewModel;
        }

        public static List<LearningClassStudent> convertViewModelsToListEntities(List<LearningClassStudentViewModel> viewModels, ILearningClassStudentService service)
        {
            List<LearningClassStudent> listEntity = new List<LearningClassStudent>();

            foreach (var item in viewModels)
            {
                listEntity.Add(LearningClassStudentViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }

    }
}
