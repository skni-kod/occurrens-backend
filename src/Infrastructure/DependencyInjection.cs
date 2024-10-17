using Application.Account.Commands.SignIn;
using Application.Persistance.Interfaces.Account;
using Application.Persistance.Interfaces.CurrentUser;
using Application.Persistance.Interfaces.Email;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Persistance.Accounts.Services;
using Infrastructure.Persistance.CurrentUserService.Services;
using Infrastructure.Persistance.Email.Config;
using Infrastructure.Persistance.Email.Services;
using Infrastructure.Persistance.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.DatabaseConfiguration(configuration);
        services.AuthorizationSettings(configuration);

        services.AddScoped<SignInManager<Account>>();
        services.AddScoped<UserManager<Account>>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.Configure<IdentityOptions>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequiredUniqueChars = 6;

            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(5);

            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = false;
        });

        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssemblyContaining<SignInCommand>();

        var smtpConfig = new SmtpConfig();
        configuration.GetSection("SMTP").Bind(smtpConfig);
        services.AddSingleton(smtpConfig);
        
            
        return services;
    }
}