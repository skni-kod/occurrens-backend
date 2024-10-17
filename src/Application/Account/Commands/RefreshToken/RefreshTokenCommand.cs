using Domain.AuthTokens;
using MediatR;

namespace Application.Account.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string? RefreshToken) : IRequest<JsonWebToken>;