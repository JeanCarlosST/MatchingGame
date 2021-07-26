using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared;
using MatchingGame.Shared.Models;

namespace MatchingGame.Client.Models
{
    public interface IFacebookAuthViewModel
    {
        public Task<string> GetFacebookJWTAsync(string accessToken);
    }

    public class FacebookAuth : IFacebookAuthViewModel
    {
        private readonly HttpClient _httpClient;
        public FacebookAuth(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<string> GetFacebookJWTAsync(string accessToken)
        {
            var httpMessageResponse = await _httpClient.PostAsJsonAsync<FacebookAuthRequest>("user/getfacebookjwt", new FacebookAuthRequest() { AccessToken = accessToken });
            return (await httpMessageResponse.Content.ReadFromJsonAsync<AuthenticationResponse>()).Token;
        }
    }
}
