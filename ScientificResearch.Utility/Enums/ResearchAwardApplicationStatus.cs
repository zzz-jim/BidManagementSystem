
namespace ScientificResearch.Utility.Enums
{
    public enum ResearchAwardApplicationStatus
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
    }
}