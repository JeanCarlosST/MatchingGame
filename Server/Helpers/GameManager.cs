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

        public bool AgregarPeticion(Peticion peticion)
        {
            if (Peticiones.Find(p => p.Jugador.Id == peticion.Jugador.Id) != null)
                return false;

            Peticion otraPeticion = Peticiones.Find(p => p.Descripcion == peticion.Descripcion && p.Jugador.Id != peticion.Jugador.Id);

            if(otraPeticion != null)
            {
                Peticiones.Remove(otraPeticion);
                Partida partida = new Partida()
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
