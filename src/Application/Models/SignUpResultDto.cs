﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Models;
public class SignUpResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } 
    public IEnumerable<string> Errors { get; set; }
}
