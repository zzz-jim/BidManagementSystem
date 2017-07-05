using System;

namespace ScientificResearch.DataTransferModel
{
    public class TravelFundsDetailTransferObject
    {
        public int ID { get; set; }
        public int? FundsRecordId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Transportation { get; set; }
        public Nullable<double> TransportationFee { get; set; }
        public Nullable<double> HotelFee { get; set; }
        public Nullable<double> OtherFee { get; set; }
        public string OtherFeeComment { get; set; }
    }
}
