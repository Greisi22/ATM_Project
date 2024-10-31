using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Application.Services.Account;

public class BankAccountService : IBankAccountService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public  BankAccountService(IApplicationDbContext applicationDbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;

    }

    public async Task<Guid> CreateAccount(BankAccountDto bankAccount, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<BankAccount>(bankAccount);

        _applicationDbContext.EntitySet<BankAccount>().Add(result);


        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        return result.Id;
    }



    public async Task<BankAccountDto> GetById(Guid id)
    {
        var bankAccount = await _applicationDbContext.EntitySet<BankAccount>()
                                                       .Where(x => x.Id == id)
                                                       .FirstOrDefaultAsync();

        var result = _mapper.Map<BankAccountDto>(bankAccount);
        
        return result;

    }
    
    public async Task<List<BankAccountDto>> GetAll()
    {

        var bankAccounts = await _applicationDbContext.EntitySet<BankAccount>()
                                                       .ToListAsync();

        var result = _mapper.Map<List<BankAccountDto>>(bankAccounts);
        
        return result;

    }

    
   public async Task<List<BankAccountDto>> GetBankAccounts()
    {

        var currentUserId = _currentUserService.UserId;

        var bankAccounts = await _applicationDbContext.EntitySet<BankAccount>()
                                                       .ToListAsync();

        var userBankAccounts = new List<BankAccount>();

        foreach ( var bankAccount in bankAccounts )
        {
            if ( bankAccount.UserId == currentUserId)
            {
                userBankAccounts.Add( bankAccount );
            }

        }

        if(userBankAccounts == null)
        {
            throw new Exception("Create a new account");
        }

        var result = _mapper.Map<List<BankAccountDto>>(userBankAccounts);

        return result;
    }

    public async Task<List<BankAccountDto>> GetAccountsByUserID(Guid userId) 
    {
        var userAccount = await _applicationDbContext.EntitySet<BankAccount>()
                                               .Where(account => account.UserId == userId)
                                               .ToListAsync();

        if(userAccount == null)
        {
            return new List<BankAccountDto>();
        }

        var result = _mapper.Map<List<BankAccountDto>>(userAccount);
        return result;
    }

    public async Task<bool> WithdrawBalance(double withdrawAmount, Guid id, CancellationToken cancellationToken)
    {
        var bankAccount = await _applicationDbContext.EntitySet<BankAccount>()
                                                       .Where(x => x.Id == id)
                                                       .FirstOrDefaultAsync();

        if(bankAccount == null)
        {
            throw new NotFoundException(nameof(BankAccount), id);
        }

        var currentBalance = bankAccount.Balance;

        if (currentBalance == 0 || currentBalance < withdrawAmount)
        {
            throw new BadRequestException("Current balance is not sufficient for the required amount ");
        }

        bankAccount.Balance = currentBalance - withdrawAmount;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return true;

    }


    public async Task<bool> DepositBalance(double depositAmount, Guid id, CancellationToken cancellationToken)
    {
        var bankAccount = await _applicationDbContext.EntitySet<BankAccount>()
                                                       .Where(x => x.Id == id)
                                                       .FirstOrDefaultAsync();

        if (bankAccount == null)
        {
            throw new NotFoundException(nameof(BankAccount), id);
        }

        var currentBalance = bankAccount.Balance;

        if (depositAmount <= 0 )
        {
            throw new BadRequestException("The amount you want to deposit, should be greater than 0.");
        }

        bankAccount.Balance = currentBalance + depositAmount;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return true;

    }
}
