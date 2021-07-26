using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchingGame.Shared.Models
{
    public class Usuarios
    {
        [Key]
        public long UsuarioId { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Nombres { get; set; }
        public string NickName { get; set; }

        [NotMapped]
        public string Token { get; set; }

        [NotMapped]
        public bool Recuerdame { get; set; }

        public Usuarios() { }

        public Usuarios(RegisterModel usuarioRegistro)
        {
            Email = usuarioRegistro.Email;
            Clave = usuarioRegistro.Clave;
            Nombres = usuarioRegistro.Nombres;
            NickName = usuarioRegistro.NickName;
        }
    }
}
