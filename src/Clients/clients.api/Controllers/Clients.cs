using AutoMapper;
using Clients.API.DTO;
using Clients.Application.Commands;
using Clients.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;

namespace Clients.API.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> logger;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public ClientsController(ILogger<ClientsController> logger, IMapper mapper, IMediator mediator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto, CancellationToken cancellationToken)
        {
            if (clientDto == null)
            {
                logger.LogError($"Invalid value for {nameof(clientDto)}");
                return BadRequest($"The submitted client object is not valid or empty");
            }

            var draftClient = mapper.Map<Client>(clientDto);
            draftClient.Id = default;

            var createClientCommand = new CreateClientCommand(draftClient);

            var clientId = await mediator.Send(createClientCommand).ConfigureAwait(false);

            var clientResourceUrl = BuildResourceLocation(clientId);

            return Ok(clientResourceUrl);
        }

        protected string BuildResourceLocation<T>(T Id)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"/{Id}";
            return locationUrl;
        }
    }
}
