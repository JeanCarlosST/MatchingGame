using System;
using System.Security.Claims;
using MatchingGame.Server.DAL;
using MatchingGame.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchingGame.Server.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly Contexto _context;

        public IHttpContextAccessor _httpContextAccessor { get; }

        public DatabaseLogger(Contexto context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            long userId = 0;
            if (user.Identity.IsAuthenticated)
                userId = Convert.ToInt64(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            Log log = new();
            log.LogLevel = logLevel.ToString();
            log.UserId = Convert.ToInt64(userId);
            log.ExceptionMessage = exception?.Message;
            log.StackTrace = exception?.StackTrace;
            log.Source = "Server";
            log.CreatedDate = DateTime.Now.ToString();

            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}