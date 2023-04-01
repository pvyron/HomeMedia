using HomeMedia.WindowsService.Queries.Torrents;
using Microsoft.AspNetCore.Mvc;

namespace HomeMedia.WindowsService.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class TorrentsController : HomeMediaController
{
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchTorrentsQuery(query), cancellationToken);

        return result.Match<IActionResult>(
            Ok,
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }
}
