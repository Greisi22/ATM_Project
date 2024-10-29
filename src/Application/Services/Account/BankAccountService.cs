using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services.Account;

public class BankAccountService : IBankAccountService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public  BankAccountService(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAccount(BankAccountDto bankAccount, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<BankAccount>(bankAccount);

        _applicationDbContext.EntitySet<BankAccount>().Add(result);


        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        return result.Id;
    }


    public async Task<List<BankAccountDto>> GetAll()
    {

        var bankAccounts = await _applicationDbContext.EntitySet<BankAccount>()
     .Select(b => new BankAccountDto
     {
         AccountType = b.AccountType,
         Currency = b.Currency,
         Frequency = b.Frequency,
         Balance = b.Balance,
         UserId = b.UserId
     }).ToListAsync();

        
        return bankAccounts;

    }

    public async Task<List<BankAccount>> GetAccountByUserID(Guid userId)
    {

        
        var userAccount = _applicationDbContext.EntitySet<BankAccount>().AnyAsync(account => account.UserId == userId);

        if(userAccount == null)
        {
            return new List<BankAccount>();
        }

        var accounts = await _applicationDbContext.EntitySet<BankAccount>()
    .Where(account => account.UserId == userId)
    .ToListAsync();
        return accounts;
    }
}
