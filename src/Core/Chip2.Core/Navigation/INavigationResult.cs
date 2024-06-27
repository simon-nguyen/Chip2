using System;

namespace Chip2.Navigation;

public interface INavigationResult
{
    bool Success { get; }
    bool Canceled { get; }

    Exception? Error { get; }

    NavigationContext? Context { get; }
}
