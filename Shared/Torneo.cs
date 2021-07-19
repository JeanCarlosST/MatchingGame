using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Torneo
    {
        public int TorneoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int CantJugadores { get; set; }
        public int JugadorGanadorId { get; set; }
        public Jugador Jugador { get; set; }
        public List<Partida> Partidas { get; set; }
    }
}
/*
 * 
 *  if(count8)
 *  EmpezarTorneo()
 *  ListJgadores
 *  Finalist = await JugarPartida(Lista[1],Lista[2]).ganador;
 *  Finalist2
 *  
 *  
 * 
 * 
 * 
 * 
 * 
 */