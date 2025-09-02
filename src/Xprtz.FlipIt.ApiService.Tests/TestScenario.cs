namespace Xprtz.FlipIt.ApiService.Tests;

public class TestScenario
{
    public Dictionary<string, CardTestData> CardTestData { get; init; } = new();
    public IEnumerable<string> ExpectedCards { get; init; } = new List<string>();
}
