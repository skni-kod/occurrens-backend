using Application.Account.Responses;
using MediatR;

namespace Application.Account.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string Token,
    string Password,
    string RepeatPassword
    ) : IRequest<ResetPasswordResponse>;