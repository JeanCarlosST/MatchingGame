using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class TorneoPartida
    {
        public int TorneoPartidaId { get; set; }
        public int TorneoId { get; set; }
        public int PartidaId { get; set; }

        public virtual Partida Partida { get; set; }
        public virtual Torneo Torneo { get; set; }
    }
}
