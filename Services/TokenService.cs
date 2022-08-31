using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationProject.DTOs;
using AuthenticationProject.Extensions;
using AuthenticationProject.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationProject.Services;

public class TokenService
{
    public static string GenerateToken(User user, IList<string> userRoles)
    {
        var key = Encoding.ASCII.GetBytes(Settings.Secret);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Name, user.FirsName),
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityToken = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
