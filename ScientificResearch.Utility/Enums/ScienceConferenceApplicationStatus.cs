
namespace ScientificResearch.Utility.Enums
{
    public enum ScienceConferenceApplicationStatus
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
        /// 会议过程中
        /// </summary>
        MeetingProcess,

        /// <summary>
        /// 会议结束
        /// </summary>
        MeetingEnd,
    }
}