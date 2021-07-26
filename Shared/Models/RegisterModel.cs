using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared.Models;

namespace MatchingGame.Shared.Models
{
    public class RegisterModel
    { 
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        

        //private HttpClient _httpClient;
        //public Register()
        //{

        //}
        //public Register(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task RegisterUser()
        //{ 
        //    await _httpClient.PostAsJsonAsync<Usuarios>("user/registeruser", this);
        //}

        //public static implicit operator Register(Usuarios user)
        //{
        //    return new Register
        //    {
        //        Email = user.Email,
        //        Clave = user.Clave
        //    };
        //}

        //public static implicit operator Usuarios(Register registerViewModel)
        //{
        //    return new Usuarios
        //    {
        //        Email = registerViewModel.Email,
        //        Clave = registerViewModel.Clave
        //    };
        //}
    }
}