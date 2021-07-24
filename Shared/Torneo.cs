﻿using System;
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
        public List<TorneoPartida> Partidas { get; set; }
        public bool Iniciado { get; set; }
        public bool Terminado { get; set; }

        public Torneo(int cantJugs)
        {
            FechaCreacion = DateTime.Now;
            CantMaxJugadores = cantJugs;
            RondaActual = -1;
            Jugadores = new List<Jugador>();
            JugadoresSigteRonda = new List<Jugador>();
        }

        public void Iniciar()
        {
            Iniciado = true;
            FechaIniciado = DateTime.Now;
            Partidas = new List<TorneoPartida>();
            JugadoresSigteRonda = new List<Jugador>(Jugadores);
            RondaActual++;

            CrearPartidas();
        }

        public void CrearPartidas()
        {
            for(int i = 0; i < JugadoresSigteRonda.Count / 2; i++)
            {
                TorneoPartida partida = new TorneoPartida(Modo.Normal, Dificultad._4x4, RondaActual)
                {
                    JugadorUno = JugadoresSigteRonda[i * 2],
                    JugadorDos = JugadoresSigteRonda[(i * 2) + 1],
                    Iniciada = true
                };
                Partidas.Add(partida);
            }

            JugadoresSigteRonda.Clear();
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

        public void AgregarJugador(Jugador jugador)
        {
            Jugadores.Add(jugador);
        }

        public bool JugadorExiste(Jugador jugador)
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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TorneoPartida))
                return false;

            TorneoPartida partida = (TorneoPartida)obj;

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
            
            if (Iniciada != partida.Iniciada)
                return false;
            
            if (Terminada != partida.Terminada)
                return false;
            
            return true;
        }
    }
}