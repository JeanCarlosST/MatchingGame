//using MatchingGame.Client.Logging;
using MatchingGame.Server.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchingGame.Server.Logging
{
    public class ApplicationLoggerProvider : ILoggerProvider
    {
        private readonly Contexto _context;

        public IHttpContextAccessor _httpContextAccessor { get; }

        public ApplicationLoggerProvider(Contexto context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_context, _httpContextAccessor);
        }

        public void Dispose()
        {

        }
    }
}