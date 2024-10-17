using Domain.AuthTokens;
using MediatR;

namespace Application.Account.Commands.SignIn;

public sealed record SignInCommand(
    string Email,
    string Password) : IRequest<JsonWebToken>;