using BankApi.Application.Identity.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Identity.Service;
public interface IIdentityService
{
    public Task CreateCredentials(CreateCredentialsRequest request, CancellationToken cancellationToken);
    Task<string> GenerateToken(LoginRequest request);
    Task<Guid> GetUserIdFromToken(string token);
    public Task<bool> IsValidCredentials(LoginRequest request);
}
