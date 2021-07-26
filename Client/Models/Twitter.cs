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
    public class Twitter
    {
        private readonly HttpClient _httpClient;
        //public Twitter(HttpClient httpClient)
        //{
        //    this._httpClient = httpClient;
        //}

        //public async Task<string> GetTwitterJWTAsync(TwitterRequestTokenResponse twitterRequestTokenResponse)
        //{
        //    var httpMessageResponse = await _httpClient.PostAsJsonAsync<TwitterRequestTokenResponse>("user/getTwitterjwt", twitterRequestTokenResponse);
        //    return (await httpMessageResponse.Content.ReadFromJsonAsync<AuthenticationResponse>()).Token;
        //}
    }
}
