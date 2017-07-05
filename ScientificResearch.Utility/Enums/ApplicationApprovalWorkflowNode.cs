
namespace ScientificResearch.Utility.Enums
{

    public enum ApplicationApprovalWorkflowNode
    {
        /// <summary>
        /// 项目申请书审批流程
        /// </summary>
        部门经理审批 = 0,
        科教科科员审批,
        科教科科长审批,
        超级管理员审批审批,
        // 结束
        结束,
        /// <summary>
        /// 经费报销流程
        /// </summary>
        填写报销单,
        //部门经理审批,
        项目负责人审批,
        //科教科科长审批,
        财务确认,
        /// <summary>
        /// 论文申请书审批流程
        /// </summary>
        //护理部审核,
        护理部审核,

        /// <summary>
        /// 论文登记审批流程
        /// </summary>
        //初审,
        初审,

        /// <summary>
        /// 学术会议公务出差审批流程
        /// </summary>
        住院医实习生负责,

        /// <summary>
        /// 项目延期申请流程
        /// </summary>
        延期申请通过,
    }
}