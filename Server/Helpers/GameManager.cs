using MatchingGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Helpers
{
    public class GameManager
    {
        private List<Peticion> Peticiones { get; set; } = new();
        private List<Partida> Partidas { get; set; } = new();
        private List<Torneo> Torneos { get; set; } = new();

        public static (Modo modo, Dificultad dificultad) ObtenerModoYDificultad(string descripcion)
        {
            int index = descripcion.IndexOf("_");
            string modoStr = descripcion.Substring(0, index);
            string dificultadStr = descripcion.Substring(index);
            Modo modo = (Modo)Enum.Parse(typeof(Modo), modoStr);
            Dificultad dificultad = (Dificultad)Enum.Parse(typeof(Dificultad), dificultadStr);

            return (modo, dificultad);
        }

        public bool AgregarPeticion(Peticion peticion)
        {
            if (Peticiones.Find(p => p.Jugador.ConnectionId == peticion.Jugador.ConnectionId) != null)
                return false;

            if(peticion.Descripcion.Contains("partida"))
            {
                Peticion otraPeticion = 
                    Peticiones.Find(p => 
                        p.Descripcion == peticion.Descripcion && p.Jugador.ConnectionId != peticion.Jugador.ConnectionId);

                if(otraPeticion != null)
                {
                    var (modo, dificultad) = ObtenerModoYDificultad(peticion.Descripcion);
                    Peticiones.Remove(otraPeticion);
                    Partida partida = new Partida(modo, dificultad)
                    {
                        PartidaId = Partidas.Count + 1,
                        JugadorUno = otraPeticion.Jugador,
                        JugadorDos = peticion.Jugador
                    };
                    Partidas.Add(partida);
                }
                else
                    Peticiones.Add(peticion);
            
                return true;
            }
            else if (peticion.Descripcion.Contains("torneo"))
            {
                string cantStr = peticion.Descripcion.Replace("torneo", "");
                int cantJug = Convert.ToInt32(cantStr);

                Torneo torneo = BuscarTorneoDisponible(cantJug);

                if (torneo == null) 
                {
                    torneo = new Torneo(cantJug);
                    Torneos.Add(torneo);
                }

                torneo.AgregarJugador(peticion.Jugador);

                if (torneo.CantMaxJugadores == torneo.Jugadores.Count)
                    torneo.Iniciar();

                return true;
            }

            return false;
        }

        public Torneo BuscarTorneoDisponible(int cantJug)
        {
            foreach(var torneo in Torneos)
            {
                if (!torneo.Terminado && torneo.CantMaxJugadores == cantJug &&
                    torneo.CantMaxJugadores < torneo.Jugadores.Count)
                    return torneo;
            }
            return null;
        }

        public Torneo EncontrarTorneo(Peticion peticion)
        {
            throw new NotImplementedException();
        }

        public Partida BuscarPartidaPorJugadorId(string connectionId)
        {
            foreach (var partida in Partidas)
            {
                if (partida.JugadorUno.Id == connectionId || partida.JugadorDos.Id == connectionId)
                {
                    return partida;
                }
            }
            return null;
        }

        public TorneoPartida ActualizarDatosPartida(TorneoPartida partida, Jugador jugador, PartidaJugadorDetalle jugDetalle)
        {
            throw new NotImplementedException();
        }

        public void TerminarPartidaTorneo(TorneoPartida partida)
        {
            throw new NotImplementedException();
        }

        public void EliminarPartida(Partida partida)
        {
            Partidas.Remove(partida);    
        }

        public Peticion BuscarPeticionPorJugadorId(string connectionId)
        {
            foreach (var peticion in Peticiones)
            {
                if (peticion.Jugador.Id == connectionId)
                {
                    return peticion;
                }
            }
            return null;
        }

        public void EliminarPeticion(Peticion peticion)
        {
            Peticiones.Remove(peticion);
        }

        public Partida EncontrarPartida(Peticion peticion)
        {   
            return Partidas.Find(p => p.JugadorUno.Id == peticion.Jugador.Id || 
                                      p.JugadorDos.Id == peticion.Jugador.Id);
        }

        internal Torneo BuscarTorneoPorPartida(TorneoPartida partida)
        {
            throw new NotImplementedException();
        }

        public Partida MarcarPartidaListo(int id, Jugador1v1 jugador)
        {
            var partida = Partidas.Find(
                p => p.PartidaId == id
            );

            if (partida != null && !partida.Iniciada)
            {
                partida.Terminada = false;

                if (partida.JugadorUno.Id == jugador.Id && partida.JugadorUno.Nickname == jugador.Nickname)
                {
                    partida.JugadorUno.listo = true;
                    partida.JugadorUno.terminado = false;
                }
                else if (partida.JugadorDos.Id == jugador.Id && partida.JugadorDos.Nickname == jugador.Nickname)
                {
                    partida.JugadorDos.listo = true;
                    partida.JugadorDos.terminado = false;
                }

                if (partida.JugadorUno.listo && partida.JugadorDos.listo)
                    partida.Iniciada = true;
            }
            return partida;
        }

        public Partida MarcarParejaEncontrada(int id, Jugador1v1 jugador)
        {
            var partida = Partidas.Find(
                p => p.PartidaId == id
            );

            if(partida != null && !partida.Terminada)
            {
                if (partida.JugadorUno.Id == jugador.Id && partida.JugadorUno.Nickname == jugador.Nickname)
                { 
                    partida.JugadorUnoParEncontrado();
                    partida.JugadorUno = jugador;
                }
                else if (partida.JugadorDos.Id == jugador.Id && partida.JugadorDos.Nickname == jugador.Nickname)
                {
                    partida.JugadorDosParEncontrado();
                    partida.JugadorDos = jugador;
                }
            }

            return partida;
        }
    }
}
