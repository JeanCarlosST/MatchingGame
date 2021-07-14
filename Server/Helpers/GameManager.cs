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
            if (Peticiones.Find(p => p.UsuarioId == peticion.UsuarioId) != null)
                return false;

            Peticion otraPeticion = Peticiones.Find(p => p.Descripcion == peticion.Descripcion && p.UsuarioId != peticion.UsuarioId);

            if(otraPeticion != null)
            {
                Peticiones.Remove(otraPeticion);
                Partida partida = new Partida()
                {
                    PartidaId = Partidas.Count + 1,
                    JugadorUnoId = otraPeticion.UsuarioId,
                    JugadorUnoNombre = otraPeticion.Nombre,
                    JugadorDosId = peticion.UsuarioId,
                    JugadorDosNombre = peticion.Nombre
                };
                Partidas.Add(partida);
            }
            else
                Peticiones.Add(peticion);

            return true;
        }

        public Partida EstablecerPartida(int id, string jugadorId, string jugadorNombre)
        {
            var partida = Partidas.Find(
                p => p.PartidaId == id
            );

            if(partida != null)
            {
                if (partida.JugadorUnoNombre == jugadorNombre)
                {
                    partida.JugadorUnoId = jugadorId;
                }
                else if (partida.JugadorDosNombre == jugadorNombre)
                {
                    partida.JugadorDosId = jugadorId;
                }
            }

            return partida;
        }

        public Partida BuscarPartidaPorJugadorId(string connectionId)
        {
            foreach (var partida in Partidas)
            {
                if (partida.JugadorUnoId == connectionId || partida.JugadorDosId == connectionId)
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

        public void EliminarPeticionDeUsuario(string connectionId)
        {
            foreach(var peticion in Peticiones)
            {
                if(peticion.UsuarioId == connectionId)
                {
                    Peticiones.Remove(peticion);
                    break;
                }
            }
        }

        public Partida EncontrarPartida(Peticion peticion)
        {   
            return Partidas.Find(p => p.JugadorUnoId == peticion.UsuarioId || 
                                      p.JugadorDosId == peticion.UsuarioId);
        }

        public Partida IniciarPartida(int id, string jugadorId)
        {
            var partida = Partidas.Find(
                p => p.PartidaId == id
            );

            if (partida != null && !partida.Iniciada)
            {
                partida.Terminada = false;

                if (partida.JugadorUnoId == jugadorId)
                {
                    partida.SumarAnimalesJugadorUno();
                }
                else if (partida.JugadorDosId == jugadorId)
                {
                    partida.SumarAnimalesJugadorDos();
                }

                if (partida.GetAnimalesJugadorUno() == 0 && partida.GetAnimalesJugadorDos() == 0)
                    partida.Iniciada = true;
            }
            return partida;
        }

        public Partida MarcarParejaEncontrada(int id, string jugadorId, string jugadorNombre)
        {
            var partida = Partidas.Find(
                p => p.PartidaId == id
            );

            if(partida != null && !partida.Terminada)
            {
                if (partida.JugadorUnoId == jugadorId && partida.JugadorUnoNombre == jugadorNombre)
                    partida.SumarAnimalesJugadorUno();
                else if (partida.JugadorDosId == jugadorId && partida.JugadorDosNombre == jugadorNombre)
                    partida.SumarAnimalesJugadorDos();
            }

            return partida;
        }
    }
}
