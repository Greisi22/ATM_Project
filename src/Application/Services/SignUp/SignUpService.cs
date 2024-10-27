
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services.SignUp;
public class SignUpService : ISignUpService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public SignUpService(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SignUpResultDto> SignUpAsync(SignUpDTO request, CancellationToken cancellationToken)
    {

        bool userExists = await _applicationDbContext.EntitySet<User>()
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (userExists)
        {
            return new SignUpResultDto { Success = false, Errors = new[] { "Invalid username or password." } };
        }


        var user = _mapper.Map<User>(request);
        user.Password = request.CreatePasswordHash();

        await _applicationDbContext.EntitySet<User>().AddAsync(user);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new SignUpResultDto { Success = true, Message = "User registered successfully" };
    }

    public Task<SignUpResultDto> SignUpAsync(SignUpDTO request)
    {
        throw new NotImplementedException();
    }
}

