using MatchingGame.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using MatchingGame.Shared.Models;
using MatchingGame.Server.Services;
using System.Net.Http;
using System.Net.Http.Json;
using MatchingGame.Server.DAL;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MatchingGame.Server.Entities;

namespace MatchingGame.Server.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Contexto _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(LoginModel usuarioLogin, bool esPersistente)
        {
            var respuesta = userService.Autenticar(usuarioLogin);
            return await Task.FromResult(respuesta);
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> RegistrarUsuario(RegisterModel usuarioRegistro)
        {
            var respuesta = userService.RegistrarUsuario(usuarioRegistro);

            return await Task.FromResult(respuesta);
        }
        [HttpPost("validar")]
        public int ValidarRegistro(RegisterModel usuarioRegistro)
        {
            int respuesta = userService.ValidarRegistro(usuarioRegistro);
            return respuesta;
        }

        [HttpPost("obtener_usuario")]
        public async Task<ActionResult<Usuarios>> ObtenerUsuarioPorJwt([FromBody]string jwtToken)
        {
            var respuesta = userService.ObtenerUsuarioPorJWT(jwtToken);
            Usuarios user = null;

            if(respuesta != null)
            {
                user = new Usuarios()
                {
                    UsuarioId = respuesta.UsuarioId,
                    Nombres = respuesta.Nombre,
                    Email = respuesta.Email,
                    NickName = respuesta.NickName
                };

                return await Task.FromResult(user);
            }

            return await Task.FromResult(user);
        }

        [HttpGet("GoogleSignIn")]
        public async Task GoogleSignIn()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "/"});
        }

        [HttpGet("TwitterSignIn")]
        public async Task TwitterSignIn()
        {
            await HttpContext.ChallengeAsync(TwitterDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "/" });
        }

        [HttpGet("FacebookSignIn")]
        public async Task FacebookSignIn()
        {         
           await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, 
               new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}