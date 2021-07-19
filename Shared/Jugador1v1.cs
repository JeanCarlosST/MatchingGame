using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Jugador1v1
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public int ParesEncontrados { get; set; }
        public int ParesTotal { get; set; }
        public int Victorias { get; set; }
        public int Puntos { get; set; }
        public string Tiempo { get; set; }
        public bool listo { get; set; }
        public bool terminado { get; set; }
    }
}
