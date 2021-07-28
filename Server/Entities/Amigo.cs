using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Amigo
    {
        public int AmigoId { get; set; }
        public int JugadorUnoId { get; set; }
        public int JugadorDosId { get; set; }
        public int Estado { get; set; }

        public virtual Usuario JugadorDos { get; set; }
        public virtual Usuario JugadorUno { get; set; }
    }
}
