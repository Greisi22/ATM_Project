using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;
public class BankAccountAuditLog: BaseEntity
{
    public Guid BankAccountId { get; set; }
    public double OldBalance { get; set; } 
    public double NewBalance { get; set; } 
    public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

    public string ChangedBy { get; set; } 
    public BankAccount BankAccount { get; set; }
}
