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


namespace CleanArchitecture.Application.Services.BankAccountAudit;
public class BankAccountAuditService : IBankAccountAuditService
{

    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public BankAccountAuditService(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<BankAccountAuditLogDto>> GetAuditBankAccountId(Guid Id)
    {
        var auditLogs = await _applicationDbContext.EntitySet<BankAccountAuditLog>()
     .Where(log => log.BankAccountId == Id)
     .ToListAsync();

       
        var auditLogDtos = auditLogs.Select(log => new BankAccountAuditLogDto
        {
            BankAccountId = log.BankAccountId,
            OldBalance = log.OldBalance,
            NewBalance = log.NewBalance,
            ChangeDate = log.ChangeDate,
            ChangedBy = log.ChangedBy
        }).ToList();

        return auditLogDtos;
    }

    public async Task<List<BankAccountAuditLogDto>> GetAuditUserId(string UserEmail)
    {

        var auditLogs = await _applicationDbContext.EntitySet<BankAccountAuditLog>()
         .Where(log => log.ChangedBy == UserEmail)
         .ToListAsync();


        var auditLogDtos = auditLogs.Select(log => new BankAccountAuditLogDto
        {
            BankAccountId = log.BankAccountId,
            OldBalance = log.OldBalance,
            NewBalance = log.NewBalance,
            ChangeDate = log.ChangeDate,
            ChangedBy = log.ChangedBy
        }).ToList();

        return auditLogDtos;
    }


    public async Task<List<BankAccountAuditLogDto>> GetAll()
    {
        var auditLogs = await _applicationDbContext.EntitySet<BankAccountAuditLog>()
         .ToListAsync();


        var auditLogDtos = auditLogs.Select(log => new BankAccountAuditLogDto
        {
            BankAccountId = log.BankAccountId,
            OldBalance = log.OldBalance,
            NewBalance = log.NewBalance,
            ChangeDate = log.ChangeDate,
            ChangedBy = log.ChangedBy
        }).ToList();

        return auditLogDtos;
    }
}

