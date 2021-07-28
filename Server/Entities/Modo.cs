using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Modo
    {
        public Modo()
        {
            Partida = new HashSet<Partida>();
        }

        public int ModoId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Partida> Partida { get; set; }
    }
}
