using System.Net.Mime;
using Flow.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace clients.api.Controllers;

[ApiController]
[Route("[controller]")]
public class Clients : ControllerBase
{
    private readonly IIdentityService identityService;

    public Clients(IIdentityService identityService)
    {
        this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login()
    {
        var username = "hamedsalami@gmail.com";
        var password = "p@55w0rd!";

        var response = await identityService.LoginAsync(username, password);

        return Ok(response);
    }
}