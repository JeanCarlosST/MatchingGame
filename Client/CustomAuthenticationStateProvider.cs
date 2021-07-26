using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MatchingGame.Shared.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MatchingGame.Client.Services;

namespace MatchingGame.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUserService userService;
        private readonly ILocalStorageService localStorageService;

        public CustomAuthenticationStateProvider(IUserService userService, ILocalStorageService localStorageService)
        {
            this.userService = userService;
            this.localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetItemAsync<string>("jwt_token");

            ClaimsIdentity identity = new ClaimsIdentity();

            if (token != null && String.IsNullOrEmpty(token))
            {
                Usuarios usuarioActual = await userService.GetUserByJWT(token);
                identity = GetClaimsIdentity(usuarioActual);
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        private ClaimsIdentity GetClaimsIdentity(Usuarios usuario)
        {
            var claimsIdentity = new ClaimsIdentity();

            if(usuario.Email != null)
            {
                claimsIdentity = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(usuario.UsuarioId)),
                    new Claim(ClaimTypes.Name, usuario.NickName),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }, "serverAuth");
            }

            return claimsIdentity;
        }

        public async Task MarcarUsuarioLogeado(Usuarios usuario)
        {
            await localStorageService.SetItemAsync("jwt_token", usuario.Token);

            var identity = GetClaimsIdentity(usuario);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarcarUsuarioNoLogeado()
        {
            await localStorageService.RemoveItemAsync("jwt_token");

            var identity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        //public async Task<Usuarios> GetUserByJWTAsync()
        //{
        //    var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
        //    if(jwtToken == null) return null;

        //    return await _loginViewModel.GetUserByJWTAsync(jwtToken);
        //}

    }
}