using MatchingGame.Client.Models;
using MatchingGame.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MatchingGame.Client.Services
{
    public interface IUserService
    {
        public Task<Usuarios> LoginAsync(LoginModel usuarioLogin);
        public Task<bool> RegisterAsync(RegisterModel usuarioRegister);
        public Task<Usuarios> ObtenerUsuarioPorJWT(string jwt);
        public Task<Int32> Validar(RegisterModel usuarioRegister);
    }

    public class UserService : IUserService
    {
        public HttpClient httpClient { get; set; }

        public UserService(HttpClient _httpClient)
        {
            this.httpClient = _httpClient;
        }

        public async Task<Usuarios> ObtenerUsuarioPorJWT(string jwtToken)
        {
            var respuesta = await httpClient.PostAsJsonAsync("user/obtener_usuario", jwtToken);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            var usuarioDevuelto = JsonConvert.DeserializeObject<Usuarios>(cuerpo);
            return await Task.FromResult(usuarioDevuelto);
        }

        public async Task<Usuarios> LoginAsync(LoginModel usuarioLogin)
        {
            //usuarioLogin.Clave = Utility.Encrypt(usuarioLogin.Clave);
            var respuesta = await httpClient.PostAsJsonAsync<LoginModel>(
                $"user/login?esPersistente={usuarioLogin.Recuerdame}", usuarioLogin);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            var usuarioDevuelto = JsonConvert.DeserializeObject<Usuarios>(cuerpo);
            return await Task.FromResult(usuarioDevuelto);
        }

        public async Task<bool> RegisterAsync(RegisterModel usuarioRegister)
        {
            //usuarioRegister.Clave = Utility.Encrypt(usuarioRegister.Clave);
            Console.WriteLine(httpClient.BaseAddress);
            var respuesta = await httpClient.PostAsJsonAsync<RegisterModel>("user/register", usuarioRegister);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            var usuarioDevuelto = JsonConvert.DeserializeObject<bool>(cuerpo);
            return await Task.FromResult(usuarioDevuelto);
        }
        public async Task<Int32> Validar(RegisterModel usuarioRegister)
        {
            var respuesta = await httpClient.PostAsJsonAsync<RegisterModel>("user/validar", usuarioRegister);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            var usuarioDevuelto = JsonConvert.DeserializeObject<Int32>(cuerpo);
            Console.WriteLine(usuarioDevuelto);
            return usuarioDevuelto;
        }
    }
}
