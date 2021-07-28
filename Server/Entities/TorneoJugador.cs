using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class TorneoJugador
    {
        public int TorneoJugadorId { get; set; }
        public int TorneoId { get; set; }
        public int UsuarioId { get; set; }
        public bool Descalificado { get; set; }

        public virtual Torneo Torneo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
