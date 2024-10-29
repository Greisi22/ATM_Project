using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Services.Account;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Models;
public class BankAccountDto : IMapFrom<BankAccount>
{

    [Required]
    public AccountType AccountType { get; set; }

    [Required]
    public string Currency { get; set; }

    [Required]
    public AccountFrequency Frequency { get; set; }

    public double Balance { get; set; }

    [Required]
    public Guid UserId { get; set; }
  


    public void Mapping(Profile profile)
    {
        profile.CreateMap<BankAccount, BankAccountDto>();
        profile.CreateMap<BankAccountDto, BankAccount>();
     
    }
}
