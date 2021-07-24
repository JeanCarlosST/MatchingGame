using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared;
using MatchingGame.Shared.Models;

namespace MatchingGame.ViewModels
{
    public interface ILoginViewModel
    {
        public string Email { get; set; }
        public string Clave { get; set; }
        public bool Recuerdame { get; set; }
        public Task LoginUser();
        public Task<AuthenticationResponse> AuthenticateJWT();
        public Task<string> GetFacebookAppIDAsync();
        public Task<TwitterRequestTokenResponse> GetTwitterOAuthTokenAsync();
        public Task<Usuarios> GetUserByJWTAsync(string jwtToken);
    }
}