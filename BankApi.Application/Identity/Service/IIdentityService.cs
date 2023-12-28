using BankApi.Application.Identity.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Identity.Service;
public interface IIdentityService
{
    Task CreateCredentials(CreateCredentialsRequest request, CancellationToken cancellationToken);
    Task<string> GenerateToken(LoginRequest request);
    Task<Guid> GetUserIdFromToken(string token);
    Task<bool> IsValidCredentials(LoginRequest request);
}
