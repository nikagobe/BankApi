using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Domain.Entities;
public class BankAccount
{
    public Guid BankAccountId { get; set; }
    public Guid UserId { get; set; }
    public decimal MoneyAmount { get; set; }

}
