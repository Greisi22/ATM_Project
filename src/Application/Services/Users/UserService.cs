using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services.Users;
public class UserService : IUserService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public UserService(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public async Task<List<UserDto>> GetAll()
    {
        var users = await _applicationDbContext.EntitySet<User>().ToListAsync();

        var result = _mapper.Map<List<UserDto>>(users);

        return result;
    }

    public async Task<UserDto> Create(UserDto userDto, CancellationToken cancellationToken)
    {
       
        var user = _mapper.Map<User>(userDto);
        user.Id = Guid.NewGuid(); 

        _applicationDbContext.EntitySet<User>().Add(user);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var createdUserDto = _mapper.Map<UserDto>(user);
        return createdUserDto;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user= await _applicationDbContext.EntitySet<User>().FindAsync(id);

        if (user == null)
        {
            return null; 
        }

        
        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
}

