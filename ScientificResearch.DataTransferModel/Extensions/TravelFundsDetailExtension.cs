using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    /// <summary>
    /// Supply the conversition between domain model and business logic model.
    /// </summary>
    public static class TravelFundsDetailExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainModel"></param>
        /// <returns></returns>
        public static TravelFundsDetailTransferObject ToDataTransferObjectModel(this TravelFundsDetail domainModel)
        {
            return new TravelFundsDetailTransferObject()
            {
                ID = domainModel.ID,
                FundsRecordId = domainModel.FundsRecordId,
                StartDate = domainModel.StartDate,
                EndDate = domainModel.EndDate,
                FromAddress = domainModel.FromAddress,
                ToAddress = domainModel.ToAddress,
                Transportation = domainModel.Transportation,
                TransportationFee = domainModel.TransportationFee,
                HotelFee = domainModel.HotelFee,
                OtherFee = domainModel.OtherFee,
                OtherFeeComment = domainModel.OtherFeeComment,
            };
        }

        public static TravelFundsDetail ToDomainModel(this TravelFundsDetailTransferObject model)
        {
            return new TravelFundsDetail()
            {
                ID = model.ID,
                FundsRecordId = model.FundsRecordId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                FromAddress = model.FromAddress,
                ToAddress = model.ToAddress,
                Transportation = model.Transportation,
                TransportationFee = model.TransportationFee,
                HotelFee = model.HotelFee,
                OtherFee = model.OtherFee,
                OtherFeeComment = model.OtherFeeComment,
            };
        }
    }
}
