using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class PartidaJugador
    {
        public int PartidaJugadorId { get; set; }
        public int PartidaId { get; set; }
        public int JugadorId { get; set; }
        public double Tiempo { get; set; }
        public double Puntos { get; set; }

        public virtual Usuario Jugador { get; set; }
        public virtual Partida Partida { get; set; }
    }
}
