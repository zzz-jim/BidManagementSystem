using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ERPNWorkFlowExtension
    {
        public static ERPNWorkFlowTransferObject ToDataTransferObjectModel(this ERPNWorkFlowViewModel viewModel)
        {
            return new ERPNWorkFlowTransferObject
            {
                ID = viewModel.ID,
                WorkFlowName = viewModel.WorkFlowName,
                FormID = viewModel.FormID,
                UserListOK = viewModel.UserListOK,
                DepListOK = viewModel.DepListOK,
                JiaoSeListOK = viewModel.JiaoSeListOK,
                PaiXuStr = viewModel.PaiXuStr,
                UserName = viewModel.UserName,
                TimeStr = viewModel.TimeStr,
                BackInfo = viewModel.BackInfo,
                IFOK = viewModel.IFOK
            };
        }

        public static ERPNWorkFlowViewModel ToViewModel(this ERPNWorkFlowTransferObject model)
        {
            return new ERPNWorkFlowViewModel
            {
                ID = model.ID,
                WorkFlowName = model.WorkFlowName,
                FormID = model.FormID,
                UserListOK = model.UserListOK,
                DepListOK = model.DepListOK,
                JiaoSeListOK = model.JiaoSeListOK,
                PaiXuStr = model.PaiXuStr,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                BackInfo = model.BackInfo,
                IFOK = model.IFOK
            };
        }
    }
}
