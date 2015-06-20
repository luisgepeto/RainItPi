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
        private const string Audience = "RainItWebServiceApplication";

        public IRainItContext RainItContext { get; set; }

        public TokenManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        public string CreateJwtToken(string serial)
        {
            var device = RainItContext.DeviceSet.SingleOrDefault(d => d.DeviceInfo.Serial == serial);
            if (device == null) return String.Empty;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, device.DeviceInfo.Serial),
                new Claim(ClaimTypes.Hash, device.DeviceInfo.Identifier.ToString())
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
                AppliesToAddress = "http://" + Audience,
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
                SigningCredentials = signingCredentials
            };
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }
        
        public ClaimsPrincipal ValidateToken(string encodedTokenString)
        {
            ClaimsPrincipal principal = null;

            SecurityToken token;
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = TokenIssuer,
                ValidAudience = "http://"+Audience,
                IssuerSigningToken = new BinarySecretSecurityToken(Encoding.UTF8.GetBytes(SecretKey))
            };
            principal = new JwtSecurityTokenHandler().ValidateToken(encodedTokenString, tokenValidationParameters, out token);
            
            return principal;
        }
    }
}
