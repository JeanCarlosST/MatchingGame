using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Torneo
    {
        public Torneo()
        {
            TorneoJugadores = new HashSet<TorneoJugador>();
            TorneoPartida = new HashSet<TorneoPartida>();
        }

        public int TorneoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int CantJugadores { get; set; }
        public int GanadorId { get; set; }

        public virtual Usuario Ganador { get; set; }
        public virtual ICollection<TorneoJugador> TorneoJugadores { get; set; }
        public virtual ICollection<TorneoPartida> TorneoPartida { get; set; }
    }
}
