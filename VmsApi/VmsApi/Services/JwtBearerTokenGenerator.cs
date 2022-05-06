using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VmsApi.Data.Models;
using VmsApi.ViewModels.GetModels;

namespace VmsApi.Services
{
    [ExcludeFromCodeCoverage]
    public class JwtBearerTokenGenerator : ITokenGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationSection _jwtSettings;

        public JwtBearerTokenGenerator(UserManager<User> userManager, IConfiguration jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.GetSection("JwtSettings");
        }

        public async Task<LoginJwtTokenDto> GetTokenAsync(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, await claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new LoginJwtTokenDto(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("secretKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("firstname", user.FirstName),
                new Claim("lastname", user.LastName),
                new Claim("username", user.Email),
                new Claim("userref", user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            System.Diagnostics.Debug.Write($"{user.Email} has roles:");
            foreach (var role in roles)
            {
                System.Diagnostics.Debug.Write($"[{role}]");
            }
        
        
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            return claims;
        }
    }
}