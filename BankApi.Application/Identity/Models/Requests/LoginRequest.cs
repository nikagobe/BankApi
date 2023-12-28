using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Identity.Models.Login;
public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
