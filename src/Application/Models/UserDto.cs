using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Models;
public class UserDto : IMapFrom<User>
{
 

    [Required]
    public string Email { get; set; }

    [Required]
    public UserRole Role { get; set; }

    [Required]
    public string Password { get; set; }

   




    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
        profile.CreateMap<UserDto, User>();
    }

    public string CreatePasswordHash()
    {
        var passwordHasher = new PasswordHasher<User>();
        return passwordHasher.HashPassword(null, this.Password);
    }
}
