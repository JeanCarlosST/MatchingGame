using MatchingGame.Shared.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Server
{
    public interface IJwtUtils
    {
        public string GenerarJWTToken(Usuarios usuario);
        //public string ObtenerUsuarioPorJWT(string jwtToken);
        public int? ValidarJWTToken(string token);

    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings appSettings;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public string GenerarJWTToken(Usuarios usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            
            var claimEmail = new Claim(ClaimTypes.Email, usuario.Email);
            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString());
            var claimNickname = new Claim(ClaimTypes.Name, usuario.NickName);

            var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimNameIdentifier, claimNickname }, "serverAuth");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        //public string ObtenerUsuarioPorJWT(string jwtToken)
        //{
        //    try
        //    {
        //        var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

        //        var tokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //        };
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        SecurityToken securityToken;

        //        var principle = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);
        //        var jwtSecurityToken = (JwtSecurityToken)securityToken;

        //        if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //            return userId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception : " + ex.Message);
        //        return null;
        //    }

        //    return null;
        //}

        public int? ValidarJWTToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
