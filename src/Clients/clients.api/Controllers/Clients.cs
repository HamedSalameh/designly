using AutoMapper;
using Clients.API.DTO;
using Clients.Application.Commands;
using Clients.Application.Queries;
using Clients.Domain.Entities;
using Designly.Auth.Identity;
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
    public class ClientsController(ILogger<ClientsController> logger, IMapper mapper, IMediator mediator, IAuthorizationProvider authorizationProvider) : ControllerBase
    {
        private readonly ILogger<ClientsController> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly IAuthorizationProvider authroizationProvider = authorizationProvider ?? throw new ArgumentNullException(nameof(authroizationProvider));

        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
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

            var tenantId = authroizationProvider.GetTenantId(HttpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var draftClient = mapper.Map<Client>(clientDto);
            draftClient.TenantId = tenantId.Value;

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

            var tenantId = authroizationProvider.GetTenantId(HttpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return BadRequest($"The submitted tenant Id is not valid or empty");
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

            var tenantId = authroizationProvider.GetTenantId(HttpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var client = await mediator.Send(new GetClientQuery(tenantId.Value, id), cancellationToken).ConfigureAwait(false);

            var clientDto = mapper.Map<ClientDto>(client);
            
            return Ok(clientDto);
        }

        [HttpGet("status/{tenantId}/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus([FromRoute] Guid tenantId, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == default || id == Guid.Empty)
            {
                logger.LogError($"Invalid value for {nameof(id)} : {id}");
                return BadRequest(id);
            }
            if (tenantId == default || tenantId == Guid.Empty)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)} : {tenantId}");
                return BadRequest(tenantId);
            }

            // Try get user status from the database and respond with the status
            var clientStatus = await mediator.Send(new GetClientStatusQuery(tenantId, id), cancellationToken).ConfigureAwait(false);

            return Ok(clientStatus);
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

            var tenantId = authroizationProvider.GetTenantId(HttpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var clientSearchQuery = new SearchClientsQuery(tenantId.Value, clientSearchDto.FirstName, clientSearchDto.FamilyName, clientSearchDto.City);
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

            var tenantId = authroizationProvider.GetTenantId(HttpContext.User);
            if (tenantId is null || Guid.Empty == tenantId)
            {
                logger.LogError($"Invalid value for {nameof(tenantId)}");
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var deleteClientCommand = new DeleteClientCommand(tenantId.Value, id);

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
