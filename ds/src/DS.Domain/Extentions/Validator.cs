using DS.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DS.Domain.Extentions;

public static class Validator
{
    public static List<string> ValidateRequired<T>(T obj) where T : class
    {
        var errors = new List<string>();

        if (obj is null)
        {
            errors.Add("object is null");

            return errors;
        }

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            if (!Attribute.IsDefined(prop, typeof(NotEmptyAttribute)))
                continue;

            var value = prop.GetValue(obj);

            if (value is null)
            {
                errors.Add($"{prop.Name} is null");
                continue;
            }

            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                errors.Add($"{prop.Name} empty or whitespace");
                continue;
            }
        }

        return errors;
    }
}
