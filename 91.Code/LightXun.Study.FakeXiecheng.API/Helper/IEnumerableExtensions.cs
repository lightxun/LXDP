﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Helper
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source,
            string fields
        )
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            // 避免在列表中遍历数据, 创建一个属性信息列表
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // 希望返回动态类型对象 ExpandoObject 所有的属性
                var propertyInfos = typeof(TSource)
                    .GetProperties(
                        BindingFlags.IgnoreCase |    // 忽略大小写
                        BindingFlags.Public |        // 公有成员
                        BindingFlags.Instance);      // 非静态方法

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                // 逗号来分隔字段字符串
                var fieldsAfterSplit = fields.Split(',');

                foreach(var field in fieldsAfterSplit)
                {
                    // 去掉收尾多余的空格, 获得属性名称
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if(propertyInfo == null)
                    {
                        throw new Exception($"属性 {propertyName} 找不到 {typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach(TSource sourceObject in source)
            {
                // 创建动态类型对象, 创建数据塑形队形
                var dataShapedObject = new ExpandoObject();

                foreach(var propertyInfo in propertyInfoList)
                {
                    // 获得对应属性的真实数据
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
