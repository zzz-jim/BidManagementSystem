using System;

namespace ScientificResearch.Utility.Helper
{
    /// <summary>
    ///     DB model和 DTO model转换器
    /// </summary>
    public static class ModelConvertor
    {
        public static void CopyTo<T>(this object source, T target)
            where T : class, new()
        {
            if (source == null)
                return;

            if (target == null)
                target = new T();

            //foreach (var property in target.GetType().GetProperties())
            //{
            //    var propertyInfo = source.GetType().GetProperty(property.Name);

            //    if (propertyInfo == null)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var propertyValue = propertyInfo.GetValue(source, null);
            //        if (propertyValue != null)
            //        {
            //            if (propertyValue.GetType().IsArray)
            //            {

            //            }
            //            target.GetType().InvokeMember(property.Name, BindingFlags.SetProperty, null, target, new object[] { propertyValue });
            //        }
            //    }
            //}

            //foreach (var field in target.GetType().GetFields())
            //{
            //    var fieldInfo = source.GetType().GetField(field.Name);

            //    if (fieldInfo == null)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var fieldValue = fieldInfo.GetValue(source);
            //        if (fieldValue != null)
            //        {
            //            target.GetType().InvokeMember(field.Name, BindingFlags.SetField, null, target, new object[] { fieldValue });
            //        }
            //    }
            //}
        }

        public static T ConvertTo<T>(this object source)
            where T : class, new()
        {
            if (source == null)
                return null;

            var target = new T();
            var typeOfTarget = typeof(T);

            try
            {
                foreach (var property in target.GetType().GetProperties())
                {
                    var propertyInfo = source.GetType().GetProperty(property.Name);

                    if (propertyInfo != null)
                    {
                        var propertyValue = propertyInfo.GetValue(source, null);

                        if (propertyValue != null)
                            typeOfTarget.GetProperty(property.Name).SetValue(target, propertyValue);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            //foreach (var field in target.GetType().GetFields())
            //{
            //    var fieldInfo = source.GetType().GetField(field.Name);

            //    if (fieldInfo == null)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var fieldValue = fieldInfo.GetValue(source);
            //        if (fieldValue != null)
            //        {
            //            target.GetType().InvokeMember(field.Name, BindingFlags.SetField, null, target, new object[] { fieldValue });
            //        }
            //    }
            //}

            return target;
        }
    }
}
