using System;

using ScientificResearch.Utility.Enums;

namespace ScientificResearch.Utility.Helper
{
    public static class EnumToStringHelper
    {
        /// <summary>
        /// Converts a string to ApplicationSortField Enum type
        /// </summary>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static ApplicationSortField ConvertStringToEnum(string sortField)
        {
            ApplicationSortField sorter = (ApplicationSortField)Enum.Parse(typeof(ApplicationSortField), sortField);

            return sorter;
        }

        /// <summary>
        /// 继续教育字符串转换成枚举类型
        /// </summary>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static ContinuingRecordSortField ContinuingConvertStringToEnum(string sortField)
        {
            ContinuingRecordSortField sorter = (ContinuingRecordSortField)Enum.Parse(typeof(ContinuingRecordSortField), sortField);

            return sorter;
        }
        /// <summary>
        /// Converts a string to ModuleNameOfScienceResearch Enum type
        /// </summary>
        /// <param name="ModuleName"></param>
        /// <returns></returns>
        public static ModuleNameOfScienceResearch ConvertStringToEnumOfModuleName(string ModuleName)
        {
            ModuleNameOfScienceResearch modulename = (ModuleNameOfScienceResearch)Enum.Parse(typeof(ModuleNameOfScienceResearch), ModuleName);

            return modulename;
        }
    }
}
