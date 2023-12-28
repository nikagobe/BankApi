using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Infrastructure.Configuration;
public class JwtSettings
{
    public string Key { get; set; }
    public int ValidForMinutes { get; set; }
}
