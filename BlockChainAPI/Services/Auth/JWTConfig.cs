﻿using BlockChain_DB.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlockChainAPI.Services.Auth
{
    public static class JWTConfig
    {

        public static IServiceCollection AddJWTConfig(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                Jwt jwt = _configuration.GetSection("Jwt").Get<Jwt>()!;
                var byte_key = Encoding.UTF8.GetBytes(jwt.Key);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(byte_key)

                };
            });

            return services;
        }
    }
}
