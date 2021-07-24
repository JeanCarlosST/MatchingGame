using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Torneo
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaIniciado { get; set; }
        public DateTime FechaTerminado { get; set; }
        public int RondaActual { get; set; }
        public int CantMaxJugadores { get; set; }
        public List<Jugador> Jugadores { get; set; }
        public List<Jugador> JugadoresSigteRonda { get; set; }
        public List<Partida> Partidas { get; set; }
        public bool Iniciado { get; set; }
        public bool Terminado { get; set; }

        public Torneo(int cantJugs)
        {
            FechaCreacion = DateTime.Now;
            CantMaxJugadores = cantJugs;
            RondaActual = -1;
            Jugadores = new List<Jugador>();
            //JugadoresSigteRonda = new List<Jugador>();
            Partidas = new List<Partida>();
        }

        public void Iniciar()
        {
            FechaIniciado = DateTime.Now;
            RondaActual++;
            Iniciado = true;
            JugadoresSigteRonda = new List<Jugador>(Jugadores);

            CrearPartidas();
        }

        public void CrearPartidas()
        {
            for(int i = 0; i < JugadoresSigteRonda.Count / 2; i++)
            {
                TorneoPartida partida = new TorneoPartida(Modo.Normal, Dificultad._4x4)
                {
                    Ronda = RondaActual,
                    JugadorUno = JugadoresSigteRonda[i * 2],
                    JugadorDos = JugadoresSigteRonda[(i * 2) + 1]
                };
                Partidas.Add(partida);
            }

            JugadoresSigteRonda.Clear();
        }

        public void VerificarPasarRonda()
        {

        }

        public void AgregarJugador(Jugador jugador)
        {
            Jugadores.Add(jugador);
        }
    }

    public class TorneoPartida : Partida
    {
        public int Ronda { get; set; }

        public TorneoPartida(Modo modo, Dificultad dificultad) : base(modo, dificultad)
        {

        }
    }
}