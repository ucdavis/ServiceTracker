using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ServiceTracker.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            DisplayAttribute displayAttribute =  enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>();
            string displayName = displayAttribute?.GetName();
            return displayName ?? enumValue.ToString();
        }

        public static List<string> GetListOfDisplayNames<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDisplayName()).ToList();
        }
    }
}