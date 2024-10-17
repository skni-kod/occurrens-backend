using MediatR;

namespace Application.Email.Commands.SendConfirmAccountEmail;

public sealed record SendConfirmAccountEmailCommand(Guid UserId) : IRequest;