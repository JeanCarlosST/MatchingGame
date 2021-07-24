using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared;

namespace MatchingGame.ViewModels
{
    public interface ITwitterAuthViewModel
    {
        public Task<string> GetTwitterJWTAsync(TwitterRequestTokenResponse twitterRequestTokenResponse);
    }
}
