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
    Task<BankAccountDto> GetById(Guid id);
    Task<List<BankAccountDto>> GetAll();
    Task<List<BankAccountDto>> GetBankAccounts();
    Task<List<BankAccountDto>> GetAccountsByUserID(Guid userId);
    Task<bool> WithdrawBalance(double withdrawAmount, Guid id, CancellationToken cancellationToken);
    Task<bool> DepositBalance(double depositAmount, Guid id, CancellationToken cancellationToken);
}
