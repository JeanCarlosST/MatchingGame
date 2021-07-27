using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Client.Models
{
    public interface ISettings
    {
        public long UserId { get; set; }
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }

        public Task Save();
        public Task UpdateTheme();
        public Task UpdateNotifications();
        public Task GetProfile();
    }

    public class Settings : ISettings
    {
        public long UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DarkTheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task GetProfile()
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task UpdateNotifications()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTheme()
        {
            throw new NotImplementedException();
        }
    }
}
