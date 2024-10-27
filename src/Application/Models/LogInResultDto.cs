﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Mappings;

namespace CleanArchitecture.Application.Models;
public class LogInResultDto 
{
    public bool Success { get; set; }
    public string Token { get; set; }  
    public IEnumerable<string> Errors { get; set; }

}
