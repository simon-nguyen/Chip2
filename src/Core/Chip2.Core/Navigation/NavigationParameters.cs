using System;

namespace Chip2.Navigation;

public class NavigationParameters
    : ParametersBase, INavigationParameters
{
    public NavigationParameters() { }
    public NavigationParameters(string query)
        : base(query) { }
    public NavigationParameters(ReadOnlySpan<char> query)
        : base(query) { }
}
