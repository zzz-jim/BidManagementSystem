using ScientificResearch.Utility.Constants;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ScientificResearch.Utility.Helper
{
    public static class Hepler
    {
        /// <summary>
        /// 从表单设计的itemlist和页面传过来的key/value集合中解析出字段的值和字段说明
        /// </summary>
        /// <param name="collection">页面传过来的key/value集合</param>
        /// <param name="itemList">表单设计的itemlist</param>
        /// <param name="formKeys">输出的字段说明，以#号分隔</param>
        /// <param name="formvalues">输出的字段值，以#号分隔</param>
        public static void GetFormKeyValueAndRemark(NameValueCollection collection, string itemList, out string formKeys, out string formvalues)
        {
            // itemlist: ||BeiYong1_项目编号|WenHao_项目名称|Drop111111111_项目类别|Text111111111_项目内容|
            var keyRemarkPairArray = itemList.Split(Constant.SplitChar);
            string[] remarkArray = new string[keyRemarkPairArray.Length];
            Dictionary<string, string> keyRemarkValuePair = new Dictionary<string, string>();
            foreach (var item in keyRemarkPairArray)
            {
                StringBuilder formValues = new StringBuilder();
                var filedRemark = item.Split(Constant.UnderlineChar);
                if (filedRemark.Length == 2 && !string.IsNullOrEmpty(filedRemark.First()) && !string.IsNullOrEmpty(filedRemark.Last()))
                {
                    var formKey = filedRemark.First();
                    keyRemarkValuePair.Add(filedRemark.Last(), collection[formKey]);
                }
            }

            formKeys = string.Join(Constant.SharpString, keyRemarkValuePair.Keys.ToArray());
            formvalues = string.Join(Constant.SharpString, keyRemarkValuePair.Values.ToArray());
        }

    }
}
