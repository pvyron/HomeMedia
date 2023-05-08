using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HomeMedia.Contracts.Torrents;
using HomeMedia.ExternalApi;
using HomeMedia.ExternalApi.Torrents;
using HomeMedia.Infrastructure;
using LanguageExt;
using Mediator;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultEndPoint = Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT") ?? throw new ValueIsNullException($"Invalid configuration, missing value for KEYVAULT_ENDPOINT");
    var secretClient = new SecretClient(new(keyVaultEndPoint), new DefaultAzureCredential());

    builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
}

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;
services.AddHttpClient();
services.AddTorrents(builder.Configuration);
services.AddJsonOptions();
services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Transient;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

var errors = new List<string>() { "I am working" };

app.MapGet("/", () => Results.Ok(errors));

app.MapGet("/privacy", () => Results.Text(PrivacyPolicy.POLICY, "text/html"));

app.MapPost("api/torrents/search", async ([FromBody] TorrentsSearchRequestModel requestModel, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
{
    try
    {
        return await mediator.Send(new SearchTorrentsQuery(requestModel), cancellationToken);
    }
    catch (Exception ex)
    {
        errors.Add($"[{requestModel.Name}]{Environment.NewLine}{ex.Message} - {ex.StackTrace}");
        return Results.StatusCode(400);
    }
}).AddEndpointFilter<SearchFilter>();

app.MapPost("api/torrents/download", async ([FromBody] TorrentDownloadRequestModel requestModel, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
{
    try
    {
        return await mediator.Send(new DownloadTorrentCommand(requestModel.MagnetLink, requestModel.SaveLocation), cancellationToken);
    }
    catch (Exception ex)
    {
        errors.Add($"{ex.Message} - {ex.StackTrace}");
        return Results.StatusCode(400);
    }
}).AddEndpointFilter<DownloadFilter>();

await app.RunAsync();