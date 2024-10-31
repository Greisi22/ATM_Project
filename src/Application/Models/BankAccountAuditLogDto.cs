using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Models;
public class BankAccountAuditLogDto : IMapFrom<BankAccountAuditLog>
{
    public Guid BankAccountId { get; set; }
    public double OldBalance { get; set; }
    public double NewBalance { get; set; }
    public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

    public Guid ChangedBy { get; set; }
   
}
