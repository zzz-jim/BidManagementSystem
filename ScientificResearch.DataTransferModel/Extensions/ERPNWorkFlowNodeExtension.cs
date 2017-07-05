using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ERPNWorkFlowNodeExtension
    {
        public static ERPNWorkFlowNodeTransferObject ToDataTransferObjectModel(this ERPNWorkFlowNode domainModel)
        {
            return new ERPNWorkFlowNodeTransferObject
            {
                ID = domainModel.ID,
                WorkFlowID = domainModel.WorkFlowID,
                NodeSerils = domainModel.NodeSerils,
                NodeName = domainModel.NodeName,
                NodeAddr = domainModel.NodeAddr,
                NextNode = domainModel.NextNode,
                IFCanJump = domainModel.IFCanJump,
                IFCanView = domainModel.IFCanView,
                IFCanEdit = domainModel.IFCanEdit,
                IFCanDel = domainModel.IFCanDel,
                JieShuHours = domainModel.JieShuHours,
                PSType = domainModel.PSType,
                SPDefaultList = domainModel.SPDefaultList,
                CanWriteSet = domainModel.CanWriteSet,
                SecretSet = domainModel.SecretSet,
                ConditionSet = domainModel.ConditionSet,
                NodeLeft = domainModel.NodeLeft,
                NodeTop = domainModel.NodeTop
            };
        }
        public static ERPNWorkFlowNode ToDomainModel(this ERPNWorkFlowNodeTransferObject model)
        {
            return new ERPNWorkFlowNode
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
