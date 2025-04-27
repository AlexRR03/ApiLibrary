using ApiLibrary.Helpers;
using ApiLibrary.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NugetLibraryModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryLibrary repo;
        private HelperActionServiceOAuth helper;
        public AuthController(RepositoryLibrary repo, HelperActionServiceOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]

        public async Task<ActionResult>Login(string email, string password)
        {
            User user = await repo.LoginUsuariosAsync(email, password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonUsuario = JsonConvert.SerializeObject(user);
                Claim[] informacion = new[]
                {
                    new Claim("datosUsuario", jsonUsuario),
                };
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddHours(1),
                    notBefore: DateTime.UtcNow
                );
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            }

        }
    }
}
