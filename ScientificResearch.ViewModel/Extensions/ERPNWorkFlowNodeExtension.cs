using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ERPNWorkFlowNodeExtension
    {
        public static ERPNWorkFlowNodeTransferObject ToDataTransferObjectModel(this ERPNWorkFlowNodeViewModel viewModel)
        {
            return new ERPNWorkFlowNodeTransferObject
            {
                ID = viewModel.ID,
                WorkFlowID = viewModel.WorkFlowID,
                NodeSerils = viewModel.NodeSerils,
                NodeName = viewModel.NodeName,
                NodeAddr = viewModel.NodeAddr,
                NextNode = viewModel.NextNode,
                IFCanJump = viewModel.IFCanJump,
                IFCanView = viewModel.IFCanView,
                IFCanEdit = viewModel.IFCanEdit,
                IFCanDel = viewModel.IFCanDel,
                JieShuHours = viewModel.JieShuHours,
                PSType = viewModel.PSType,
                SPDefaultList = viewModel.SPDefaultList,
                CanWriteSet = viewModel.CanWriteSet,
                SecretSet = viewModel.SecretSet,
                ConditionSet = viewModel.ConditionSet,
                NodeLeft = viewModel.NodeLeft,
                NodeTop = viewModel.NodeTop
            };
        }

        public static ERPNWorkFlowNodeViewModel ToViewModel(this ERPNWorkFlowNodeTransferObject model)
        {
            return new ERPNWorkFlowNodeViewModel
            {
                ID = model.ID,
                WorkFlowID = model.WorkFlowID,
                NodeSerils = model.NodeSerils,
                NodeName = model.NodeName,
                NodeAddr = model.NodeAddr,
                NextNode = model.NextNode,
                IFCanJump = model.IFCanJump,
                IFCanView = model.IFCanView,
                IFCanEdit = model.IFCanEdit,
                IFCanDel = model.IFCanDel,
                JieShuHours = model.JieShuHours,
                PSType = model.PSType,
                SPDefaultList = model.SPDefaultList,
                CanWriteSet = model.CanWriteSet,
                SecretSet = model.SecretSet,
                ConditionSet = model.ConditionSet,
                NodeLeft = model.NodeLeft,
                NodeTop = model.NodeTop

            };
        }
    }
}
