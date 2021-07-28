using MatchingGame.Server.DAL;
using MatchingGame.Server.Entities;
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
        Usuario Autenticar(LoginModel usuarioLogin);
        bool RegistrarUsuario(RegisterModel usarioRegistro);
        public Usuario ObtenerUsuarioPorJWT(string jwtToken);
    }

    public class UserService : IUserService
    {
        private Context contexto;
        private IJwtUtils jwtUtils;
        private readonly AppSettings appSettings;

        public UserService(Context contexto, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            this.contexto = contexto;
            this.jwtUtils = jwtUtils;
            this.appSettings = appSettings.Value;
        }

        public Usuario Autenticar(LoginModel usuarioLogin)
        {
            Usuario usuario = 
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
            var NickNameExists = contexto.Usuarios.Where(u => u.NickName == usuarioRegistro.NickName).FirstOrDefault();
            
            if (emailAddressExists == null)
            {
                Usuario usuario = new Usuario(usuarioRegistro);
                contexto.Usuarios.Add(usuario);
                contexto.SaveChanges();
                return true;
            }

            return false;
        }

        public Usuario ObtenerUsuarioPorJWT(string jwtToken)
        {
            string id = jwtUtils.ObtenerUsuarioPorJWT(jwtToken);
            Usuario usuario = null;

            if (!String.IsNullOrEmpty(id))
            {
                usuario = contexto.Usuarios
                    .Where(u => u.UsuarioId == Convert.ToInt32(id))
                    .FirstOrDefault();
            }

            return usuario;
        }

        public int ValidarRegistro(RegisterModel usuarioRegistro)
        {
            var emailAddressExists = contexto.Usuarios.Where(u => u.Email == usuarioRegistro.Email);
            var NickNameExists = contexto.Usuarios.Where(u => u.NickName == usuarioRegistro.NickName).FirstOrDefault();
            if (emailAddressExists is null)
                return 1;
           else if (NickNameExists is null)
                return 2;
            else
                return 0;
        }
    }
}
