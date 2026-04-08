using System.ComponentModel;
using System.Reflection;

namespace RetirementTime.Domain.Entities.Common;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the [Description("...")] value, falling back to ToString() if not present.
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field?.GetCustomAttribute<DescriptionAttribute>() is { } attr)
            return attr.Description;

        return value.ToString();
    }
}