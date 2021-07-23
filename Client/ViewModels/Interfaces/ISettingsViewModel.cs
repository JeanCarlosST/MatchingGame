using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared.Models;

namespace MatchingGame.ViewModels
{
    public interface ISettingsViewModel
    {
        public long UserId { get; set; }
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }

        public Task Save();
        public Task UpdateTheme();
        public Task UpdateNotifications();
        public Task GetProfile();
    }
}
