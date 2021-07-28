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
        public List<TorneoJugador> Jugadores { get; set; }
        public List<TorneoJugador> JugadoresSigteRonda { get; set; }
        public List<TorneoPartida> Partidas { get; set; }
        public bool Iniciado { get; set; }
        public bool Terminado { get; set; }

        public Torneo(int cantJugs)
        {
            FechaCreacion = DateTime.Now;
            CantMaxJugadores = cantJugs;
            RondaActual = -1;
            Jugadores = new List<TorneoJugador>();
            JugadoresSigteRonda = new List<TorneoJugador>();
        }

        public void Iniciar()
        {
            Iniciado = true;
            FechaIniciado = DateTime.Now;
            Partidas = new List<TorneoPartida>();
            JugadoresSigteRonda = new List<TorneoJugador>(Jugadores);
            RondaActual++;

            CrearPartidas();
        }

        public void CrearPartidas()
        {
            for(int i = 0; i < JugadoresSigteRonda.Count / 2; i++)
            {
                var (modo, dificultad) = ObtenerModoYDificultadPorRonda(RondaActual);
                TorneoPartida partida = new TorneoPartida(modo, dificultad, RondaActual)
                {
                    JugadorUno = JugadoresSigteRonda[i * 2],
                    JugadorDos = JugadoresSigteRonda[(i * 2) + 1],
                    Iniciada = true
                };
                ((TorneoJugador)partida.JugadorUno).Estado = 1;
                ((TorneoJugador)partida.JugadorDos).Estado = 1;
                Partidas.Add(partida);
            }

            JugadoresSigteRonda.Clear();
        }

        public static (Modo, Dificultad) ObtenerModoYDificultadPorRonda(int ronda)
        {
            switch(ronda)
            {
                case 0:
                    return (Modo.Normal, Dificultad._4x4);
                case 1:
                    return (Modo.Normal, Dificultad._4x6);
                case 2:
                    return (Modo.Normal, Dificultad._4x8);
                case 3:
                    return (Modo.Normal, Dificultad._8x8);
                default:
                    return (Modo.Normal, Dificultad._4x4);
            }
        }

        public void VerificarPaseDeRonda()
        {
            if(Partidas.FindAll(p => p.Ronda == RondaActual).All(p => p.Terminada))
            {
                if(JugadoresSigteRonda.Count == 1)
                {
                    Terminado = true;
                    FechaTerminado = DateTime.Now;
                }
                else
                {
                    RondaActual++;
                    CrearPartidas();
                }
            }
        }

        public void AgregarJugador(TorneoJugador jugador)
        {
            Jugadores.Add(jugador);
        }

        public bool JugadorExiste(TorneoJugador jugador)
        {
            return Jugadores.Find(j => jugador.Nickname.Equals(j.Nickname)) != null;
        }
    }

    public class TorneoPartida : Partida
    {
        public int Ronda { get; set; }

        public TorneoPartida(Modo modo, Dificultad dificultad, int ronda) : base(modo, dificultad)
        {
            this.Ronda = ronda;
        }

        public bool ContieneJugador(string connectionId)
        {
            return JugadorUno.ConnectionId == connectionId || JugadorDos.ConnectionId == connectionId;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TorneoPartida))
                return false;

            Partida partida = (TorneoPartida)obj;

            if (!JugadorUno.Equals(partida.JugadorUno))
                return false;

            if (!JugadorDos.Equals(partida.JugadorDos))
                return false;

            if (Modo != partida.Modo)
                return false;

            if (Dificultad != partida.Dificultad)
                return false;

            if (ParesTotales != partida.ParesTotales)
                return false;

            //if (Iniciada != partida.Iniciada)
            //    return false;

            if (Terminada != partida.Terminada)
                return false;

            return true;
        }

        public override string ToString()
        {
            string ganador = null;

            if (JugadorUnoDetalle.Tiempo < JugadorDosDetalle.Tiempo)
                ganador = JugadorUno.Nickname;

            if (JugadorDosDetalle.Tiempo < JugadorUnoDetalle.Tiempo)
                ganador = JugadorDos.Nickname;

            if (!string.IsNullOrEmpty(ganador))
                return $"PartidaGanador: {ganador}, Ronda: {Ronda}";
            else
                return $"Partida; J1: {JugadorUno.Nickname}, J2: {JugadorDos.Nickname}";
        }
    }

    public class TorneoJugador : Jugador
    {
        public int Estado { get; set; } // -1 Abandonado, 0 En espera, 1 Jugando, 2 Ausente
        public DateTime? TiempoAusente { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TorneoJugador))
                return false;

            TorneoJugador jugador = (TorneoJugador)obj;

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