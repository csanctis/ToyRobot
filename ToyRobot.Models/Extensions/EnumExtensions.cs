using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ToyRobot.Models.Extensions;

public static class EnumExtensions
{
    //This is a extension class of enum
    public static string GetEnumDisplayName(this Enum enumType)
    {
        var member = enumType.GetType().GetMember(enumType.ToString());
        var memberInfo = member.FirstOrDefault();
        return memberInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? enumType.ToString();
    }
}