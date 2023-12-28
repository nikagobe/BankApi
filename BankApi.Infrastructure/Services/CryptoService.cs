using BankApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Services;
public class CryptoService : ICryptoService
{
    private readonly CryptoSettings _config;


    public CryptoService(IOptions<CryptoSettings> config)
    {
        _config = config.Value;
    }

    public string GenerateHash(string password, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);
        using (var deriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, _config.Iterations))
        {
            var hashBytes = deriveBytes.GetBytes(_config.PasswordHashLength);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public string GenerateSalt()
    {
        var bytes = RandomNumberGenerator.GetBytes(_config.SaltLength);

        return Convert.ToBase64String(bytes);
    }



}
