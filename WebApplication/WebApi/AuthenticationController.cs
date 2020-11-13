using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOptions<JwtSetting> _options;

        public AuthenticationController(IOptions<JwtSetting> options)
        {
            this._options = options;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("requestToken")]
        public ActionResult RequestToken()
        {
            string token=GetToken("admin",this._options.Value);

            var claims=GetClaims(token);

            return Content(token);
        }


        private string GetToken(string userName,JwtSetting setting)
        {
            string token = string.Empty;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(setting.Issuer, setting.Audience, claims,
                expires: DateTime.Now.AddSeconds(setting.ExpireSeconds),
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        public IEnumerable<Claim> GetClaims(string token)
        {
            var jwtHander = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = jwtHander.ReadJwtToken(token);
            return jwtSecurityToken.Claims.ToList();
        }
    }
}