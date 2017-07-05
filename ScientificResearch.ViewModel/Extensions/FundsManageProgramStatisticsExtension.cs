using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel.Extensions
{
    public static class FundsManageProgramStatisticsExtension
    {
        public static FundsManageProgramStatisticsTransferObject ToDataTranferObjectModel(this FundsManageProgramStatisticsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }
            return new FundsManageProgramStatisticsTransferObject()
            {
                Number = viewModel.Number,
                ProjectName = viewModel.ProjectName,
                FundsType = viewModel.FundsType,
                Programtype = viewModel.Programtype,
                ApplyMan = viewModel.ApplyMan,
                LocalDepartment = viewModel.LocalDepartment,
                SuperiorFunds = viewModel.SuperiorFunds,
                HospitalFunds = viewModel.HospitalFunds,
                ProjectTotalFunds = viewModel.ProjectTotalFunds,
                Income = viewModel.Income,
                Expend = viewModel.Expend,
                Balance = viewModel.Balance,
                EstablishTiem = viewModel.EstablishTiem,
                IsIncome = viewModel.IsIncome,
                ProjecProcessTime = viewModel.ProjecProcessTime
            };
        }
        public static FundsManageProgramStatisticsViewModel TovViewModel(this FundsManageProgramStatisticsViewModel model)
        {
            return new FundsManageProgramStatisticsViewModel()
            {
                Number = model.Number,
                ProjectName = model.ProjectName,
                FundsType = model.FundsType,
                Programtype = model.Programtype,
                ApplyMan = model.ApplyMan,
                LocalDepartment = model.LocalDepartment,
                SuperiorFunds = model.SuperiorFunds,
                HospitalFunds = model.HospitalFunds,
                ProjectTotalFunds = model.ProjectTotalFunds,
                Income = model.Income,
                Expend = model.Expend,
                Balance = model.Balance,
                EstablishTiem = model.EstablishTiem,
                IsIncome = model.IsIncome,
                ProjecProcessTime = model.ProjecProcessTime
            };
        }
    }
}
