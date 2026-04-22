using Soenneker.Tests.HostedUnit;

namespace Soenneker.Box.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class BoxOpenApiClientTests : HostedUnitTest
{
    public BoxOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
