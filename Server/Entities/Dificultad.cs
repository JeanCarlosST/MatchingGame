using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Dificultad
    {
        public Dificultad()
        {
            Partida = new HashSet<Partida>();
        }

        public int DificultadId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Partida> Partida { get; set; }
    }
}
