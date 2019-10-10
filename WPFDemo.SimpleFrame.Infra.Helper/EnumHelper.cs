using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举获取属性值列表
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static List<StatusDictionary> GetSelectList(Type enumType)
        {
            var result = (from object e in Enum.GetValues(enumType)
                          let e1 = (int)e
                          select new StatusDictionary
                          {
                              Id = e1.ToString(),
                              Name = ((Enum)e).ToString(),
                              Description = GetDescription((Enum)e),
                          }).ToList();
            return result;
        }

        /// <summary>
        /// 根据枚举获取属性值列表
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static List<EnumStructInfo> GetEnumStructInfo(this Type enumType)
        {
            var result = (from object e in Enum.GetValues(enumType)
                          let e1 = (int)e
                          select new EnumStructInfo
                          {
                              Id = e1.ToString(),
                              Name = ((Enum)e).ToString(),
                              Description = GetDescription((Enum)e),
                              AdditionalInfo = GetAdditionalInfo((Enum)e),
                          }).ToList();
            return result;
        }

        /// <summary>
        /// 根据枚举成员获取属性描述
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum e)
        {
            //获取枚举的Type类型对象
            var type = e.GetType();

            //获取枚举的所有字段
            var field = type.GetField(e.ToString());
            if (field == null) return e.ToString();

            //第二个参数true表示查找EnumDisplayNameAttribute的继承链
            var cus = field.GetCustomAttributes(typeof(DescriptionAttribute), true);

            //如果没有找到自定义属性，直接返回属性项的名称
            var des = cus.Length > 0 && cus[0] != null && cus[0] is DescriptionAttribute obj ? obj.Description : e.ToString();

            return des;
        }

        public static string GetAdditionalInfo(this Enum e)
        {
            //获取枚举的Type类型对象
            var type = e.GetType();

            //获取枚举的所有字段
            var field = type.GetField(e.ToString());

            //第二个参数true表示查找EnumDisplayNameAttribute的继承链
            var cus = field.GetCustomAttributes(typeof(AdditionalInfoAttribute), true);

            //如果没有找到自定义附加属性，直接返回属性项的名称
            var info = cus.Length > 0 && cus[0] is AdditionalInfoAttribute obj ? obj.AdditionalInfo : e.ToString();

            return info;
        }
    }

    public class StatusDictionary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class EnumStructInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class AdditionalInfoAttribute : Attribute
    {
        public AdditionalInfoAttribute(string info)
        {
            this.AdditionalInfo = info;
        }

        public string AdditionalInfo { get; set; }
    }
}
