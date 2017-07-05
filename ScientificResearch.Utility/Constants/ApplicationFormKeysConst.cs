using System.Collections.Generic;

namespace ScientificResearch.Utility.Constants
{
    /// <summary>
    /// 存储申请书表单内容中的键值对的常量类
    /// </summary>
    public static class ApplicationFormKeysConst
    {
        public static Dictionary<string, string> KeyValuePairDictionary = new Dictionary<string, string>();

        static ApplicationFormKeysConst()
        {
            KeyValuePairDictionary.Add("Drop1364262284", string.Empty); // 项目类型
            KeyValuePairDictionary.Add("Drop968600384", string.Empty); // 立项类型
            KeyValuePairDictionary.Add("Date442495555", string.Empty); // 申请时间
            KeyValuePairDictionary.Add("Text435761615", string.Empty); // 申请人
            KeyValuePairDictionary.Add("Text289827346", string.Empty); // 科室
            KeyValuePairDictionary.Add("Text1783445882", string.Empty); // 项目负责人
            KeyValuePairDictionary.Add("Text309804476", string.Empty); // 项目参与人员
            KeyValuePairDictionary.Add("TextArea683159807", string.Empty); // 概述
        }
    }
}
