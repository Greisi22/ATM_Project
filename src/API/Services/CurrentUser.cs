using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace API.Services;

public class CurrentUser : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid GetUserId()
    {
        if (!HasHttpContext())
            return Guid.Empty;

        var userIdClaim = GetClaimTypeByName("UserId");

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public Guid UserId => GetUserId();
 

    private string? GetClaimTypeByName(string claimName)
    {
        var claimType = GetClaims().Claims
            .FirstOrDefault(c => c.Type == claimName)?.Value;

        return claimType;
    }

    private bool HasHttpContext()
    {
        return _httpContextAccessor.HttpContext is not null;
    }

    private ClaimsPrincipal GetClaims()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
        {
            return new ClaimsPrincipal(); 
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        return new ClaimsPrincipal(new ClaimsIdentity(jwtToken?.Claims ?? Enumerable.Empty<Claim>(), "Token"));
    }
}
