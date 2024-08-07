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
                _logger.LogDebug("Handling request {ValidateUserCommandHandler} for {UserId}", nameof(ValidateUserCommandHandler), request.UserId);
            }

            try
            {
                var userStatus = await unitOfWork.UsersRepository.GetUserStatusAsync(request.UserId, request.TenantId, cancellationToken).ConfigureAwait(false);

                if (userStatus is null)
                {
                    var userValidationException = new AccountException(AccountErrors.UserNotFound.Description, AccountErrors.UserNotFound);
                    return new Result<bool>(userValidationException);
                }

                Result<bool> validationResult = userStatus switch
                {
                    Consts.UserStatus.BeforeActivation => new Result<bool>(new AccountException(AccountErrors.UserIsNotActivated.Description, AccountErrors.UserIsNotActivated)),
                    Consts.UserStatus.Active => new Result<bool>(true),
                    Consts.UserStatus.Suspended => new Result<bool>(new AccountException(AccountErrors.UserIsSuspended.Description, AccountErrors.UserIsSuspended)),
                    Consts.UserStatus.Disabled => new Result<bool>(new AccountException(AccountErrors.UserIsDisabled.Description, AccountErrors.UserIsDisabled)),
                    Consts.UserStatus.MarkedForDeletion => new Result<bool>(new AccountException(AccountErrors.UserIsMarkedForDeletion.Description, AccountErrors.UserIsMarkedForDeletion)),
                    Consts.UserStatus.Deleted => new Result<bool>(new AccountException(AccountErrors.UserIsDeleted.Description, AccountErrors.UserIsDeleted)),
                    Consts.UserStatus.Blacklisted => new Result<bool>(new AccountException(AccountErrors.UserIsBlacklisted.Description, AccountErrors.UserIsBlacklisted)),
                    _ => throw new AccountException(AccountErrors.UnsupportedUserStatus.Description, AccountErrors.UnsupportedUserStatus)
                };

                return validationResult;
            }
            catch (AccountException accountException)
            {
                return new Result<bool>(accountException);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not validate user due to error: {Message}", exception.Message);
                return new Result<bool>(exception);
            }
        }
    }
}
