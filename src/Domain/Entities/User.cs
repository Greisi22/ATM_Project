using System;

using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
public class User : BaseEntity
{
    public string Email { get; set; }

    public string Password { get; set; }

    public UserRole Role { get; set; }
	
}
