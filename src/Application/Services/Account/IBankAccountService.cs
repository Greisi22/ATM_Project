using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Services.Account;
public interface IBankAccountService 
{
    Task<Guid> CreateAccount(BankAccountDto bankAccount, CancellationToken cancellationToken);
    Task<List<BankAccountDto>> GetAll();

    Task<List<BankAccountDto>> GetAccountByUserID(Guid userId);
}
