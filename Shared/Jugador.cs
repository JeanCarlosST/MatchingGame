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
        public string UsuarioId { get; set; }
        public string Nickname { get; set; }
        public int Victorias { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Jugador))
                return false;
            
            Jugador jugador = (Jugador)obj;

            if (ConnectionId != jugador.ConnectionId)
                return false;
            
            if (Nickname != jugador.Nickname)
                return false;
            
            if (Victorias != jugador.Victorias)
                return false;
            
            return true;
        }
    }
}
