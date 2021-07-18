using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class TorneoJugador
    {
        public int TorneoJugadorId { get; set; }
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }
        public int CantVictorias { get; set; }

    }
}
