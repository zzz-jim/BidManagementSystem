
namespace ScientificResearch.DataTransferModel
{
    public class ScienceResearchAmountOfApplicationStatisticTransferObject
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string Modeule { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectApprovalType { get; set; }
        /// <summary>
        /// 申请书个数
        /// </summary>
        public int ApplicationCount { get; set; }
        /// <summary>
        /// 申请书申报成功个数
        /// </summary>
        public int ApplicationSuccessCount { get; set; }
    }
}
