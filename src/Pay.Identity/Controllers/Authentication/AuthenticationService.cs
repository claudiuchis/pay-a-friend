using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Identity.Queries;
using static Pay.Identity.Queries.ReadModels;

namespace Pay.Identity.Authentication
{
    public class AuthenticationService
    {
        UserQueryService _queryService;
        Func<string, string, bool> _verifyPassword;        
        public AuthenticationService(
            UserQueryService queryService, 
            Func<string, string, bool> verifyPassword)
        {
            _queryService = queryService;
            _verifyPassword = verifyPassword;
        }

        public async Task<(bool result, UserDetails details)> CheckCredentials(
            string email,
            string password
        )
        {
            var userDetails = await _queryService.GetUserByEmail(email);

            if (userDetails != null && _verifyPassword(password, userDetails.HashedPassword))
            {
                // claims.Add(new Claim(ClaimTypes.NameIdentifier, userDetails.Id));
                // claims.Add(new Claim(ClaimTypes.Email, userDetails.Email));
                // claims.Add(new Claim(ClaimTypes.Name, userDetails.FullName));
                // claims.Add(new Claim(ClaimTypes.Role, "Member"));
                return (true, userDetails);
            }
            return (false, userDetails);
        }
    }
}
