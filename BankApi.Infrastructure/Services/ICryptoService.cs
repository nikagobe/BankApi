﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Services;
public interface ICryptoService
{
    string GenerateSalt();
    string GenerateHash(string password, string salt);
}
