using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            try
            {
                var name = enumValue.GetType()
                            ?.GetMember(enumValue.ToString())
                            ?.First()
                            ?.GetCustomAttribute<DisplayAttribute>()
                            ?.GetName();

                return name ?? string.Empty;
            }
            catch (Exception)
            {
                return enumValue.ToString();
            }
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ParseEnumAndGetDisplayName(this string? value, Type type)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            var name = Enum.Parse(type, value, true);

            var name2 = (name as Enum)?.GetDisplayName() ?? string.Empty;

            return name2;
        }
    }
}
