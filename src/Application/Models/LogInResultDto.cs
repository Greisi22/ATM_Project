using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Models;
public class LogInResultDto 
{
    public bool Success { get; set; }
    public string Token { get; set; }  
    public UserRole role {  get; set;  }
    public IEnumerable<string> Errors { get; set; }

}
