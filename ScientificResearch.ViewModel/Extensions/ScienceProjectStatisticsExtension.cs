using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class ScienceProjectStatisticsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ScienceProjectStatisticsTransferObject ToDataTransferObjectModel(this ScienceProjectStatisticsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new ScienceProjectStatisticsTransferObject()
            {
                Balance = viewModel.Balance,
                CounterpartFunds = viewModel.CounterpartFunds,
                Department = viewModel.Department,
                Name = viewModel.Name,
                Number = viewModel.Number,
                Payment = viewModel.Payment,
                ProjectManager = viewModel.ProjectManager,
                ReleaseFunds = viewModel.ReleaseFunds,
                TeamMemberList = viewModel.TeamMemberList,
                Time = viewModel.Time,
                TotalFunds = viewModel.TotalFunds,
                Type = viewModel.Type,
                EstablishType = viewModel.EstablishType,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ScienceProjectStatisticsViewModel ToViewModel(this ScienceProjectStatisticsTransferObject model)
        {
            return new ScienceProjectStatisticsViewModel()
            {
                // ReleaseFunds + CounterpartFunds + Payment = Balance
                Balance = model.ReleaseFunds + model.CounterpartFunds + model.Payment,
                CounterpartFunds = model.CounterpartFunds,
                Department = model.Department,
                Name = model.Name,
                Number = model.Number,
                Payment = model.Payment,
                ProjectManager = model.ProjectManager,
                ReleaseFunds = model.ReleaseFunds,
                TeamMemberList = model.TeamMemberList,
                Time = model.Time,
                TotalFunds = model.ReleaseFunds + model.CounterpartFunds,
                Type = model.Type,
                EstablishType = model.EstablishType,
            };
        }
    }
}
