using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services.BankAccountAudit;
public interface IBankAccountAuditService
{
    Task<List<BankAccountAuditLogDto>> GetAuditBankAccountId(Guid Id);
    Task<List<BankAccountAuditLogDto>> GetAuditUserId(string UserEmail);
    Task<List<BankAccountAuditLogDto>> GetAll();
}
