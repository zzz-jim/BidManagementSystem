using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ERPNWorkFlowExtension
    {
        public static ERPNWorkFlowTransferObject ToDataTransferObjectModel(this ERPNWorkFlow domainModel)
        {
            return new ERPNWorkFlowTransferObject
            {
                ID = domainModel.ID,
                WorkFlowName = domainModel.WorkFlowName,
                FormID = domainModel.FormID,
                UserListOK = domainModel.UserListOK,
                DepListOK = domainModel.DepListOK,
                JiaoSeListOK = domainModel.JiaoSeListOK,
                PaiXuStr = domainModel.PaiXuStr,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                BackInfo = domainModel.BackInfo,
                IFOK = domainModel.IFOK
            };
        }
        public static ERPNWorkFlow ToDomainModel(this ERPNWorkFlowTransferObject model)
        {
            return new ERPNWorkFlow
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
