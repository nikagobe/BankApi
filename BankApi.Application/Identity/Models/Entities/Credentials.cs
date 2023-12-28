using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Identity.Models.Entities;
public class Credentials
{
    public Guid CredentialsId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public DateTime CreatedDate { get; set; }
}
