using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Client;
using MatchingGame.Shared.Models;

namespace MatchingGame.ViewModels
{
    public class RegisterViewModel : IRegisterViewModel
    {
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        

        private HttpClient _httpClient;
        public RegisterViewModel()
        {

        }
        public RegisterViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RegisterUser()
        { 
            await _httpClient.PostAsJsonAsync<Usuarios>("user/registeruser", this);
        }

        public static implicit operator RegisterViewModel(Usuarios user)
        {
            return new RegisterViewModel
            {
                Email = user.Email,
                Clave = user.Clave
            };
        }

        public static implicit operator Usuarios(RegisterViewModel registerViewModel)
        {
            return new Usuarios
            {
                Email = registerViewModel.Email,
                Clave = registerViewModel.Clave
            };
        }
    }
}