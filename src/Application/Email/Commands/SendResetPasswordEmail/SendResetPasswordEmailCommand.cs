using Application.Email.Responses;
using MediatR;

namespace Application.Email.Commands.SendResetPasswordEmail;

public sealed record SendResetPasswordEmailCommand(
    string Email
    ) : IRequest<SendResetPasswordEmailResponse>;