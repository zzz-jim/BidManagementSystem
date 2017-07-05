using ScientificResearch.Utility.Enums;
using System;

namespace ScientificResearch.Utility.Helper
{
    public static class CommonHelper
    {
        /// <summary>
        /// Converts a string to ApplicationSortField Enum type
        /// </summary>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static string GenerateProjectNumber(ApplicationType type)
        {
            // K 1503 14 11 02 9281
            string result = string.Empty;

            switch (type)
            {
                case ApplicationType.ScienceResearch:
                    result = "K";
                    break;
                case ApplicationType.GoodSubject:
                    result = "Z";
                    break;
                case ApplicationType.SubjectLeader:
                    result = "D";
                    break;
                case ApplicationType.PublishPaper:
                    result = "F";
                    break;
                case ApplicationType.ScienceConference:
                    result = "H";
                    break;
                case ApplicationType.ResearchAward:
                    result = "C";
                    break;
                default:
                    break;
            }

            result += DateTime.Now.ToString("yyyyMMddHHmmss");

            return result;
        }

        /// <summary>
        /// 返回设置字段的可写和可见属性的js脚本
        /// </summary>
        /// <param name="CanWriteSetStr">可写字段集合</param>
        /// <param name="SecretSetStr">保密字段集合</param>
        /// <param name="formItemList">ERPNWorkForm表的ItemsList字段以.Split('|')成的数组</param>
        /// <returns>返回设置字段的可写和可见属性的js 脚本</returns>
        public static string SetTheWriteAndHiddenField(string CanWriteSetStr, string SecretSetStr, string[] formItemList)
        {
            string PiLiangSet = string.Empty;

            for (int ItemNum = 0; ItemNum < formItemList.Length; ItemNum++)
            {
                if (formItemList[ItemNum].ToString().Trim().Length > 0)
                {
                    if (StrIfIn(formItemList[ItemNum].ToString(), CanWriteSetStr) == false)//不属于可写字段
                    {
                        PiLiangSet = PiLiangSet + "document.getElementById(\"" + formItemList[ItemNum].ToString().Split('_')[0] + "\").readOnly=true;";//readOnly
                    }
                    else
                    {
                        PiLiangSet = PiLiangSet + "document.getElementById(\"" + formItemList[ItemNum].ToString().Split('_')[0] + "\").readOnly=false;";//readOnly
                    }
                    if (StrIfIn(formItemList[ItemNum].ToString(), SecretSetStr) == true)//属于保密字段
                    {
                        PiLiangSet = PiLiangSet + "document.getElementById(\"" + formItemList[ItemNum].ToString().Split('_')[0] + "\").style.visibility=\"hidden\";";
                    }
                    else
                    {
                        PiLiangSet = PiLiangSet + "document.getElementById(\"" + formItemList[ItemNum].ToString().Split('_')[0] + "\").style.visibility=\"visible\";";
                    }
                }
            }

            return PiLiangSet;
        }

        /// <summary>
        /// 判断Str1是否是在Str2这个长的字符串中
        /// </summary>
        /// <param name="Str1"></param>
        /// <param name="Str2"></param>
        /// <returns></returns>
        public static bool StrIfIn(string Str1, string Str2)
        {
            if (Str2.IndexOf(Str1) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
