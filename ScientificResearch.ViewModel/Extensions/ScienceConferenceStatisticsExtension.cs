using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class ScienceConferenceStatisticsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ScienceConferenceStatisticsTransferObject ToDataTransferObjectModel(this ScienceConferenceStatisticsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new ScienceConferenceStatisticsTransferObject()
            {
                Department = viewModel.Department,
                Students = viewModel.Students,
                Member = viewModel.Member,
                Others = viewModel.Others,
                No = viewModel.No,
                Count = viewModel.Count,
                Funds = viewModel.Funds,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ScienceConferenceStatisticsViewModel ToViewModel(this ScienceConferenceStatisticsTransferObject model)
        {
            return new ScienceConferenceStatisticsViewModel()
            {
              
                Department = model.Department,
                Students = model.Students,
                Member = model.Member,
                Others = model.Others,
                No = model.No,
                Count = model.Students+model.Member+model.Others+model.No,
                Funds = model.Funds,
            };
        }
    }
}
