using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace HomeMedia.WindowsService.Controllers;

public abstract class HomeMediaController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}
