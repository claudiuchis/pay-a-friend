using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Pay.Identity.Projections.ReadModels;

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

        public bool CheckCredentials(
            string email,
            string password,
            out UserDetails userDetails
        )
        {
            var builder = Builders<UserDetails>.Filter;
            var filter = builder.Eq(user => user.Email, email);
            userDetails = _database.GetDocumentCollection<UserDetails>().Find(filter).FirstOrDefault();

            if (userDetails != null && _verifyPassword(password, userDetails.HashedPassword))
            {
                // claims.Add(new Claim(ClaimTypes.NameIdentifier, userDetails.Id));
                // claims.Add(new Claim(ClaimTypes.Email, userDetails.Email));
                // claims.Add(new Claim(ClaimTypes.Name, userDetails.FullName));
                // claims.Add(new Claim(ClaimTypes.Role, "Member"));
                return true;
            }
            return false;
        }
    }
}
