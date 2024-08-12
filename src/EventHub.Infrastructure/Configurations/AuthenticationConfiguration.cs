using System.Text;
using EventHub.Shared.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventHub.Infrastructor.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthetication(this IServiceCollection services)
    {
        var jwtOptions = services.GetOptions<JwtOptions>("JwtOptions");
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        var authentication = services.GetOptions<Authentication>("Authentication");
        var googleAuthentication = authentication.Google;
        var facebookAuthentication = authentication.Facebook;


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