using Application.Account.Responses;
using MediatR;

namespace Application.Account.Commands.ConfirmAccount;

public sealed record ConfirmAccountCommand(
    Guid UserId,
    string Token
    ) : IRequest<ConfirmAccountResponse>;