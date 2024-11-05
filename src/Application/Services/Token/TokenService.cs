using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Enums;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly IApplicationDbContext _applicationDbContext;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger, IApplicationDbContext applicationDbContext)
        {
            _configuration = configuration;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public string GenerateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException("User email cannot be null or empty.");
            }

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("One or more JWT configuration values are missing.");
            }

            _logger.LogInformation("Generating token for user: {Email}", user.Email);
            _logger.LogInformation("User ID being added to token: {UserId}", user.Id);
            var role = user.Role.ToString();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            _logger.LogInformation("Token generated successfully for user: {Email}", user.Email);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       

       
    }
}
