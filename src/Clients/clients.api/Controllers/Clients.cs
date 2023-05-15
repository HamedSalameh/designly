using AutoMapper;
using Clients.API.DTO;
using Clients.Application.Commands;
using Clients.Application.Queries;
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

            // TODO: Set the tenant Id from the logged in user context tenant

            var createClientCommand = new CreateClientCommand(draftClient);

            var clientId = await mediator.Send(createClientCommand, cancellationToken).ConfigureAwait(false);

            var clientResourceUrl = BuildResourceLocation(clientId);

            return Ok(clientResourceUrl);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateClient(Guid Id, [FromBody] ClientDto clientDto, CancellationToken cancellationToken)
        {
            if (Id == default)
            {
                logger.LogError("Invalid value of Id : ", Id);
                return BadRequest(Id);
            }

            clientDto.Id = Id;
            var client = mapper.Map<Client>(clientDto);

            var updateClientCommand = new UpdateClientCommand(client);

            var clientId = await mediator.Send(updateClientCommand, cancellationToken).ConfigureAwait(false);

            return Ok(BuildResourceLocation(clientId.Id));
        }

        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                logger.LogError("Invalid value for {nameof(id)} : {id}", id);
                return BadRequest(id);
            }

            var client = await mediator.Send(new GetClientQuery(id), cancellationToken).ConfigureAwait(false);

            var clientDto = mapper.Map<ClientDto>(client);

            return Ok(clientDto);
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                logger.LogError("Invalid value for {id} : {id}", nameof(id));
                return BadRequest(id);
            }

            var deleteClientCommand = new DeleteClientCommand(id);

            await mediator.Send(deleteClientCommand, cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        protected string BuildResourceLocation<T>(T Id)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"/{Id}";
            return locationUrl;
        }
    }
}
