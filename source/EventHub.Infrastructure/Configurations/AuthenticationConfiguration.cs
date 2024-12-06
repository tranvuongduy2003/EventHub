using System.Text;
using EventHub.Domain.Shared.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventHub.Infrastructure.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthetication(this IServiceCollection services)
    {
        JwtOptions jwtOptions = services.GetOptions<JwtOptions>("JwtOptions");
        byte[] key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        Authentication authentication = services.GetOptions<Authentication>("Authentication");
        ProviderOptions googleAuthentication = authentication.Google;
        ProviderOptions facebookAuthentication = authentication.Facebook;


        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.SaveToken = true;
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = googleAuthentication.ClientId;
                googleOptions.ClientSecret = googleAuthentication.ClientSecret;

                googleOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                googleOptions.Scope.Add("profile");
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                googleOptions.SaveTokens = true;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = facebookAuthentication.ClientId;
                facebookOptions.AppSecret = facebookAuthentication.ClientSecret;

                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                facebookOptions.SaveTokens = true;
            });

        return services;
    }
}
