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
            descripcion = descripcion.Replace("partida", "");
            int index = descripcion.IndexOf("_");
            string modoStr = descripcion.Substring(0, index);
            string dificultadStr = descripcion.Substring(index);
            Modo modo = (Modo)Enum.Parse(typeof(Modo), modoStr);
            Dificultad dificultad = (Dificultad)Enum.Parse(typeof(Dificultad), dificultadStr);

            return (modo, dificultad);
        }

        public bool AgregarPeticion(Peticion peticion)
        {
            if (ExistePeticion(peticion))
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
                int cantJug = ObtenerCantidadJugadores(peticion.Descripcion);
                if (cantJug != 4 && cantJug != 8 && cantJug != 16)
                    return false;

                Torneo torneo = BuscarTorneoDisponible(cantJug);

                if (torneo == null) 
                {
                    torneo = new Torneo(cantJug);
                    Torneos.Add(torneo);
                }

                Jugador jugador = peticion.Jugador;

                if(!torneo.JugadorExiste(jugador))
                {
                    torneo.AgregarJugador(peticion.Jugador);

                    if (torneo.CantMaxJugadores == torneo.Jugadores.Count)
                        torneo.Iniciar();
    
                    return true;
                }
            }

            return false;
        }

        private bool ExistePeticion(Peticion peticion)
        {
            bool existe = Peticiones.Find(p => p.Jugador.ConnectionId == peticion.Jugador.ConnectionId) != null;

            return existe;
        }

        private static int ObtenerCantidadJugadores(string descripcion)
        {
            string cantStr = descripcion.Replace("torneo", "");
            return Convert.ToInt32(cantStr);
        }

        private Torneo BuscarTorneoDisponible(int cantJug)
        {
            foreach(var torneo in Torneos)
            {
                if (!torneo.Iniciado && torneo.CantMaxJugadores == cantJug && torneo.Jugadores.Count < torneo.CantMaxJugadores)
                    return torneo;
            }
            return null;
        }

        public Torneo EncontrarTorneo(Peticion peticion)
        {
            foreach(var torneo in Torneos)
            {
                if (!torneo.Terminado && torneo.Jugadores.Contains(peticion.Jugador))
                    return torneo;
            }

            return null;
        }

        public Partida BuscarPartidaPorJugadorId(string connectionId)
        {
            foreach (var partida in Partidas)
            {
                if (partida.JugadorUno.ConnectionId == connectionId || partida.JugadorDos.ConnectionId == connectionId)
                {
                    return partida;
                }
            }
            return null;
        }

        public TorneoPartida ActualizarDatosPartida(TorneoPartida partida, Jugador jugador, PartidaJugadorDetalle jugDetalle)
        {
            Torneo torneo = BuscarTorneoPorPartida(partida);

            if (torneo != null && partida.Iniciada && !partida.Terminada)
            {
                partida = torneo.Partidas.Find(p => p.Equals(partida));

                if (partida.JugadorUno.Nickname == jugador.Nickname && partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                    partida.JugadorUnoDetalle = jugDetalle;

                else if (partida.JugadorDos.Nickname == jugador.Nickname && partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                    partida.JugadorDosDetalle = jugDetalle;

                return partida;
            }
            else
                return null;

        }

        public void TerminarPartidaTorneo(TorneoPartida partida)
        {
            Torneo torneo = BuscarTorneoPorPartida(partida);

            partida = torneo.Partidas.Find(p => p.Equals(partida));

            Jugador ganador = partida.Terminar();
            if (ganador != null)
            {
                torneo.JugadoresSigteRonda.Add(ganador);
                torneo.VerificarPaseDeRonda();
                bool terminado = torneo.Terminado;
            }
        }

        public void EliminarPartida(Partida partida)
        {
            Partidas.Remove(partida);    
        }

        public Peticion BuscarPeticionPorJugadorId(string connectionId)
        {
            foreach (var peticion in Peticiones)
            {
                if (peticion.Jugador.ConnectionId == connectionId)
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

        public void EliminarPeticion(string connectionId)
        {
            Peticion peticion = BuscarPeticionPorJugadorId(connectionId);
            if (peticion != null)
                EliminarPeticion(peticion);
        }

        public Partida EncontrarPartida(Peticion peticion)
        {   
            return Partidas.Find(p => p.JugadorUno.ConnectionId == peticion.Jugador.ConnectionId || 
                                      p.JugadorDos.ConnectionId == peticion.Jugador.ConnectionId);
        }

        public Torneo BuscarTorneoPorPartida(TorneoPartida partida)
        {
            foreach(var torneo in Torneos)
            {
                if (torneo.Partidas.Contains(partida))
                    return torneo;
            }

            return null;
        }

        public Partida PerderPartida(Partida partida, Jugador jugador)
        {
            partida = Partidas.Find(p => p.Equals(partida));

            if (partida != null)
            {
                if (partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                    partida.JugadorDosDetalle.Puntos = -1;

                else if (partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                    partida.JugadorUnoDetalle.Puntos = -1;
            }
            
            return partida;
        }

        public Partida MarcarParejaEncontrada(Partida partida, Jugador jugador, PartidaJugadorDetalle jugDetalle)
        {
            if (!partida.Iniciada || partida.Terminada)
                return null;

            partida = Partidas.Find(p => p.Equals(partida));

            if (partida.JugadorUno.Nickname == jugador.Nickname && partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                partida.JugadorUnoDetalle = jugDetalle;
            
            else if (partida.JugadorDos.Nickname == jugador.Nickname && partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                partida.JugadorDosDetalle = jugDetalle;
            
            return partida;

            //var partida = Partidas.Find(
            //    p => p.PartidaId == id
            //);

            //if(partida != null && !partida.Terminada)
            //{
            //    if (partida.JugadorUno.Id == jugador.Id && partida.JugadorUno.Nickname == jugador.Nickname)
            //    { 
            //        partida.JugadorUnoParEncontrado();
            //        partida.JugadorUno = jugador;
            //    }
            //    else if (partida.JugadorDos.Id == jugador.Id && partida.JugadorDos.Nickname == jugador.Nickname)
            //    {
            //        partida.JugadorDosParEncontrado();
            //        partida.JugadorDos = jugador;
            //    }
            //}

            //return partida;
        }
    }
}
