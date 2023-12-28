using BankApi.Application.Common;
using BankApi.Application.Identity.Models.Entities;
using BankApi.Application.Identity.Models.Login;
using BankApi.Application.Identity.Service;
using BankApi.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Services;
public class IdentityService : IIdentityService
{
    private readonly UnitOfWork     _unitOfWork;
    private readonly ICryptoService _cryptoService;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(UnitOfWork unitOfWork,
                           ICryptoService cryptoService,
                           IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _cryptoService = cryptoService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task CreateCredentials(CreateCredentialsRequest request,
                                        CancellationToken cancellationToken)
    {
        var salt = _cryptoService.GenerateSalt();
        var passwordHash = _cryptoService.GenerateHash(request.Password, salt);

        var credentials = new Credentials()
        {
            CredentialsId = Guid.NewGuid(),
            UserId = request.UserId,
            UserName = request.UserName,
            Salt = salt,
            PasswordHash = passwordHash,
            CreatedDate = DateTime.UtcNow
        };

        _unitOfWork.DbContext.Credentials.Add(credentials);

    }

    public async Task<bool> IsValidCredentials(LoginRequest request)
    {
        var userCredentials = _unitOfWork.DbContext.Credentials
            .Where(x => x.UserName == request.UserName).FirstOrDefault();
        
        if (userCredentials is null) return false;

        var loginPasswordHash = _cryptoService.GenerateHash(request.Password, userCredentials.Salt);

        return loginPasswordHash == userCredentials.PasswordHash;
    }

    public async Task<Guid> GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key))
        };

        var principal = handler.ValidateToken(token, validationParameters, out var securityToken);

        var userIdClaim = principal.FindFirst("UserId")?.Value;

        return Guid.Parse(userIdClaim);
    }
    public async Task<string> GenerateToken(LoginRequest request)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userInfo = await _unitOfWork.DbContext.Credentials.Where(x => x.UserName == request.UserName).FirstAsync();

        var claims = new[]
        {
            new Claim("UserId", userInfo.UserId.ToString())
        };

        var Sectoken = new JwtSecurityToken(
          expires: DateTime.Now.AddMinutes(_jwtSettings.ValidForMinutes),
          claims: claims,
          signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

        return token;
    }
}
