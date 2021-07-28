using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingGame.Server.DAL;
using MatchingGame.Shared;

namespace MatchingGame.Server.Services
{
    public class PlayerService 
    {
        private readonly Context context;
        private readonly IJwtUtils jwtUtils;

        public PlayerService(Context contexto, IJwtUtils jwtUtils)
        {
            context = contexto;
            this.jwtUtils = jwtUtils;
        }

        public void GetRecordRanking(Modo modo, Dificultad dificultad)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GetUserRanking()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GuardarPartidaSolo(Partida partida, string jwtToken)
        {
            bool guardo = false;           

            try
            {
                int? id = jwtUtils.ValidarJWTToken(jwtToken);

                if (id != null)
                {
                    context.Partida.Add(ConvertToEntity(partida));
                    guardo = context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return guardo;
        }

        public bool GuardarPartida1v1(Partida partida, string jwtToken)
        {
            bool guardo = false;

            try
            {
                int? id = jwtUtils.ValidarJWTToken(jwtToken);

                if (id != null)
                {
                    context.Partida.Add(ConvertToEntity1v1(partida));
                    guardo = context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return guardo;
        }

        public void GetPlayerStats(string jwtToken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GetPlayerFriends(string jwtToken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GetPlayerRequests(string jwtToken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CrearTorneo(Torneo torneo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GuardarPartidaTorneo(TorneoPartida torneoPartida)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CerrarTorneo(Torneo torneo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }        

        private Entities.Partida ConvertToEntity(Shared.Partida partida)
        {
            var entidadPartida = new Entities.Partida()
            {
                Fecha = partida.Fecha,
                TipoId = 1,
                ModoId = Convert.ToInt32(partida.Modo) + 1,
                DificultadId = Convert.ToInt32(partida.Dificultad) + 1,
                PartidaJugadores = new HashSet<Entities.PartidaJugador>()
                {
                    new Entities.PartidaJugador()
                    {
                        JugadorId = partida.JugadorUno.UsuarioId,
                        Puntos = partida.JugadorUnoDetalle.Puntos,
                        Tiempo = partida.JugadorUnoDetalle.Tiempo
                    }
                },
                GanadorId = partida.JugadorUno.UsuarioId

            };

            return entidadPartida;
        }

        private Entities.Partida ConvertToEntity1v1(Shared.Partida partida)
        {
            var entidadPartida = new Entities.Partida()
            {
                Fecha = partida.Fecha,
                TipoId = 1,
                ModoId = Convert.ToInt32(partida.Modo) + 1,
                DificultadId = Convert.ToInt32(partida.Dificultad) + 1,
                PartidaJugadores = new HashSet<Entities.PartidaJugador>()
                {
                    new Entities.PartidaJugador()
                    {
                        JugadorId = partida.JugadorUno.UsuarioId,
                        Puntos = partida.JugadorUnoDetalle.Puntos,
                        Tiempo = partida.JugadorUnoDetalle.Tiempo
                    },
                    new Entities.PartidaJugador()
                    {
                        JugadorId = partida.JugadorDos.UsuarioId,
                        Puntos = partida.JugadorDosDetalle.Puntos,
                        Tiempo = partida.JugadorDosDetalle.Tiempo
                    }
                },
                GanadorId = partida.JugadorDosDetalle.Puntos > partida.JugadorUnoDetalle.Puntos 
                            ? partida.JugadorDos.UsuarioId 
                            : partida.JugadorUno.UsuarioId

            };

            return entidadPartida;
        }
    }
}
