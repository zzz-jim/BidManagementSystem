
using System.Collections.Generic;
using System.Collections.Specialized;
namespace ScientificResearch.Utility.Helper
{
    public static class FormContentReplaceHelper
    {
        /// <summary>
        /// 用textAreaValue替换formContent中TextArea的值
        /// </summary>
        /// <param name="formContent">源字符串</param>
        /// <param name="textAreaId">要替换的textArea的Id</param>
        /// <param name="textAreaValue">新值</param>
        /// <returns>替换后的结果字符串</returns>
        public static string TextAreaReplaceMethod(string formContent, string textAreaId, string textAreaValue)
        {
            string baseelement = textAreaId; //基础元素
            int baseposition = formContent.IndexOf(baseelement);
            int startPosition = formContent.IndexOf('>', baseposition);
            int endPosition = formContent.IndexOf("</textarea>", baseposition);

            formContent = formContent.Remove(startPosition + 1, endPosition - (startPosition + 1));

            int targetposition = formContent.IndexOf('<', baseposition);

            formContent = formContent.Insert(targetposition, textAreaValue);

            return formContent;
        }

        /// <summary>
        /// 用textboxValue替换formContent中Textbox和date的值
        /// </summary>
        /// <param name="formContent">源字符串</param>
        /// <param name="textboxId">要替换的textbox的Id</param>
        /// <param name="textboxValue">新值</param>
        /// <returns>替换后的结果字符串</returns>
        public static string TextboxReplaceMethod(string formContent, string textboxId, string textboxValue)
        {
            string oldvalue;
            string newvalue;

            oldvalue = "id=\"" + textboxId + "\"";
            newvalue = oldvalue + " value=\"" + textboxValue + "\"";

            formContent = formContent.Replace(oldvalue, newvalue);

            return formContent;
        }

        /// <summary>
        /// 替换formContent中CheckBox的值
        /// </summary>
        /// <param name="formContent">源字符串</param>
        /// <param name="textAreaId">要替换的CheckBox的Id</param>
        /// <returns>替换后的结果字符串</returns>
        public static string CheckBoxReplaceMethod(string formContent, string checkBoxId)
        {
            string oldvalue;
            string newvalue;

            oldvalue = "id=\"" + checkBoxId + "\"";
            newvalue = oldvalue + " checked";

            formContent = formContent.Replace(oldvalue, newvalue);

            return formContent;
        }

        /// <summary>
        /// 替换formContent中RadioButton的值
        /// </summary>
        /// <param name="formContent">源字符串</param>
        /// <param name="radioId">要替换的RadioButton的Id</param>
        /// <returns>替换后的结果字符串</returns>
        public static string RadioButtonReplaceMethod(string formContent, string radioId)
        {
            string oldvalue;
            string newvalue;

            oldvalue = "id=\"" + radioId + "\"";
            newvalue = oldvalue + " checked";

            formContent = formContent.Replace(oldvalue, newvalue);

            return formContent;
        }

        /// <summary>
        /// 用checkBoxValue替换formContent中DropdownList的值
        /// </summary>
        /// <param name="formContent">源字符串</param>
        /// <param name="textboxValue">新值</param>
        /// <returns>替换后的结果字符串</returns>
        public static string DropdownListReplaceMethod(string formContent, string dropSelectedValue)
        {
            string oldvalue;
            string newvalue;

            oldvalue = "value=" + dropSelectedValue;
            newvalue = "selected=" + "\"\"" + " value = " + dropSelectedValue;

            formContent = formContent.Replace(oldvalue, newvalue);

            return formContent;
        }

        /// <summary>
        /// 替换form content中的所有Drop，Date，Text和TextArea值
        /// </summary>
        /// <param name="formContent">要替换的form content源内容</param>
        /// <param name="collection">Form Collection 键值对集合</param>
        /// <returns></returns>
        public static string ReplaceFormContentValue(string formContent, NameValueCollection collection)
        {
            // Stores the form name and key of the form content.
            Dictionary<string, string> nameValuePair = new Dictionary<string, string>();

            // Step 1: Get key value dictionary
            foreach (var item in collection)
            {
                if (item.ToString().Contains("Drop") || item.ToString().Contains("Date") || item.ToString().Contains("Text") || item.ToString().Contains("TextArea")
                    || item.ToString().Contains("Rad") || item.ToString().Contains("CHK") || item.ToString().Contains("Num"))
                {
                    nameValuePair.Add(item.ToString(), collection[item.ToString()].ToString());
                }
                else
                {
                    // Empty
                }
            }

            //先去掉formcontent中全部<select>属性selected
            string oldattrvalue = "selected";
            string newattrvalue = "";
            formContent = formContent.Replace(oldattrvalue, newattrvalue);
            foreach (var item in nameValuePair)
            {
                if (item.Key.Contains("Text") || item.Key.Contains("Date") || item.Key.Contains("Num"))
                {
                    if (item.Key.Contains("TextArea"))
                    {
                        formContent = FormContentReplaceHelper.TextAreaReplaceMethod(formContent, item.Key, item.Value);
                    }
                    else
                    {
                        formContent = FormContentReplaceHelper.TextboxReplaceMethod(formContent, item.Key, item.Value);
                    }
                }
                else if (item.Key.Contains("Drop"))
                {
                    formContent = FormContentReplaceHelper.DropdownListReplaceMethod(formContent, item.Value);
                }
                else if (item.Key.Contains("Rad"))
                {
                    formContent = FormContentReplaceHelper.RadioButtonReplaceMethod(formContent, item.Value);
                }
                else if (item.Key.Contains("CHK"))
                {
                    formContent = FormContentReplaceHelper.CheckBoxReplaceMethod(formContent, item.Key);
                }
            }

            return formContent;
        }
    }
}
