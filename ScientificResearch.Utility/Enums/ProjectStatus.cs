
namespace ScientificResearch.Utility.Enums
{
    /// <summary>
    /// 政府采购项目状态流程
    /// </summary>
    public enum BiddingProjectStatus
    {
        /// <summary>
        /// 项目注册登记中
        /// </summary>
        ProjectRegitering,//step 1 

        /// <summary>
        /// 项目已注册
        /// </summary>
        ProjectRegitered,//step 2

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
        /// 招标已公告
        /// </summary>
        TenderNoticeAnnounced,//step 3

        /// <summary>
        /// 报名情况 BidBondInfo
        /// </summary>
        RegisterInfo,

        /// <summary>
        /// 保证金情况 BidBondInfo
        /// </summary>
        BidBondInfo,

        /// <summary>
        /// 开评标资料
        /// </summary>
        OpenBidsDocument,

        /// <summary>
        /// 中标通知书
        /// </summary>
        BidWinnerNotice,

        /// <summary>
        /// 中标通知已公示，结束
        /// </summary>
        BidWinnerNoticeAnnounced,// step 4

        /// <summary>
        /// 文档存档，结束
        /// </summary>
        Archived// step 4
    }
}