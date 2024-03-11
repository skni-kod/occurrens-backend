using Application.Contracts.AccountAnswer;
using Application.WorkAccount.Commands.RegisterUser;
using Application.WorkAccount.Commands.ResetPassword;
using Application.WorkAccount.Commands.SignInUser;
using Application.WorkAccount.Queries.ConfirmAccount;
using Application.WorkAccount.Queries.ForgotPassword;
using Application.WorkAccount.Queries.GenerateTokenToResetPassword;
using Core.Account.enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Account;

[Route("account")]
public class AccountController : ApiController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
        
    }

    /// <summary>
    /// Create user 
    /// </summary>
    [HttpPost("user-register")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command);

        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
            );
        
    }
    
    /// <summary>
    /// Create user account
    /// </summary>
    /// <param name="request"></param>
    /// <param name="who"></param>
    /// <returns></returns>
    [HttpPost("sign-in/{who}")]
    public async Task<IActionResult> SignIn([FromBody] SignInUserRequest request, [FromRoute]UserRoles who)
    {
        var command = new SignInUserCommand(
            request.Login,
            request.Password,
            who);

        var response = await _mediator.Send(command);

        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
            );
        
    }
    
    /// <summary>
    /// Send e-mail to access reset password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost("forgot-password/{role}")]
    public async Task<IActionResult> ForgotPassword([FromBody] string email, [FromRoute] UserRoles role)
    {
        var query = new ForgotPasswordQuery(email, role);

        var response = await _mediator.Send(query);

        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
        );
        
    }
    
    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var response = await _mediator.Send(command);
        
        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
        );
        
    }

    /// <summary>
    /// Verification account on email
    /// </summary>
    /// <param name="token"></param>
    /// <param name="role"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("verificate-account/{token}/{role}/{id}")]
    public async Task<IActionResult> ConfirmAccount([FromRoute] string token, [FromRoute] UserRoles role,
        [FromRoute] Guid id)
    {
        var query = new ConfirmAccountQuery(token,role,id);

        var response = await _mediator.Send(query);

        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
            );
        
    }

    /// <summary>
    /// Generate reset password token after click email link
    /// </summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet("generate-token-to-reset-password/{id}/{role}")]
    public async Task<IActionResult> GenerateTokenToResetPassword([FromRoute] Guid id, [FromRoute] string role)
    {
        var query = new GenerateTokenToResetPasswordCommand(id, role);

        var response = await _mediator.Send(query);
        
        return response.Match(
            accountResponse => Ok(accountResponse),
            errors => Problem(errors)
        );
        
    }
}