using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatchingGame.Shared.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
        }

        [Key]
        public long UsuarioId { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Source { get; set; }
        public string Nombres { get; set; }
        public string NickName { get; set; }
    }
}
