using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class CompanyViewModel: Company
    {
        public string CompanyId { get; set; }

        public CompanyViewModel() { }

        public CompanyViewModel(Company entity)
        {
            CompanyName = entity.CompanyName;
            CompanyDescription = entity.CompanyDescription;
            Address = entity.Address;
            Email = entity.Email;
            Phone = entity.Phone;
            CompanyTrainingMajors = entity.CompanyTrainingMajors;

            CompanyId = entity.Id.ToString();

            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            UpdatedBy = entity.UpdatedBy;
            CreatedAt = entity.CreatedAt;
            CreatedBy = entity.CreatedBy;
            IsDeleted = entity.IsDeleted;

        }

        public static List<CompanyViewModel> convertEntitesToListViewModel(List<Company> entities)
        {
            List<CompanyViewModel> listViewModel = new List<CompanyViewModel>();

            foreach (var item in entities)
            {
                listViewModel.Add(new CompanyViewModel(item));
            }

            return listViewModel;
        }

        public static List<Company> convertViewModelsToListEntity(List<CompanyViewModel> viewModels)
        {
            List<Company> listEntity = new List<Company>();

            foreach (var item in viewModels)
            {
                listEntity.Add(CompanyViewModel.convertViewModelToEntity(item));
            }

            return listEntity;
        }

        public static Company convertViewModelToEntity(CompanyViewModel v)
        {
            Company entity = new Company();

            entity.CompanyName = v.CompanyName;
            entity.CompanyDescription = v.CompanyDescription;
            entity.Address = v.Address;
            entity.Email = v.Email;
            entity.Phone = v.Phone;
            entity.CompanyTrainingMajors = v.CompanyTrainingMajors;

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
