using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudentInternshipManagement.Services.Implements;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class TrainingMajorViewModel : BaseViewModel
    {
        [DisplayName("Mã định hướng")]
        public override int Id { get; set; }

        /// <summary>
        /// thuộc tính này trùng với thuộc tính Id, thích dùng Id hoặc thuộc tính này đều được
        /// </summary>
        [DisplayName("Mã định hướng")]
        public int TrainingMajorId { get; set; }


        [Required]
        [MaxLength(50)]
        [DisplayName("Tên định hướng")]
        public string TrainingMajorName { get; set; }

        [DisplayName("Môn học")]
        [UIHint("SubjectTemplate")]
        public int SubjectId { get; set; }
        

        public TrainingMajorViewModel() { }

        public TrainingMajorViewModel(TrainingMajor entity)
        {
            TrainingMajorName = entity.TrainingMajorName;
            SubjectId = entity.SubjectId;
            TrainingMajorId = entity.Id;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;
        }

        public static List<TrainingMajorViewModel> convertEntitesToListViewModel(List<TrainingMajor> entities)
        {
            List<TrainingMajorViewModel> listViewModel = new List<TrainingMajorViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new TrainingMajorViewModel(item));
            }

            return listViewModel;
        }

        public static List<TrainingMajor> convertViewModelsToListEntity(List<TrainingMajorViewModel> viewModels, ITrainingMajorService service)
        {
            List<TrainingMajor> listEntity = new List<TrainingMajor>();

            foreach (var item in viewModels)
            {
                listEntity.Add(TrainingMajorViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }

        public static TrainingMajor convertViewModelToEntity(TrainingMajorViewModel v, ITrainingMajorService service)
        {
            TrainingMajor entity = new TrainingMajor();
            entity = service.GetById(v.Id);

            entity.TrainingMajorName = v.TrainingMajorName;
            entity.SubjectId = v.SubjectId;

            entity.Id = v.Id;
            entity.UpdatedAt = v.UpdatedAt;
            entity.UpdatedBy = v.UpdatedBy;
            entity.CreatedAt = v.CreatedAt;
            entity.CreatedBy = v.CreatedBy;
            entity.IsDeleted = v.IsDeleted;

            return entity;
        }
    }
}
