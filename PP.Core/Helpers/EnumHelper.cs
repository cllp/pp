﻿using System;
using System.ComponentModel;

namespace PP.Core.Helpers
{
    public static class EnumHelper
    {
        public static string[] EnumToList<T>()
        {
            return System.Enum.GetNames(typeof(T));
        }

        public static Array EnumToArray<T>()
        {
            return System.Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int EnumToInt(System.Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts a string to a specified enumerator.
        /// </summary>
        /// <typeparam name="T">Enumerator to convert string to.</typeparam>
        /// <param name="name">String to convert to enumerator.</param>
        /// <returns>Returns the value from the specified enumerator.</returns>
        public static T StringToEnum<T>(string name)
        {
            return (T)System.Enum.Parse(typeof(T), name);
        }

        /// <summary>
        /// ex. EnumHelper.ToEnum<int, ClassificationCountry>(4);
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TInput, TEnum>(this TInput value)
        {
            Type type = typeof(TEnum);

            if (value == null)
            {
                throw new ArgumentException("Value is null or empty.", "value");
            }

            if (!type.IsEnum)
            {
                throw new ArgumentException("Enum expected.", "TEnum");
            }

            return (TEnum)System.Enum.Parse(type, value.ToString(), true);
        }

        /// <summary>
        /// Get description from enum 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi == null) return value.ToString();
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        } 
    }
}
