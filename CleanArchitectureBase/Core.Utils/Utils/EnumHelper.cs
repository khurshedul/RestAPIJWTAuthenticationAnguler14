using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Utils.Utils
{
    public static class EnumHelper
    {
        public static SortedList GetEnumForBind(this Type enumeration)
        {
            var items = Enum.GetValues(enumeration);

            SortedList sl = new SortedList();
            foreach (var item in items)
            {
                var fieldInfo = item.GetType().GetField(item.ToString());
                var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                sl.Add(item.ToString(), descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : item.ToString());
            }

            return sl;
        }

        public static SortedList GetEnumForBind<TEnum>() where TEnum : struct, Enum
        {
            var items = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            SortedList sl = new SortedList();
            foreach (var item in items)
            {
                sl.Add(item.ToString(), item.GetDescription());
            }

            return sl;
        }

        public static List<SelectListItem> ToSelectListItem(this Type enumeration)
        {
            var items = Enum.GetValues(enumeration);

            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var item in items)
            {
                var fieldInfo = item.GetType().GetField(item.ToString());
                var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                selectList.Add(new SelectListItem
                {
                    Value = item.ToString(),
                    Text = descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : item.ToString(),
                });
            }

            return selectList;
        }

        //public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        //{
        //    return Enumerations.GetEnumDescription((Enum)(object)((TEnum)EnumValue));
        //}

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static string ToDescriptionString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
