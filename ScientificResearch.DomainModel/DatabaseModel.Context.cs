﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScientificResearch.DomainModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CSPostOAEntities : DbContext
    {
        public CSPostOAEntities()
            : base("name=CSPostOAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ERPAnPai> ERPAnPai { get; set; }
        public virtual DbSet<ERPBaoJia> ERPBaoJia { get; set; }
        public virtual DbSet<ERPBaoXiao> ERPBaoXiao { get; set; }
        public virtual DbSet<ERPBBSBanKuai> ERPBBSBanKuai { get; set; }
        public virtual DbSet<ERPBBSTieZi> ERPBBSTieZi { get; set; }
        public virtual DbSet<ERPBook> ERPBook { get; set; }
        public virtual DbSet<ERPBookJieHuan> ERPBookJieHuan { get; set; }
        public virtual DbSet<ERPBuMen> ERPBuMen { get; set; }
        public virtual DbSet<ERPBuyChanPin> ERPBuyChanPin { get; set; }
        public virtual DbSet<ERPBuyOrder> ERPBuyOrder { get; set; }
        public virtual DbSet<ERPCarBaoXian> ERPCarBaoXian { get; set; }
        public virtual DbSet<ERPCarBaoYang> ERPCarBaoYang { get; set; }
        public virtual DbSet<ERPCarInfo> ERPCarInfo { get; set; }
        public virtual DbSet<ERPCarJiaYou> ERPCarJiaYou { get; set; }
        public virtual DbSet<ERPCarLog> ERPCarLog { get; set; }
        public virtual DbSet<ERPCarShiYong> ERPCarShiYong { get; set; }
        public virtual DbSet<ERPCarWeiHu> ERPCarWeiHu { get; set; }
        public virtual DbSet<ERPCarWeiZhang> ERPCarWeiZhang { get; set; }
        public virtual DbSet<ERPCommon> ERPCommon { get; set; }
        public virtual DbSet<ERPContract> ERPContract { get; set; }
        public virtual DbSet<ERPContractChanPin> ERPContractChanPin { get; set; }
        public virtual DbSet<ERPCrmSetting> ERPCrmSetting { get; set; }
        public virtual DbSet<ERPCustomFuWu> ERPCustomFuWu { get; set; }
        public virtual DbSet<ERPCustomHuiFang> ERPCustomHuiFang { get; set; }
        public virtual DbSet<ERPCustomInfo> ERPCustomInfo { get; set; }
        public virtual DbSet<ERPCustomNeed> ERPCustomNeed { get; set; }
        public virtual DbSet<ERPCYDIC> ERPCYDIC { get; set; }
        public virtual DbSet<ERPDangAn> ERPDangAn { get; set; }
        public virtual DbSet<ERPDanWeiInfo> ERPDanWeiInfo { get; set; }
        public virtual DbSet<ERPFileList> ERPFileList { get; set; }
        public virtual DbSet<ERPGongGao> ERPGongGao { get; set; }
        public virtual DbSet<ERPGuDing> ERPGuDing { get; set; }
        public virtual DbSet<ERPGuDingJiLu> ERPGuDingJiLu { get; set; }
        public virtual DbSet<ERPHuiBao> ERPHuiBao { get; set; }
        public virtual DbSet<ERPHuiYuan> ERPHuiYuan { get; set; }
        public virtual DbSet<ERPJiangCheng> ERPJiangCheng { get; set; }
        public virtual DbSet<ERPJiangChengZhiDu> ERPJiangChengZhiDu { get; set; }
        public virtual DbSet<ERPJianLi> ERPJianLi { get; set; }
        public virtual DbSet<ERPJiaoSe> ERPJiaoSe { get; set; }
        public virtual DbSet<ERPJinDu> ERPJinDu { get; set; }
        public virtual DbSet<ERPJiXiao> ERPJiXiao { get; set; }
        public virtual DbSet<ERPJiXiaoCanShu> ERPJiXiaoCanShu { get; set; }
        public virtual DbSet<ERPJSDIC> ERPJSDIC { get; set; }
        public virtual DbSet<ERPJuanKu> ERPJuanKu { get; set; }
        public virtual DbSet<ERPJXDetails> ERPJXDetails { get; set; }
        public virtual DbSet<ERPKaoQin> ERPKaoQin { get; set; }
        public virtual DbSet<ERPKaoQinSetting> ERPKaoQinSetting { get; set; }
        public virtual DbSet<ERPLanEmail> ERPLanEmail { get; set; }
        public virtual DbSet<ERPLanEmailShou> ERPLanEmailShou { get; set; }
        public virtual DbSet<ERPLinkLog> ERPLinkLog { get; set; }
        public virtual DbSet<ERPLinkMan> ERPLinkMan { get; set; }
        public virtual DbSet<ERPLiRun> ERPLiRun { get; set; }
        public virtual DbSet<ERPMeeting> ERPMeeting { get; set; }
        public virtual DbSet<ERPMianShi> ERPMianShi { get; set; }
        public virtual DbSet<ERPMobile> ERPMobile { get; set; }
        public virtual DbSet<ERPNetEmail> ERPNetEmail { get; set; }
        public virtual DbSet<ERPNetEmailShou> ERPNetEmailShou { get; set; }
        public virtual DbSet<ERPNForm> ERPNForm { get; set; }
        public virtual DbSet<ERPNFormType> ERPNFormType { get; set; }
        public virtual DbSet<ERPNWorkDetails> ERPNWorkDetails { get; set; }
        public virtual DbSet<ERPNWorkFlow> ERPNWorkFlow { get; set; }
        public virtual DbSet<ERPNWorkFlowNode> ERPNWorkFlowNode { get; set; }
        public virtual DbSet<ERPNWorkFlowToDoUser> ERPNWorkFlowToDoUser { get; set; }
        public virtual DbSet<ERPNWorkFlowWT> ERPNWorkFlowWT { get; set; }
        public virtual DbSet<ERPNWorkFlowWTLog> ERPNWorkFlowWTLog { get; set; }
        public virtual DbSet<ERPNWorkToDo> ERPNWorkToDo { get; set; }
        public virtual DbSet<ERPOffice> ERPOffice { get; set; }
        public virtual DbSet<ERPPeiXun> ERPPeiXun { get; set; }
        public virtual DbSet<ERPPeiXunRiJi> ERPPeiXunRiJi { get; set; }
        public virtual DbSet<ERPPeiXunXiaoGuo> ERPPeiXunXiaoGuo { get; set; }
        public virtual DbSet<ERPPinShen> ERPPinShen { get; set; }
        public virtual DbSet<ERPProduct> ERPProduct { get; set; }
        public virtual DbSet<ERPProject> ERPProject { get; set; }
        public virtual DbSet<ERPRedHead> ERPRedHead { get; set; }
        public virtual DbSet<ERPRenShiHeTong> ERPRenShiHeTong { get; set; }
        public virtual DbSet<ERPReport> ERPReport { get; set; }
        public virtual DbSet<ERPReportType> ERPReportType { get; set; }
        public virtual DbSet<ERPRiZhi> ERPRiZhi { get; set; }
        public virtual DbSet<ERPSaveFileName> ERPSaveFileName { get; set; }
        public virtual DbSet<ERPSerils> ERPSerils { get; set; }
        public virtual DbSet<ERPSheBei> ERPSheBei { get; set; }
        public virtual DbSet<ERPShenPi> ERPShenPi { get; set; }
        public virtual DbSet<ERPShiShi> ERPShiShi { get; set; }
        public virtual DbSet<ERPShouKuan> ERPShouKuan { get; set; }
        public virtual DbSet<ERPSongYang> ERPSongYang { get; set; }
        public virtual DbSet<ERPSupplyLink> ERPSupplyLink { get; set; }
        public virtual DbSet<ERPSupplys> ERPSupplys { get; set; }
        public virtual DbSet<ERPSystemSetting> ERPSystemSetting { get; set; }
        public virtual DbSet<ERPTalkInfo> ERPTalkInfo { get; set; }
        public virtual DbSet<ERPTalkOnlineUser> ERPTalkOnlineUser { get; set; }
        public virtual DbSet<ERPTalkSetting> ERPTalkSetting { get; set; }
        public virtual DbSet<ERPTaskFP> ERPTaskFP { get; set; }
        public virtual DbSet<ERPTelFile> ERPTelFile { get; set; }
        public virtual DbSet<ERPTiKu> ERPTiKu { get; set; }
        public virtual DbSet<ERPTiKuKaoShi> ERPTiKuKaoShi { get; set; }
        public virtual DbSet<ERPTiKuKaoShiJieGuo> ERPTiKuKaoShiJieGuo { get; set; }
        public virtual DbSet<ERPTiKuShiJuan> ERPTiKuShiJuan { get; set; }
        public virtual DbSet<ERPTiKuShiJuanSet> ERPTiKuShiJuanSet { get; set; }
        public virtual DbSet<ERPTiKuType> ERPTiKuType { get; set; }
        public virtual DbSet<ERPTongXunLu> ERPTongXunLu { get; set; }
        public virtual DbSet<ERPTouSu> ERPTouSu { get; set; }
        public virtual DbSet<ERPTreeList> ERPTreeList { get; set; }
        public virtual DbSet<ERPUser> ERPUser { get; set; }
        public virtual DbSet<ERPUserDesk> ERPUserDesk { get; set; }
        public virtual DbSet<ERPVote> ERPVote { get; set; }
        public virtual DbSet<ERPWorkPlan> ERPWorkPlan { get; set; }
        public virtual DbSet<ERPWorkRiZhi> ERPWorkRiZhi { get; set; }
        public virtual DbSet<ERPXCDetails> ERPXCDetails { get; set; }
        public virtual DbSet<ERPXinChou> ERPXinChou { get; set; }
        public virtual DbSet<ERPXinChouCanShu> ERPXinChouCanShu { get; set; }
        public virtual DbSet<ERPXueXi> ERPXueXi { get; set; }
        public virtual DbSet<ERPXueXiXinDe> ERPXueXiXinDe { get; set; }
        public virtual DbSet<ERPYinZhang> ERPYinZhang { get; set; }
        public virtual DbSet<ERPYinZhangLog> ERPYinZhangLog { get; set; }
        public virtual DbSet<FundsRecord> FundsRecord { get; set; }
        public virtual DbSet<ProjectRecord> ProjectRecord { get; set; }
        public virtual DbSet<TravelFundsDetail> TravelFundsDetail { get; set; }
        public virtual DbSet<ProjectBonusCredit> ProjectBonusCredit { get; set; }
        public virtual DbSet<PaperMagazine> PaperMagazine { get; set; }
        public virtual DbSet<PaperMagazineLevel> PaperMagazineLevel { get; set; }
        public virtual DbSet<PaperMagazineType> PaperMagazineType { get; set; }
        public virtual DbSet<PaperBonusCredit> PaperBonusCredit { get; set; }
        public virtual DbSet<FundsThreshold> FundsThreshold { get; set; }
        public virtual DbSet<ContinuingEducationRecord> ContinuingEducationRecord { get; set; }
        public virtual DbSet<ProjectFile> ProjectFile { get; set; }
        public virtual DbSet<ProjectRegistration> ProjectRegistration { get; set; }
    }
}
