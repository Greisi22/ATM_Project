using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;

namespace CleanArchitecture.Application.Models;
public class LogInDto : IMapFrom<User>
{

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<LogInDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }


}
