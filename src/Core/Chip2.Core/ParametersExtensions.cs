using System;
using System.Collections.Generic;
using System.Linq;

namespace Chip2;

public static class ParametersExtensions
{
    public static bool ContainsKey(this IEnumerable<KeyValuePair<string, object?>> parameters, string key)
        => parameters.Any(p => string.Compare(p.Key, key, StringComparison.Ordinal) == 0);

    public static T? GetValue<T>(this IEnumerable<KeyValuePair<string, object?>> parameters, string key)
        => (T?)GetValue(parameters, key, typeof(T));

    public static object? GetValue(this IEnumerable<KeyValuePair<string, object?>> parameters, string key, Type type)
    {
        foreach (var kvp in parameters)
        {
            if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
            {
                if (TryGetValueInternal(kvp, type, out var value))
                    return value;

                throw new InvalidCastException($"Unable to convert the value of Type '{kvp.Value?.GetType().FullName}' to '{type.FullName}' for the key '{key}' ");
            }
        }

        return GetDefault(type);
    }

    public static IEnumerable<T> GetValues<T>(this IEnumerable<KeyValuePair<string, object?>> parameters, string key)
    {
        List<T> values = [];
        var type = typeof(T);

        foreach (var kvp in parameters)
        {
            if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0 &&
                TryGetValueInternal(kvp, type, out var value) &&
                value is T valueAsT)
            {
                values.Add(valueAsT);
            }
        }

        return values.ToArray();
    }

    public static bool TryGetValue<T>(
        this IEnumerable<KeyValuePair<string, object?>> parameters
        , string key
        , out T? value)
    {
        var type = typeof(T);

        foreach (var kvp in parameters)
        {
            if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
            {
                var success = TryGetValueInternal(kvp, type, out var valueAsObject);
                if (valueAsObject is T valueAsT)
                {
                    value = valueAsT;
                    return success;
                }
            }
        }

        value = default;
        return false;
    }

    private static bool TryGetValueInternal(KeyValuePair<string, object?> kvp, Type type, out object? value)
    {
        value = GetDefault(type);

        var valueAsString = kvp.Value is string str ? str : kvp.Value?.ToString();

        var success = false;
        if (kvp.Value is null)
        {
            success = true;
        }
        else if (kvp.Value.GetType() == type)
        {
            success = true;
            value = kvp.Value;
        }
        else if (type.IsAssignableFrom(kvp.Value.GetType()))
        {
            success = true;
            value = kvp.Value;
        }
        else if (type.IsEnum && !string.IsNullOrEmpty(valueAsString))
        {
            if (Enum.IsDefined(type, valueAsString))
            {
                success = true;
                value = Enum.Parse(type, valueAsString);
            }
            else if (int.TryParse(valueAsString, out var numericValue))
            {
                success = true;
                value = Enum.ToObject(type, numericValue);
            }
        }

        if (!success && type.GetInterface("System.IConvertible") != null)
        {
            success = true;
            value = Convert.ChangeType(kvp.Value, type);
        }

        return success;
    }

    private static object? GetDefault(Type type)
        => type.IsValueType ? Activator.CreateInstance(type) : null;
}
