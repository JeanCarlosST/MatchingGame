using MatchingGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Helpers
{
    public class Peticion
    {
        public Jugador Jugador { get; set; }
        public string Descripcion { get; set; }

        public Peticion(Jugador jugador, string connectionId, string descripcion)
        {
            this.Jugador = jugador;
            this.Jugador.ConnectionId = connectionId;
            this.Descripcion = descripcion;
        }
    }
}
