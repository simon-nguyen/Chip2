using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chip2.Core;

namespace Chip2;
public abstract class ParametersBase
    : IParameters
{
    private readonly List<KeyValuePair<string, object?>> _entries = [];

    protected ParametersBase() { }

    protected ParametersBase(string query)
    {
        if (!string.IsNullOrEmpty(query))
        {
            ParseQuery(query.AsSpan());
        }
    }

    protected ParametersBase(ReadOnlySpan<char> query)
    {
        ParseQuery(query);
    }

    protected virtual void ParseQuery(ReadOnlySpan<char> query)
    {

    }

    public object? this[string key]
    {
        get
        {
            foreach (var entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                    return entry.Value;
            }

            return null;
        }
    }

    public int Count => _entries.Count;

    public IEnumerable<string> Keys
        => _entries.Select(e => e.Key);

    public void Add(string key, object value)
        => _entries.Add(new KeyValuePair<string, object?>(key, value));

    public bool ContainsKey(string key)
        => _entries.ContainsKey(key);

    public T? GetValue<T>(string key)
        => _entries.GetValue<T>(key);

    public IEnumerable<T> GetValues<T>(string key)
        => _entries.GetValues<T>(key);

    public bool TryGetValue<T>(string key, out T? value)
        => _entries.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        => _entries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
