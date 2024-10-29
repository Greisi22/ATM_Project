using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;


[ApiController]
[Authorize(Policy = "RequireAdminRole")]
[Route("user")]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    public UserController(ILogger<UserController> logger, IUserService userService) 
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    
    public async Task<IActionResult> Create([FromBody] UserDto userDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdUser = await _userService.Create(userDto, cancellationToken);

        return Ok(createdUser);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var userDto = await _userService.GetUserById(id);

        return Ok(userDto);
    }
}
