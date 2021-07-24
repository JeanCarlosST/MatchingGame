using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Shared.Models;

namespace MatchingGame.ViewModels
{
    public interface IRegisterViewModel
    {
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }


        public Task RegisterUser();
    }
}