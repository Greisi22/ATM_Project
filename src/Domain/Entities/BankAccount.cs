using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
namespace CleanArchitecture.Domain.Entities;

  public class BankAccount : BaseEntity
    {

        public AccountType AccountType { get; set; }

        public string Currency { get; set; }

        public AccountFrequency Frequency { get; set; }

        public double Balance { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

    public ICollection<BankAccountAuditLog> BankAccountAuditLogs { get; set; } = new List<BankAccountAuditLog>();
}
