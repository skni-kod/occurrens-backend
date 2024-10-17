using MediatR;

namespace Application.Account.Commands.SignOut;

public sealed record SignOutCommand(string RefreshToken) : IRequest;