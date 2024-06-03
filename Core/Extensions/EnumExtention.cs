using System.ComponentModel;

namespace Core.Extensions
{
    public static class EnumExtention
    {
        public static string GetDescriptionFromEnum(this Enum value)
        {
            DescriptionAttribute? attribute = value.GetType()
                                                   .GetField(value.ToString())!
                                                   .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                   .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
