
namespace ScientificResearch.Utility.Enums
{
    /// <summary>
    /// 政府采购项目状态流程
    /// </summary>
    public enum BiddingProjectStatus
    {
        /// <summary>
        /// 项目已注册
        /// </summary>
        ProjectRegitered,

        /// <summary>
        /// 招标合同资料上传
        /// </summary>
        Contract,

        /// <summary>
        /// 招标文件
        /// </summary>
        BiddingDocument,

        /// <summary>
        /// 招标公告
        /// </summary>
        TenderNotice,

        /// <summary>
        /// 报名保证金情况 BidBondInfo
        /// </summary>
        RegisterInfo,

        /// <summary>
        /// 开评标资料
        /// </summary>
        OpenBidsDocument,

        /// <summary>
        /// 中标通知书
        /// </summary>
        BidWinnerNotice,
    }
}