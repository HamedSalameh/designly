using Asp.Versioning;
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
    public class ClientsController(ILogger<ClientsController> logger, IMapper mapper, IMediator mediator, ITenantProvider tenantProvider) : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ITenantProvider _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));

        [HttpPost]
        [Authorize(Policy = IdentityData.AccountOwnerPolicyName)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto, CancellationToken cancellationToken)
        {
            if (clientDto == null)
            {
                _logger.LogError("Invalid value for {clientDto}", nameof(clientDto));
                return BadRequest($"The submitted client object is not valid or empty");
            }

            var tenantId = _tenantProvider.GetTenantId();
            if (Guid.Empty == tenantId)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var draftClient = _mapper.Map<Client>(clientDto);
            draftClient.TenantId = tenantId;

            var createClientCommand = new CreateClientCommand(draftClient);

            var clientId = await _mediator.Send(createClientCommand, cancellationToken).ConfigureAwait(false);

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
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid value for {id}", nameof(id));
                return BadRequest(id);
            }

            var tenantId = _tenantProvider.GetTenantId();
            if (Guid.Empty == tenantId)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            clientDto.Id = id;
            var client = _mapper.Map<Client>(clientDto);

            var updateClientCommand = new UpdateClientCommand(client);

            var updatedClient = await _mediator.Send(updateClientCommand, cancellationToken).ConfigureAwait(false);

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
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid value for {id}", nameof(id));
                return BadRequest(id);
            }

            var tenantId = _tenantProvider.GetTenantId();
            if ( Guid.Empty == tenantId)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var client = await _mediator.Send(new GetClientQuery(tenantId, id), cancellationToken).ConfigureAwait(false);

            var clientDto = _mapper.Map<ClientDto>(client);
            
            return Ok(clientDto);
        }

        [HttpGet("validate/{tenantId}/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Validate([FromRoute] Guid tenantId, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid value for {id}", nameof(id));
                return BadRequest(id);
            }
            if (tenantId == Guid.Empty)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest(tenantId);
            }

            // Try get user status from the database and respond with the status
            var clientStatus = await _mediator.Send(new GetClientStatusQuery(tenantId, id), cancellationToken).ConfigureAwait(false);

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
                _logger.LogError( "Invalid value for {clientSearchDto}", nameof(clientSearchDto));
                return BadRequest($"The submitted search object is not valid or empty");
            }

            var tenantId = _tenantProvider.GetTenantId();
            if (Guid.Empty == tenantId)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var clientSearchQuery = new SearchClientsQuery(tenantId, clientSearchDto.FirstName, clientSearchDto.FamilyName, clientSearchDto.City);
            var clients = await _mediator.Send(clientSearchQuery, cancellationToken).ConfigureAwait(false);
            var clientDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);
            
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
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid value for {id}", nameof(id));
                return BadRequest(id);
            }

            var tenantId = _tenantProvider.GetTenantId();
            if (Guid.Empty == tenantId)
            {
                _logger.LogError("Invalid value for {tenantId}", nameof(tenantId));
                return BadRequest($"The submitted tenant Id is not valid or empty");
            }

            var deleteClientCommand = new DeleteClientCommand(tenantId, id);

            await _mediator.Send(deleteClientCommand, cancellationToken).ConfigureAwait(false);

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
