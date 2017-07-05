
namespace UI.ScientificResearch.Extensions
{
    /// <summary>
    /// The enum type index or the key name of the session value.
    /// </summary>
    public enum SessionKeyEnum
    {
        UserModel = 0,
        User,
        Role,
        SectionId,
        SectionName,
        RolesName,
        UserRoles,

        // search criteria
        /// <summary>
        /// 项目名称
        /// </summary>
        ProjectName,

        /// <summary>
        /// 项目状态
        /// </summary>
        ProjectStatus,

        /// <summary>
        /// 项目是否冻结
        /// </summary>
        IsLocked,

        /// <summary>
        /// 项目查找开始时间
        /// </summary>
        StartTime,

        /// <summary>
        /// 项目查找结束时间
        /// </summary>
        EndTime,

        // Funds list search criteria
        /// <summary>
        /// 
        /// </summary>
        FundsCriteriaFundsName,
        FundsCriteriaIsIncome,
        FundsCriteriaFundsType,
        FundsCriteriaStartTime,
        FundsCriteriaEndTime,
        FundsCriteriaUserName,
        FundsCriteriaState,
    }
}