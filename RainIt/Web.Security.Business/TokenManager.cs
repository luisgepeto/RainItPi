using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using RainIt.Interfaces.Repository;
using Web.Security.Domain;
using Web.Security.Interfaces;

namespace Web.Security.Business
{
    public class TokenManager : ITokenManager
    {
        private const string SecretKey = "ZSA23YHBS3LS9ZXS30AFE";
        private const string TokenIssuer = "RainItTokenService";

        public IRainItContext RainItContext { get; set; }

        public TokenManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        public string CreateJwtToken(string userName)
        {
            var profile = RainItContext.UserSet.SingleOrDefault(u => u.Username == userName);
            if (profile == null) return String.Empty;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, profile.Roles.FirstOrDefault().Name),
            };
           
            return CreateSecurityToken(claims);
        }

        private string CreateSecurityToken(List<Claim> claims)
        {
            if (claims == null) return null;
            var signingCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256");

            var claimsIdentity = new ClaimsIdentity(claims, "Custom Authentication", ClaimTypes.Name, ClaimTypes.Role);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                TokenIssuerName = TokenIssuer,
                //AppliesToAddress = "http://" + audience,
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
                SigningCredentials = signingCredentials
            };
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor MakeSecurityTokenDescriptor(InMemorySymmetricSecurityKey sSKey, List<Claim> claimList)
        {
            var now = DateTime.UtcNow;
            Claim[] claims = claimList.ToArray();
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = SecurityConstants.TokenIssuer,
                AppliesToAddress = SecurityConstants.TokenAudience,
                Lifetime = new Lifetime(now, now.AddMinutes(SecurityConstants.TokenLifetimeMinutes)),
                SigningCredentials = new SigningCredentials(sSKey,
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };
        }

        public ClaimsPrincipal ValidateJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Parse JWT from the Base64UrlEncoded wire form (<Base64UrlEncoded header>.<Base64UrlEncoded body>.<signature>)
            //string parsedJwt = tokenHandler.ReadToken(jwtToken);

            TokenValidationParameters validationParams =
                new TokenValidationParameters()
                {
                    ValidIssuer = SecurityConstants.TokenIssuer,
                    ValidateIssuer = true,
                    //SigningToken = new BinarySecretSecurityToken(SecurityConstants.KeyForHmacSha256),
                };

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(jwtToken, validationParams, out validatedToken);
        }
    }
}
