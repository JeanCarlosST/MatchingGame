using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Client;
using MatchingGame.Shared;
using MatchingGame.Shared.Models;

namespace MatchingGame.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        public string Email { get; set; }
        public string Clave { get; set; }
        public bool Recuerdame { get; set; }

        private HttpClient _httpClient;
        public LoginViewModel()
        {

        }
        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoginUser()
        {
            await _httpClient.PostAsJsonAsync<Usuarios>($"user/loginuser?isPersistent={this.Recuerdame}", this);
        }

        public async Task<AuthenticationResponse> AuthenticateJWT()
        {
            //creating authentication request
            AuthenticationRequest authenticationRequest = new AuthenticationRequest();
            authenticationRequest.EmailAddress = this.Email;
            authenticationRequest.Password = this.Clave;
            
            //authenticating the request
            var httpMessageReponse = await _httpClient.PostAsJsonAsync<AuthenticationRequest>($"user/authenticatejwt", authenticationRequest);
            
            //sending the token to the client to store
            return await httpMessageReponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
        }
        public async Task<string> GetFacebookAppIDAsync()
        {
            return await _httpClient.GetStringAsync("user/getfacebookappid");
        }

        public async Task<Usuarios> GetUserByJWTAsync(string jwtToken)
        {
            //preparing the http request
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "user/getuserbyjwt");
            requestMessage.Content = new StringContent(jwtToken);
        
            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        
            //making the http request
            var response = await _httpClient.SendAsync(requestMessage);
        
            //var responseStatusCode = response.StatusCode;
            var returnedUser = await response.Content.ReadFromJsonAsync<Usuarios>();
        
            //returning the user if found
            if(returnedUser != null) return await Task.FromResult(returnedUser);
            else return null;
        }

        public async Task<TwitterRequestTokenResponse> GetTwitterOAuthTokenAsync()
        {
            return await _httpClient.GetFromJsonAsync<TwitterRequestTokenResponse>("user/gettwitteroauthtokenusingresharp");
        }

        public static implicit operator LoginViewModel(Usuarios user)
        {
            return new LoginViewModel
            {
                Email = user.Email,
                Clave = user.Clave
            };
        }

        public static implicit operator Usuarios(LoginViewModel loginViewModel)
        {
            return new Usuarios
            {
                Email = loginViewModel.Email,
                Clave = loginViewModel.Clave
            };
        }
    }
}