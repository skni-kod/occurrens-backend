using Application.Account.Enums;
using Application.Account.Responses;
using Domain.Roles;
using MediatR;

namespace Application.Account.Commands.SignUp;

public sealed record SignUpCommand(
    string Name,
    string SecondName,
    string Surname,
    string Pesel,
    string PhoneNumber,
    DateOnly BirthDate,
    string Email,
    string Password,
    string RepeatPassword,
    EnumRole Role
    ) : IRequest<SignUpResponse>;