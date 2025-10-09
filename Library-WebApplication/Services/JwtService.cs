using Library_WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_WebApplication.Services
{
    public class JwtService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly Encryption _encryption;
        private readonly EmailValidation _emailValidation;
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;
        public  JwtService(AppDbContext dbContext, IConfiguration config, Encryption encryption, EmailValidation emailValidation)
        {
            _context = dbContext;
            _secret = config["Jwt:Key"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _expiryMinutes = int.Parse(config["Jwt:ExpireMinutes"]);
            _encryption = encryption;
            _emailValidation = emailValidation;
        }
        public async Task <string> GenerateToken(User user, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),  // User Id
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), // Username
                new Claim(ClaimTypes.Role, roleName) // Role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true, // Check expiration
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Ensure it's a JWT and signed with the right algorithm
                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {

                    string jbwName = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name").ToString();
                    Console.WriteLine(jbwName);


                    return true;
                }

                return false;
            }
            catch
            {
                return false; // Invalid, expired, or tampered token
            }
        }


    }
}
