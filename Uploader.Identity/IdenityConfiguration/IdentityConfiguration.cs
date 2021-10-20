using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Uploader.Identity.IdenityConfiguration
{
    public static class IdentityConfiguration
    {
        public static ICollection<string> ProfileUserClaims()
        {
            var currentClaims = new IdentityResources.Profile().UserClaims;

            currentClaims.Add(JwtClaimTypes.Role);

            return currentClaims;
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() { UserClaims = ProfileUserClaims() },
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("uploader.api", "Uploader Api"),
                new ApiScope("uploader.web.api", "Uploader WEB Api")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("uploader.api")
                {
                    Scopes = { "uploader.api" },
                    UserClaims = { JwtClaimTypes.Role }
                },
                new ApiResource("uploader.web.api")
                {
                    Scopes = { "uploader.web.api" },
                    UserClaims = { JwtClaimTypes.Role }
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "uploader.api.swaggerui",
                    ClientName = "Uploader Api Swagger UI",
                    ClientUri = "https://localhost:6001",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =
                    {
                        "https://localhost:6001/swagger/oauth2-redirect.html"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "uploader.api",
                    },
                    AllowedCorsOrigins = { "https://localhost:6001" },
                },

                new Client
                {
                    ClientId = "uploader.web",
                    ClientName = "Uploader Web",
                    ClientUri = "https://localhost:5001",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "https://localhost:5001/signin-oidc",
                    },

                    PostLogoutRedirectUris = { "https://localhost:5001/signout-oidc" },
                    AllowedCorsOrigins = { "https://localhost:5001" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "uploader.api",
                        "uploader.web.api"
                    },

                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}
