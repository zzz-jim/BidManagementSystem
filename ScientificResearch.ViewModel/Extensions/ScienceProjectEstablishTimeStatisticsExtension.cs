using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class ScienceProjectEstablishTimeStatisticsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ScienceProjectEstablishTimeStatisticsTransferObject ToDataTransferObjectModel(this ScienceProjectEstablishTimeStatisticsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new ScienceProjectEstablishTimeStatisticsTransferObject()
            {
                CityLevel = viewModel.CityLevel,
                CountryLevel = viewModel.CountryLevel,
                CountyLevel = viewModel.CountyLevel,
                HospitalLevel = viewModel.HospitalLevel,
                ProvinceLevel = viewModel.ProvinceLevel,
                Year = viewModel.Year,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ScienceProjectEstablishTimeStatisticsViewModel ToViewModel(this ScienceProjectEstablishTimeStatisticsTransferObject model)
        {
            return new ScienceProjectEstablishTimeStatisticsViewModel()
            {
                CityLevel = model.CityLevel,
                CountryLevel = model.CountryLevel,
                CountyLevel = model.CountyLevel,
                HospitalLevel = model.HospitalLevel,
                ProvinceLevel = model.ProvinceLevel,
                Year = model.Year,
                TotalCount = model.CityLevel + model.CountryLevel + model.CountyLevel + model.HospitalLevel + model.ProvinceLevel,
            };
        }
    }
}
