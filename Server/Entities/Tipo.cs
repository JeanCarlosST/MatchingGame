using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Tipo
    {
        public Tipo()
        {
            Partida = new HashSet<Partida>();
        }

        public int TipoId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Partida> Partida { get; set; }
    }
}
