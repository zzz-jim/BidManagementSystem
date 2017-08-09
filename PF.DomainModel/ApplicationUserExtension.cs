using PF.DomainModel.Identity;
using ScientificResearch.Utility.Constants;

namespace PF.DomainModel
{
    public static class ApplicationUserExtension
    {
        public static ApplicationUserTransferObject ToDataTransferObjectModel(this ApplicationUser domainModel)
        {
            return new ApplicationUserTransferObject
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                WorkId = domainModel.WorkId,
                PinYin = domainModel.PinYin,
                Qualification = domainModel.Qualification,
                Degree = domainModel.Degree,
                Special = domainModel.Special,
                TechnicalTitle = domainModel.TechnicalTitle,
                Duty = domainModel.Duty,
                LastUpdateDate = domainModel.LastUpdateDate,
            };
        }

        public static ApplicationUser ToDomainModel(this ApplicationUserTransferObject model)
        {
            return new ApplicationUser
            {
                Id = model.Id,
                Name = model.Name,
                WorkId = model.WorkId,
                PinYin = model.PinYin,
                Qualification = model.Qualification,
                Degree = model.Degree,
                Special = model.Special,
                TechnicalTitle = model.TechnicalTitle,
                Duty = model.Duty,
                LastUpdateDate = model.LastUpdateDate,
            };
        }

        public static SectionTransferObject ToDataTransferObjectModel(this Section domainModel)
        {
            return new SectionTransferObject
            {
                Name = domainModel.Name,
                Id = "Section" + Constant.Comma.ToString() + domainModel.Id,
                //DepartmentId = domainModel.DepartmentId,
                HasChildren = true,
            };
        }

        public static Section ToDomainModel(this SectionTransferObject model)
        {
            return new Section
            {
                Name = model.Name,
                Id = model.Id,
                //DepartmentId = model.DepartmentId,
            };
        }

        public static DepartmentTransferObject ToDataTransferObjectModel(this Department domainModel)
        {
            return new DepartmentTransferObject
            {
                Name = domainModel.Name,
                Id = "Department" + Constant.Comma.ToString() + domainModel.Id,
                HospitalId = domainModel.HospitalId,
                HasChildren = true,
            };
        }

        public static Department ToDomainModel(this DepartmentTransferObject model)
        {
            return new Department
            {
                Name = model.Name,
                Id = model.Id,
                HospitalId = model.HospitalId,
            };
        }

        public static HospitalTransferObject ToDataTransferObjectModel(this Hospital domainModel)
        {
            return new HospitalTransferObject
            {
                Name = domainModel.Name,
                Id = "Hospital" + Constant.Comma.ToString() + domainModel.Id,
                HasChildren = true,
            };
        }

        public static Hospital ToDomainModel(this HospitalTransferObject model)
        {
            return new Hospital
            {
                Name = model.Name,
                Id = model.Id,
            };
        }
    }
}

