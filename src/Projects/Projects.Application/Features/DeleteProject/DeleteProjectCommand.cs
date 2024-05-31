using LanguageExt.Common;
using MediatR;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectCommand : IRequest<Result<bool>>
    {
        public Guid TenantId { get; set; }
        public Guid ProjectId { get; set; }
    }
}