using BankApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Domain.Entities;
public class Transaction
{
    public long TransactionId { get; set; }
    public TransactionType Type { get; set; }
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
    public decimal Amount { get; set; }
}
