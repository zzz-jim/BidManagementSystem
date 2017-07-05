using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ERPBuMenExtension
    {
        public static ERPBuMenTransferObject ToDataTransferObjectModel(this ERPBuMen domainModel)
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

        public static ERPBuMen ToDomainModel(this ERPBuMenTransferObject model)
        {
            return new ERPBuMen
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
