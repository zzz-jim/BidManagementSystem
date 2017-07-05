using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ERPNFormExtension
    {
        public static ERPNFormTransferObject ToDataTransferObjectModel(this ERPNFormViewModel domainModel)
        {
            return new ERPNFormTransferObject()
            {
                ID = domainModel.ID,
                FormName = domainModel.FormName,
                TypeID = domainModel.TypeID,
                UserListOK = domainModel.UserListOK,
                DepListOK = domainModel.DepListOK,
                JiaoSeListOK = domainModel.JiaoSeListOK,
                PaiXuStr = domainModel.PaiXuStr,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                ContentStr = domainModel.ContentStr,
                ItemsList = domainModel.ItemsList,
                IFOK = domainModel.IFOK,
                LastModifiedTime = domainModel.LastModifiedTime,
                ModifiedPerson = domainModel.ModifiedPerson,
                ApprovalFlowDescription = domainModel.ApprovalFlowDescription,
                FormType = domainModel.FormType
            };
        }
        public static ERPNFormViewModel ToViewModel(this ERPNFormTransferObject model)
        {
            return new ERPNFormViewModel()
            {
                ID = model.ID,
                FormName = model.FormName,
                TypeID = model.TypeID,
                UserListOK = model.UserListOK,
                DepListOK = model.DepListOK,
                JiaoSeListOK = model.JiaoSeListOK,
                PaiXuStr = model.PaiXuStr,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                ContentStr = model.ContentStr,
                ItemsList = model.ItemsList,
                IFOK = model.IFOK,
                LastModifiedTime = model.LastModifiedTime,
                ModifiedPerson = model.ModifiedPerson,
                ApprovalFlowDescription = model.ApprovalFlowDescription,
                FormType = model.FormType
            };
        }
    }
}
