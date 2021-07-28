using MatchingGame.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            AmigoJugadorDos = new HashSet<Amigo>();
            AmigoJugadorUnos = new HashSet<Amigo>();
            Partida = new HashSet<Partida>();
            PartidaJugadores = new HashSet<PartidaJugador>();
            Ratings = new HashSet<Rating>();
            TorneoJugadores = new HashSet<TorneoJugador>();
            Torneos = new HashSet<Torneo>();
        }

        public Usuario(RegisterModel usuarioRegistro)
        {
            Email = usuarioRegistro.Email;
            Clave = usuarioRegistro.Clave;
            Nombre = usuarioRegistro.Nombres;
            NickName = usuarioRegistro.NickName;
            Activo = true;
        }

        public int UsuarioId { get; set; }
        public string NickName { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool? Activo { get; set; }

        [NotMapped]
        public string Token { get; set; }

        [NotMapped]
        public bool Recuerdame { get; set; }

        public virtual ICollection<Amigo> AmigoJugadorDos { get; set; }
        public virtual ICollection<Amigo> AmigoJugadorUnos { get; set; }
        public virtual ICollection<Partida> Partida { get; set; }
        public virtual ICollection<PartidaJugador> PartidaJugadores { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<TorneoJugador> TorneoJugadores { get; set; }
        public virtual ICollection<Torneo> Torneos { get; set; }
    }
}
