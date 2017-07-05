using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class TravelFundsDetailExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static TravelFundsDetailTransferObject ToDataTransferObjectModel(this TravelFundsDetailViewModel viewModel)
        {
            return new TravelFundsDetailTransferObject()
            {
                ID = viewModel.ID,
                FundsRecordId = viewModel.FundsRecordId,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                FromAddress = viewModel.FromAddress,
                ToAddress = viewModel.ToAddress,
                Transportation = viewModel.Transportation,
                TransportationFee = viewModel.TransportationFee,
                HotelFee = viewModel.HotelFee,
                OtherFee = viewModel.OtherFee,
                OtherFeeComment = viewModel.OtherFeeComment,
            };
        }

        public static TravelFundsDetailViewModel ToViewModel(this TravelFundsDetailTransferObject model)
        {
            return new TravelFundsDetailViewModel()
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