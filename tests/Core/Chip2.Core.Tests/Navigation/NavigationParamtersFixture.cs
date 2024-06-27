using Chip2.Navigation;

namespace Chip2.Tests.Navigation;

public class NavigationParamtersFixture
{
    private const string _uri = "?id=123&name=simonn";

    [Fact]
    public void ParseParametersFromQuery()
    {
        var parameters = new NavigationParameters(_uri);
        Assert.Equal(2, parameters.Count);
        Assert.True(parameters.ContainsKey("id"));
        Assert.Equal("123", parameters["id"]);
        Assert.True(parameters.ContainsKey("name"));
        Assert.Equal("simonn", parameters["name"]);
    }
}
