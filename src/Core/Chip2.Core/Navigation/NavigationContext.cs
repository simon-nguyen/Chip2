using System;

namespace Chip2.Navigation;

public class NavigationContext
{
    public NavigationContext(Uri uri)
    {
        Uri = uri;
    }

    /// <summary>
    /// Gets the navigation URI.
    /// </summary>
    public Uri Uri { get; private set; }
}
