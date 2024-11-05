using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace CleanArchitecture.Application.Services.Google;
public interface IGoogleAuthService
{
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token);
    Task<User> GetOrCreateUserAsync(string email, string googleId, CancellationToken cancellationToken);
    Task<User> ValidateGoogleTokenAsync(string token, CancellationToken cancellationToken);
}
