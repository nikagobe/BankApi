using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Configuration;
public class CryptoSettings
{
    public int Iterations { get; set; }
    public int SaltLength { get; set; }
    public int PasswordHashLength { get; set; }
}
