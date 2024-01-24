﻿using Accounts.Domain;
using Accounts.Infrastructure.Interfaces;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Features.ValidateUser
{
    public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, Result<bool>>
    {
        private readonly ILogger<ValidateUserCommandHandler> _logger;
        private readonly IUnitOfWork unitOfWork;

        public ValidateUserCommandHandler(ILogger<ValidateUserCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<bool>> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Validating user Id {request.userId} in tenant {request.userId}");
            }

            var user = await unitOfWork.UsersRepository.GetUserByIdAsync(request.userId, cancellationToken).ConfigureAwait(false);

            if (user is null)
            {
                var userValidationException = new AccountException(AccountErrors.UserNotFound.Description, AccountErrors.UserNotFound);
                return new Result<bool>(userValidationException);
            }

            Result<bool> validationResult = user.Status switch
            {
                Consts.UserStatus.BeforeActivation => new Result<bool>(new AccountException(AccountErrors.UserIsNotActivated)),
                Consts.UserStatus.Active => new Result<bool>(true),
                Consts.UserStatus.Suspended => new Result<bool>(new AccountException(AccountErrors.UserIsSuspended)),
                Consts.UserStatus.Disabled => new Result<bool>(new AccountException(AccountErrors.UserIsDisabled)),
                Consts.UserStatus.MarkedForDeletion => new Result<bool>(new AccountException(AccountErrors.UserIsMarkedForDeletion)),
                Consts.UserStatus.Deleted => new Result<bool>(new AccountException(AccountErrors.UserIsDeleted)),
                Consts.UserStatus.Blacklisted => new Result<bool>(new AccountException(AccountErrors.UserIsBlacklisted)),
                _ => throw new AccountException(AccountErrors.UnsupportedUserStatus.Description, AccountErrors.UnsupportedUserStatus)
            };

            return validationResult;
        }
    }
}
