using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class SubjectViewModel : Subject
    {
        public string SubjectId { get; set; }

        public SubjectViewModel() { }

        public SubjectViewModel(Subject entity)
        {
            SubjectCode = entity.SubjectCode;
            SubjectName = entity.SubjectName;
            DepartmentId = entity.DepartmentId;
            Department = entity.Department;
            TrainingMajors = entity.TrainingMajors;
            SubjectId = entity.Id.ToString();

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;

        }

        public static List<SubjectViewModel> convertEntitesToListViewModel(List<Subject> entities)
        {
            List<SubjectViewModel> listViewModel = new List<SubjectViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new SubjectViewModel(item));
            }

            return listViewModel;
        }

        public static List<Subject> convertViewModelsToListEntity(List<SubjectViewModel> viewModels)
        {
            List<Subject> listEntity = new List<Subject>();

            foreach (var item in viewModels)
            {
                listEntity.Add(SubjectViewModel.convertViewModelToEntity(item));
            }

            return listEntity;
        }

        public static Subject convertViewModelToEntity(SubjectViewModel v)
        {
            Subject entity = new Subject();

            entity.SubjectCode = v.SubjectCode;
            entity.SubjectName = v.SubjectName;
            entity.DepartmentId = v.DepartmentId;
            entity.Department = v.Department;
            entity.TrainingMajors = v.TrainingMajors;

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
