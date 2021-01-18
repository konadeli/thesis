using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "0000000001",
                    Username = "kona",
                    Password = "deli",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Kona Deli"),
                        new Claim(JwtClaimTypes.GivenName, "Kona"),
                        new Claim(JwtClaimTypes.FamilyName, "Deli"),
                        new Claim(JwtClaimTypes.Email, "kona.deli@gmail.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://kona.com")
                    }
                },

            };
    }
}