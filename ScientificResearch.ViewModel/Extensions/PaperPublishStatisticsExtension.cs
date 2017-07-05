using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class PaperPublishStatisticsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static PaperPublishStatisticsTransferObject ToDataTransferObjectModel(this PaperPublishStatisticsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new PaperPublishStatisticsTransferObject()
            {
                 Superjournal =viewModel.Superjournal,
                 Onejournal=viewModel.Onejournal,
                 Twojournal=viewModel.Twojournal,
                 Threejournal=viewModel.Threejournal,
                 Year = viewModel.Year,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PaperPublishStatisticsViewModel ToViewModel(this PaperPublishStatisticsTransferObject model)
        {
            return new PaperPublishStatisticsViewModel()
            {

                Superjournal = model.Superjournal,
                Onejournal = model.Onejournal,
                Twojournal = model.Twojournal,
                Threejournal = model.Threejournal,
                Year = model.Year,
                TotalCount = model.Superjournal + model.Onejournal + model.Twojournal + model.Threejournal,
            };
        }
    }
}
