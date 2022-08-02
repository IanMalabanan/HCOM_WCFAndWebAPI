using Hcom.Web.Api.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface ITokenService
    {
        Task<AuthResult> GenerateJwtToken(ApplicationUser user);
        Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest);
        Task<AuthResult> IsValidToken(TokenRequest tokenRequest);
    }
}
