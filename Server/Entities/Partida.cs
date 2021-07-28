using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Partida
    {
        public Partida()
        {
            PartidaJugadores = new HashSet<PartidaJugador>();
            TorneoPartida = new HashSet<TorneoPartida>();
        }

        public int PartidaId { get; set; }
        public DateTime Fecha { get; set; }
        public int ModoId { get; set; }
        public int TipoId { get; set; }
        public int DificultadId { get; set; }
        public int GanadorId { get; set; }

        public virtual Dificultad Dificultad { get; set; }
        public virtual Usuario Ganador { get; set; }
        public virtual Modo Modo { get; set; }
        public virtual Tipo Tipo { get; set; }
        public virtual ICollection<PartidaJugador> PartidaJugadores { get; set; }
        public virtual ICollection<TorneoPartida> TorneoPartida { get; set; }
    }
}
