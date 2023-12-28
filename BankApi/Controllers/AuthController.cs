using BankApi.Application.Identity.Models.Login;
using BankApi.Application.Identity.Service;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var isValidCredentials = await _identityService.IsValidCredentials(request);

        if (!isValidCredentials)
        {
            return Unauthorized();
        }

        var token = await _identityService.GenerateToken(request);

        return Ok(token);
    }
}
