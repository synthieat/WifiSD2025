using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Common.Extensions
{
    public static class EnumExtension
    {
        /* Lokalisierte Beschreibung für einen ENUM-Wert abfragen */
        public static string GetDescription<T>(this T value)
            where T : Enum
        {
            var fieldInfo = value.GetType().GetField(value.ToString()); 
            var descriptionAttribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            return string.Empty;
        }

        public static IEnumerable<string> GetDescriptions<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(s => GetDescription(s));            
        }

        public static IEnumerable<KeyValuePair<object, string>> EnumToList<T>()
            where T : Enum
        {
            var result = new List<KeyValuePair<object, string>>();
            var enumType = typeof(T);
            foreach(Enum _enum in Enum.GetValues(enumType))
            {
                result.Add(new KeyValuePair<object, string>((object)_enum, GetDescription(_enum)));
            }

            return result;
        }

    }
}
