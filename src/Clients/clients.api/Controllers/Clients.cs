using AutoMapper;
using Clients.API.DTO;
using Clients.Application.Commands;
using Clients.Application.Queries;
using Clients.Domain;
using Clients.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Clients.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClientsController(ILogger<ClientsController> logger, IMapper mapper, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<ClientsController> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

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
            // In production, the tenant Id will be set from the logged in user context
            logger.LogWarning("Setting the tenant Id to the development tenant");
            draftClient.TenantId = Consts.DevelopmentTenant;

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
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto clientDto, CancellationToken cancellationToken)
        {
            if (id == default)
            {
                logger.LogError($"Invalid value of Id : {id}");
                return BadRequest(id);
            }

            clientDto.Id = id;
            var client = mapper.Map<Client>(clientDto);

            var updateClientCommand = new UpdateClientCommand(client);

            var updatedClient = await mediator.Send(updateClientCommand, cancellationToken).ConfigureAwait(false);

            return Ok(BuildResourceLocation(updatedClient.Id));
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
                logger.LogError($"Invalid value for {nameof(id)} : {id}");
                return BadRequest(id);
            }

            var client = await mediator.Send(new GetClientQuery(id), cancellationToken).ConfigureAwait(false);

            var clientDto = mapper.Map<ClientDto>(client);

            return Ok(clientDto);
        }

        [HttpPost("search")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromBody] ClientSearchDto clientSearchDto, CancellationToken cancellationToken)
        {
            if (clientSearchDto == null)
            {
                logger.LogError($"Invalid value for {nameof(clientSearchDto)}");
                return BadRequest($"The submitted search object is not valid or empty");
            }

            var clientSearchQuery = new SearchClientsQuery(clientSearchDto.FirstName, clientSearchDto.FamilyName, clientSearchDto.City);
            var clients = await mediator.Send(clientSearchQuery, cancellationToken).ConfigureAwait(false);
            var clientDtos = mapper.Map<IEnumerable<ClientDto>>(clients);
            
            return Ok(clientDtos);
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
                logger.LogError($"Invalid value for {nameof(id)} : {id}");
                return BadRequest(id);
            }

            var deleteClientCommand = new DeleteClientCommand(id);

            await mediator.Send(deleteClientCommand, cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet("{id}/canDelete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CanDelete(Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                logger.LogError($"Invalid value for {nameof(id)} : {id}");
                return BadRequest(id);
            }

            var canDeleteClientQuery = new CanDeleteClientQuery(id);

            var canDelete = await mediator.Send(canDeleteClientQuery, cancellationToken).ConfigureAwait(false);

            return Ok(canDelete);
        }

        protected string BuildResourceLocation<T>(T Id)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + $"/{Id}";
            return locationUrl;
        }
    }
}
