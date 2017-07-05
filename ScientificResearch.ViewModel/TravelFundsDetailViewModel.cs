using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class TravelFundsDetailViewModel
    {
        public int ID { get; set; }
        public int? FundsRecordId { get; set; }

        [Display(Name = "起日期")]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "止日期")]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "出发地点")]
        public string FromAddress { get; set; }

        [Display(Name = "到达地点")]
        public string ToAddress { get; set; }

        [Display(Name = "交通工具")]
        public string Transportation { get; set; }

        [Display(Name = "交通费用")]
        public Nullable<double> TransportationFee { get; set; }

        [Display(Name = "住宿费用")]
        public Nullable<double> HotelFee { get; set; }

        [Display(Name = "其他费用")]
        public Nullable<double> OtherFee { get; set; }

        [Display(Name = "其他费用说明")]
        public string OtherFeeComment { get; set; }
    }
}
