using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Jugador
    {
        public string ConnectionId { get; set; }
        public string Nickname { get; set; }
        public int Victorias { get; set; }
    }
}
