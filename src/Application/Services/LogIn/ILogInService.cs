using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Services.LogIn;
public interface ILogInService
{
    Task<LogInResultDto> LoginAsync(LogInDto request, CancellationToken cancellationToken);
}
