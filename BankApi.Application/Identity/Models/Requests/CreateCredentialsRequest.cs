﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Identity.Models.Login;
public class CreateCredentialsRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Guid UserId { get; set; }
}
