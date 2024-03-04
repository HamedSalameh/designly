using MediatR;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommand : IRequest
    {
        public Guid TenantId { get; set; }
        public Guid ProjectId { get; set; }
    }
}