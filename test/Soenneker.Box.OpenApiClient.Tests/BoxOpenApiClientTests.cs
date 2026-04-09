using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Box.OpenApiClient.Tests;

[Collection("Collection")]
public sealed class BoxOpenApiClientTests : FixturedUnitTest
{
    public BoxOpenApiClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
