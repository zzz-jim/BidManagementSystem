
namespace ScientificResearch.Utility.Enums
{
    public enum ApplicationStatus
    {
      
        /// <summary>
        /// 申请未上报
        /// </summary>
        AplicationWriting,

        /// <summary>
        /// 申请审批中（上报）
        /// </summary>
        ApplicationApproving,

        /// <summary>
        /// 申请驳回
        /// </summary>
        ApplicationRejected,

        /// <summary>
        /// 申请已通过
        /// </summary>
        ApplicationApproved,

        /// <summary>
        /// 项目未确立
        /// </summary>
        ProjectEstablishing,

        /// <summary>
        /// 项目未立项
        /// </summary>
        ProjectRejected,

        /// <summary>
        /// 项目已确立
        /// </summary>
        ProjectEstablished,

        /// <summary>
        /// 大的项目过程中
        /// </summary>
        BigProjectProcessing,

        /// <summary>
        /// 合同未上报
        /// </summary>
        ContractSigning,

        /// <summary>
        /// 合同记录
        /// </summary>
        ContractSigned,

        /// <summary>
        /// 项目过程中
        /// </summary>
        ProjectProcessing,

        /// <summary>
        /// 结题报告未填写
        /// </summary>
        ConcludeUnWrite,

        /// <summary>
        /// 结题报告未上报
        /// </summary>
        ConcludeUnSubmit,

        /// <summary>
        /// 结题报告审核中
        /// </summary>
        ProjectConcluding,

        /// <summary>
        /// 结题报告被驳回
        /// </summary>
        ConcludeRejected,

        /// <summary>
        /// 项目结束
        /// </summary>
        ProjectConcluded,

        /// <summary>
        /// 延期申请未上报
        /// </summary>
        ExtensionUnWrite,

        /// <summary>
        /// 延期申请审批中（科员，科长）
        /// </summary>
        ExtensionRequestApproving,

        /// <summary>
        /// 延期申请已经通过审批
        /// </summary>
        ExtensionAgreed,

        /// <summary>
        /// 延期申请被驳回
        /// </summary>
        ExtensionRejected,

        ///继教通知状态
        /// <summary>
        /// 通知未建立
        /// </summary>
        NoticeUnwrite,

        /// <summary>
        /// 通知未下发
        /// </summary>
        NoticeNotIssued,

        /// <summary>
        /// 已发送通知
        /// </summary>
        SentNotice,

        /// <summary>
        /// 添加参会人员
        /// </summary>
        AddMeetingPerson,

        /// <summary>
        /// 授予学分中
        /// </summary>
        GiveCrediting,

        /// <summary>
        /// 已授予学分
        /// </summary>
        GavenCredited,

        ///外出进修
        /// <summary>
        /// 借款登记未填写
        /// </summary>
        BorrowRegistration,

       /// <summary>
       ///  借款登记保存中
       /// </summary>
        BorrowRegistrationSave,

        /// <summary>
        /// 承诺书未同意
        /// </summary>
        PromiseBookNotAgree,

        /// <summary>
        /// 派出证明未填写
        /// </summary>
        SendProof,

        /// <summary>
        /// 派出证明填写中
        /// </summary>
        SendProofSave,

        /// <summary>
        /// 派出证明已提交(审批中)
        /// </summary>
        SendProofSubmit,

        /// <summary>
        /// 派出证明驳回
        /// </summary>
        SendProofReject,

        /// <summary>
        /// 完成情况未填写
        /// </summary>
        CompletionNot,

        /// <summary>
        /// 完成情况保存
        /// </summary>
        Completing,

        /// <summary>
        /// 完成情况提交（审批中）
        /// </summary>
        CompletSubmit,
        
        /// <summary>
        /// 完成情况审批完成
        /// </summary>
        Completed,

        /// <summary>
        /// 完成情况驳回
        /// </summary>
        CompletReject,

        /// <summary>
        /// 任务跟踪未填写
        /// </summary>
        TaskTrackNot,

        /// <summary>
        /// 任务跟踪
        /// </summary>
        TaskTracking,

        ///研究生
        /// <summary>
        /// 费用登记未填写
        /// </summary>
        CostRegistNot,

        /// <summary>
        /// 费用登记已填写
        /// </summary>
        CostRegistSubmit,

        /// <summary>
        /// 费用登记保存
        /// </summary>
        CostRegistSave,

        /// <summary>
        /// 书本费未登记
        /// </summary>
        BookCostRegistNot,

        /// <summary>
        /// 书本费已登记
        /// </summary>
        BookCostRegistSubmit,

        /// <summary>
        /// 结业证书未填写
        /// </summary>
        GraduationCertificateNot,

        /// <summary>
        /// 结业证书保存
        /// </summary>
        GraduationCertificateSave,

        /// <summary>
        /// 结业证书提交
        /// </summary>
        GraduationCertificateSubmit,

        /// <summary>
        /// 结业证书审批
        /// </summary>
        GraduationCertificateAgree,

        /// <summary>
        /// 结业证书驳回
        /// </summary>
        GraduationCertificateReject,

        /// <summary>
        /// 奖励登记未填写
        /// </summary>
        RewardSituationNot,

        /// <summary>
        /// 奖励登记已发放
        /// </summary>
        RewardSituationSubmit,

        /// <summary>
        /// 研究生专升本未授予学分
        /// </summary>
        NotGrantCredits,

        /// <summary>
        /// 授予学分
        /// </summary>
        GrantCredits,
    }
}