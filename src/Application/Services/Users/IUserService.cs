using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Services.Users;
public interface IUserService
{
    Task <List<UserDto>> GetAll();
    Task<UserDto> Create(UserDto user, CancellationToken cancellationToken);

    Task<UserDto> GetUserById(Guid id);
}
