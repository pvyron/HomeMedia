using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Contracts.Torrents;
using HomeMedia.ExternalApi;
using HomeMedia.ExternalApi.Torrents;
using HomeMedia.Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddHttpClient();
services.AddTorrents(builder.Configuration);
services.AddJsonOptions();
services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Transient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPost("api/torrents/search", async ([FromBody] TorrentsSearchRequestModel requestModel, [FromServices] ISender mediator, CancellationToken cancellationToken) => await mediator.Send(new SearchTorrentsQuery(requestModel), cancellationToken));

await app.RunAsync();