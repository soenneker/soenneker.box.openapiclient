[![](https://img.shields.io/nuget/v/soenneker.box.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.box.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.box.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.box.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.box.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.box.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.box.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.box.openapiclient/actions/workflows/codeql.yml)

# Soenneker.Box.OpenApiClient

A Kiota-generated .NET client for the Box API, built from Box's OpenAPI description.

## Installation

```bash
dotnet add package Soenneker.Box.OpenApiClient
```

## Recommended setup

For dependency injection, token configuration, and client reuse, install the companion utility:

```bash
dotnet add package Soenneker.Box.OpenApiClientUtil
```

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Box.OpenApiClientUtil.Registrars;

services.AddBoxOpenApiClientUtilAsSingleton();
```

Configure the Box access token under `Box:ApiKey`, then inject `IBoxOpenApiClientUtil` and call `Get` to obtain the generated client.

## Direct construction

Use Kiota's request adapter when constructing the generated client yourself:

```csharp
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Box.OpenApiClient;
using Soenneker.Box.OpenApiClient.Models;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var authentication = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(
    authentication,
    httpClient: httpClient);

var box = new BoxOpenApiClient(adapter);

UserFull? currentUser = await box.Users.Me.GetAsync(
    request =>
    {
        request.QueryParameters.Fields = ["id", "name", "login"];
    },
    cancellationToken);
```

`AnonymousAuthenticationProvider` is appropriate in this example because the bearer header is already applied to the dedicated `HttpClient`. Do not use an unauthenticated shared client that can send its default authorization header to unrelated hosts.

## Navigating the API

The client mirrors the Box API path hierarchy through request builders:

- `box.Users.Me` targets `/users/me`.
- `box.Files[fileId]` targets a file by ID.
- `box.Folders[folderId]` targets a folder by ID.
- `box.Search` exposes search operations.
- `box.Events`, `box.Webhooks`, and the other root properties expose their corresponding Box resources.

Endpoint methods accept a request-configuration callback for query parameters, headers, and middleware options, followed by a cancellation token.

## Practical notes

- Keep the `HttpClient`, request adapter, and generated client long-lived. The companion utility manages that lifecycle for dependency-injection applications.
- Response models and endpoint return values may be nullable when the schema allows an empty response. Check the result before dereferencing it.
- Some endpoints return streams. Dispose those streams after consuming them.
- Kiota surfaces service failures through generated error models or Kiota exceptions, depending on the endpoint's schema mapping.
- Public type and property names follow the source OpenAPI description. Regeneration can add, rename, or remove generated members when that description changes.
- Files under `src/Soenneker.Box.OpenApiClient` are generated. Put application-specific behavior in a separate project rather than editing generated request builders or models.
- Treat Box access tokens as credentials and never include them in logs, exception messages, or source control.
