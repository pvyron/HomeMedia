namespace HomeMedia.ExternalApi.Torrents;

public sealed class SearchFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;
    private readonly string _searchKey;

    public SearchFilter(IConfiguration configuration)
    {
        _configuration = configuration;
        _searchKey = _configuration.GetRequiredSection("ApiKeys:Search").Value!;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey))
        {
            return Results.BadRequest("Missing ApiKey header");
        }

        if (apiKey != _searchKey)
        {
            return Results.Unauthorized();
        }

        return await next.Invoke(context);
    }
}

public sealed class DownloadFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;
    private readonly string _downloadKey;

    public DownloadFilter(IConfiguration configuration)
    {
        _configuration = configuration;
        _downloadKey = _configuration.GetRequiredSection("ApiKeys:Download").Value!;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey))
        {
            return Results.BadRequest("Missing ApiKey header");
        }

        if (apiKey != _downloadKey)
        {
            return Results.Unauthorized();
        }

        return await next.Invoke(context);
    }
}