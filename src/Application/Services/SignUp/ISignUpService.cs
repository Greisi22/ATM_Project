using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Services.SignUp;
public interface ISignUpService
{
    Task<SignUpResultDto> SignUpAsync(SignUpDTO request, CancellationToken cancellationToken);
}
