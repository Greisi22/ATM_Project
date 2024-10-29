using System;
using System.Collections;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Entities;


public class User : BaseEntity
{
    public string Email { get; set; }

    public string Password { get; set; }

    public UserRole Role { get; set; }

    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
