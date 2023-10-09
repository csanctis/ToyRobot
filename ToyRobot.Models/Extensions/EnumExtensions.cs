using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ToyRobot.Models.Extensions;

public static class EnumExtensions
{
    //This is a extension class of enum
    public static string GetEnumDisplayName(this Enum enumType)
    {
        return enumType.GetType().GetMember(enumType.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            .Name;
    }
}