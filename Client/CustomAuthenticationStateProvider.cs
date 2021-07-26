using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MatchingGame.Shared.Models;
using MatchingGame.ViewModels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MatchingGame.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILoginViewModel _loginViewModel;
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationStateProvider(ILoginViewModel loginViewModel, ILocalStorageService localStorageService)
        {
            _loginViewModel = loginViewModel;
            _localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Usuarios currentUser = await GetUserByJWTAsync();        
         
            if (currentUser != null && currentUser.Email != null)
            {
                var claimEmailAddress = new Claim(ClaimTypes.Name, currentUser.Email);
                var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.UsuarioId));
                var claimsIdentity = new ClaimsIdentity(new[] { claimEmailAddress, claimNameIdentifier }, "serverAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
            else
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task<Usuarios> GetUserByJWTAsync()
        {
            var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
            if(jwtToken == null) return null;

            return await _loginViewModel.GetUserByJWTAsync(jwtToken);
        }
    }
}