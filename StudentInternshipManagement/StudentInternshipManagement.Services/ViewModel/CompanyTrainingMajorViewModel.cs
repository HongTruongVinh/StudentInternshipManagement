using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentInternshipManagement.Services.Implements;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class CompanyTrainingMajorViewModel : BaseViewModel
    {
        /// <summary>
        /// thuộc tính này trùng với thuộc tính Id, thích dùng Id hoặc thuộc tính này đều được
        /// </summary>
        public int CompanyTrainingMajorId { get; set; }


        [UIHint("CompanyTemplate")]
        [DisplayName("Mã công ty")]
        public int CompanyId { get; set; }

        //[Key]
        //[Column(Order = 2)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [UIHint("TrainingMajorTemplate")]
        [DisplayName("Mã định hướng")]
        public int TrainingMajorId { get; set; }

        [DisplayName("Công ty")]
        public virtual Company Company { get; set; }

        [DisplayName("Định hướng")]
        public virtual TrainingMajor TrainingMajor { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Số lượng tối đa")]
        public int TotalTraineeCount { get; set; }

        [Range(0, 1000)]
        [DisplayName("Số lượng còn lại")]
        public int AvailableTraineeCount { get; set; }

        [DisplayName("Tên định hướng")]
        public string TrainingMajorName { get; set; }

        public static List<CompanyTrainingMajorViewModel> convertEntitesToListViewModel(List<CompanyTrainingMajor> entities)
        {
            List<CompanyTrainingMajorViewModel> listViewModel = new List<CompanyTrainingMajorViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new CompanyTrainingMajorViewModel(item));
            }

            return listViewModel;
        }

        public static List<CompanyTrainingMajor> convertViewModelsToListEntity(List<CompanyTrainingMajorViewModel> viewModels, ICompanyTrainingMajorService service)
        {
            List<CompanyTrainingMajor> listEntity = new List<CompanyTrainingMajor>();

            foreach (var item in viewModels)
            {
                listEntity.Add(CompanyTrainingMajorViewModel.convertViewModelToEntity(item, service));
            }

            return listEntity;
        }

        public CompanyTrainingMajorViewModel() { }

        public CompanyTrainingMajorViewModel(CompanyTrainingMajor entity)
        {
            CompanyId = entity.CompanyId;
            TrainingMajorId = entity.TrainingMajorId;
            Company = entity.Company;
            TrainingMajor = entity.TrainingMajor;
            TotalTraineeCount = entity.TotalTraineeCount;
            AvailableTraineeCount = entity.AvailableTraineeCount;

            CompanyTrainingMajorId = entity.Id;
            TrainingMajorName = TrainingMajor.TrainingMajorName;

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;

        }

        public static CompanyTrainingMajor convertViewModelToEntity(CompanyTrainingMajorViewModel v, ICompanyTrainingMajorService service)
        {
            CompanyTrainingMajor entity = new CompanyTrainingMajor();
            entity = service.GetById(v.CompanyId);

            entity.CompanyId = v.CompanyId;
            entity.TrainingMajorId = v.TrainingMajorId;
            entity.Company = v.Company;
            entity.TrainingMajor = v.TrainingMajor;
            entity.TotalTraineeCount = v.TotalTraineeCount;
            entity.AvailableTraineeCount = v.AvailableTraineeCount;

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
