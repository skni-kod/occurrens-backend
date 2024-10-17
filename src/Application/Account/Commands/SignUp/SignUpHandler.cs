using Application.Account.Responses;
using Application.Email.Commands.SendConfirmAccountEmail;
using Application.Persistance.Interfaces.Account;
using MediatR;


namespace Application.Account.Commands.SignUp;

public sealed class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IAccountService _accountService;
    private readonly IMediator _mediator;

    public SignUpHandler(IAccountService accountService, IMediator mediator)
    {
        _accountService = accountService;
        _mediator = mediator;
    }
    
    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.Account
        {
            Name = request.Name,
            UserName = request.Email,
            SecondName = request.SecondName,
            Surname = request.Surname,
            Pesel = request.Pesel,
            PhoneNumber = request.PhoneNumber,
            BirthDate = request.BirthDate,
            Email = request.Email
        };

        var userId = await _accountService.CreateUserAsync(user, request.Password, request.Role, cancellationToken);

        await _mediator.Send(new SendConfirmAccountEmailCommand(userId), cancellationToken);

        return new SignUpResponse("Success!");
    }
}