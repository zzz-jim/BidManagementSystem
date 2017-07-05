using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ScientificResearch.Utility
{
    /// <summary>
    /// 实体转换扩展辅助类
    /// </summary>
    /// <typeparam name="T">需要从DataTable得到的实体模型的类型</typeparam>
    public static class DataTableConvertHelper<T> where T : new()
    {
        /// <summary>
        /// 将DataTable转换为任何模型的List列表
        /// </summary>
        /// <param name="dataTable">实体模型的数据来源</param>
        /// <returns>元素的类型为<c>T</c>的列表</returns>
        public static IList<T> ConvertToModel(DataTable dataTable)
        {
            #region validation

            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return new List<T>();
            }

            #endregion

            // 定义返回的结果集合
            IList<T> result = new List<T>();

            T model = new T();

            // 获得此模型类的所有公共属性
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var property in properties)
                {
                    var propertyName = property.Name;

                    // 检查DataTable是否包含列名和属性名的列
                    if (dataTable.Columns.Contains(propertyName))
                    {
                        // 判断此属性是否有Setter, 即可写
                        if (!property.CanWrite)
                        {
                            continue;
                        }

                        var propertyValue = row[propertyName];

                        if (propertyValue != DBNull.Value)
                        {
                            property.SetValue(model, propertyValue, null);
                        }
                    }
                }

                result.Add(model);
            }

            return result;
        }
    }
}