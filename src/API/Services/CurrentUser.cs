using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace API.Services;

public class CurrentUser : ICurrentUserService
{
    readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;
    public CurrentUser(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
    {
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }
    public string UserId => GetUserName();

    private string GetUserName()
    {
        if (!HasHttpContext() || _env.IsDevelopment()) return string.Empty;

        var username = GetClaimTypeByName("username");
        return username ?? string.Empty;
    }

    private string? GetClaimTypeByName(string claimName)
    {
        var claimType = GetClaims().Claims
            .Where(c => c.Type == claimName)
            .Select(c => c.Value)
            .FirstOrDefault();

        return claimType;
    }

    private bool HasHttpContext()
    {
        return (_httpContextAccessor.HttpContext is not null);
    }

    private ClaimsPrincipal GetClaims()
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(CurrentToken()) as JwtSecurityToken;

        var claimsIdentity = new ClaimsIdentity(token.Claims, "Token");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }

    private string CurrentToken()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"];
        var token = authorizationHeader.FirstOrDefault()?.Substring("Bearer ".Length).Trim();

        return token;
    }
}
