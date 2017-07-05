using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ERPBuMenExtension
    {
        public static ERPBuMenTransferObject ToDataTransferObjectModel(this ERPBuMenViewModel domainModel)
        {
            return new ERPBuMenTransferObject
            {
                ID = domainModel.ID,
                BuMenName = domainModel.BuMenName,
                ChargeMan = domainModel.ChargeMan,
                TelStr = domainModel.TelStr,
                ChuanZhen = domainModel.ChuanZhen,
                BackInfo = domainModel.BackInfo,
                DirID = domainModel.DirID,
            };
        }

        public static ERPBuMenViewModel ToViewModel(this ERPBuMenTransferObject model)
        {
            return new ERPBuMenViewModel
            {
                ID = model.ID,
                BuMenName = model.BuMenName,
                ChargeMan = model.ChargeMan,
                TelStr = model.TelStr,
                ChuanZhen = model.ChuanZhen,
                BackInfo = model.BackInfo,
                DirID = model.DirID,
            };
        }
    }
}