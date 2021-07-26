using MatchingGame.Server.DAL;
using MatchingGame.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatchingGame.Server.Services
{
    public interface IUserService
    {
        Usuarios Autenticar(LoginModel usuarioLogin);
        bool RegistrarUsuario(RegisterModel usarioRegistro);
        public Usuarios ObtenerUsuarioPorJWT(string jwtToken);
    }

    public class UserService : IUserService
    {
        private Contexto contexto;
        private IJwtUtils jwtUtils;
        private readonly AppSettings appSettings;

        public UserService(Contexto contexto, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            this.contexto = contexto;
            this.jwtUtils = jwtUtils;
            this.appSettings = appSettings.Value;
        }

        public Usuarios Autenticar(LoginModel usuarioLogin)
        {
            Usuarios usuario = 
                contexto.Usuarios
                    .Where(u => u.Email == usuarioLogin.Email && u.Clave == usuarioLogin.Clave)
                    .FirstOrDefault();

            if(usuario != null)
            {
                var jwtToken = jwtUtils.GenerarJWTToken(usuario);
                usuario.Token = jwtToken;
            }

            return usuario;
        }

        public bool RegistrarUsuario(RegisterModel usuarioRegistro)
        {
            var emailAddressExists = contexto.Usuarios.Where(u => u.Email == usuarioRegistro.Email).FirstOrDefault();
            if (emailAddressExists == null)
            {
                Usuarios usuario = new Usuarios(usuarioRegistro);
                contexto.Usuarios.Add(usuario);
                contexto.SaveChanges();
                return true;
            }

            return false;
        }

        public Usuarios ObtenerUsuarioPorJWT(string jwtToken)
        {
            string id = jwtUtils.ObtenerUsuarioPorJWT(jwtToken);
            Usuarios usuario = null;

            if (!String.IsNullOrEmpty(id))
            {
                usuario = contexto.Usuarios
                    .Where(u => u.UsuarioId == Convert.ToInt32(id))
                    .FirstOrDefault();
            }

            return usuario;
        }
    }
}
