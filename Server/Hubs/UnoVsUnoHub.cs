using MatchingGame.Server.Helpers;
using MatchingGame.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Hubs
{
    public class UnoVsUnoHub : Hub
    {
        private GameManager Manejador { get; set; }

        public UnoVsUnoHub(GameManager manager)
        {
            this.Manejador = manager;
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var peticion = Manejador.BuscarPeticionPorJugadorId(Context.ConnectionId);

            if (peticion != null)
                Manejador.EliminarPeticion(peticion);

            var partida = Manejador.BuscarPartidaPorJugadorId(Context.ConnectionId);

            if (partida != null)
            {
                if (partida.JugadorUno.ConnectionId == Context.ConnectionId)
                    await Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("JugadorDesconectado");

                else if (partida.JugadorDos.ConnectionId == Context.ConnectionId)
                    await Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("JugadorDesconectado");

                Manejador.EliminarPartida(partida);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void RecibirPeticion(Jugador jugador, string peticionStr)
        {
            Peticion peticion = new Peticion(jugador, Context.ConnectionId, peticionStr);

            if (Manejador.AgregarPeticion(peticion))
            {
                Partida partida = Manejador.EncontrarPartida(peticion);
                if (partida != null)
                {
                    partida.Iniciada = true;
                    EnviarPartidaAMetodo(partida, "RecibirPartida");
                }
            }
        }

        public void RecibirParejaEncontrada(Partida partida, Jugador jugador, PartidaJugadorDetalle jugDetalle)
        {
            if (jugador.ConnectionId == Context.ConnectionId)
            {
                partida = Manejador.MarcarParejaEncontrada(partida, jugador, jugDetalle);

                if (partida != null)
                {
                    partida.Terminar();
                    //EnviarPartidaAMetodo(partida, "RecibirMovimiento");

                    EnviarPartidaAMetodo(partida, "RecibirMovimiento");
                    //if (partida.Terminada)
                }
            }
        }

        public void PerderPartida(Partida partida, Jugador jugador)
        {
            if (jugador.ConnectionId == base.Context.ConnectionId)
            {
                partida = Manejador.BuscarPartidaPorJugadorId(jugador.ConnectionId);
                partida = Manejador.PerderPartida(partida, jugador);

                if (partida != null)
                {
                    EnviarPartidaAMetodo(partida, "RecibirMovimiento");
                    partida.Terminar();
                    
                    if (partida.Terminada)
                        EnviarPartidaAMetodo(partida, "RecibirMovimiento");
                    
                }
            }
        }

        private void EnviarPartidaAMetodo(Partida partida, string metodo)
        {
            Task j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.ConnectionId).SendAsync(metodo, partida));
            Task j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.ConnectionId).SendAsync(metodo, partida));

            Task.WaitAll(new[] { j1, j2 });
        }

        private async Task JugadorIniciaPartida(Jugador jugador)
        {
            if (jugador.ConnectionId == Context.ConnectionId)
            {
                var partida = Manejador.BuscarPartidaPorJugadorId(jugador.ConnectionId);
                if (partida != null)
                {
                    if (partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                        await Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("OponenteJugando");

                    else if (partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                        await Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("OponenteJugando");
                }
            }
        }

        public async Task RecibirPeticionCambioPartida(Partida partida, Jugador jugador, string peticionCambio)
        {
            if (jugador.ConnectionId == Context.ConnectionId)
            {
                partida = Manejador.BuscarPartidaPorJugadorId(jugador.ConnectionId);
                if (partida != null)
                {
                    if (partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                        await Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("RecibirPeticionCambio", peticionCambio);
                    
                    else if (partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                        await Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("RecibirPeticionCambio", peticionCambio);
                }
            }
        }

        public async Task RecibirRespuestaCambioPartida(Partida partida, Jugador jugador, bool cambio, Modo modo, Dificultad dificultad)
        {
            if (jugador.ConnectionId == Context.ConnectionId)
            {
                partida = Manejador.BuscarPartidaPorJugadorId(jugador.ConnectionId);
                if (partida != null)
                {
                    if (cambio)
                    {
                        partida.Modo = modo;
                        partida.Dificultad = dificultad;
                    }

                    if (partida.JugadorUno.ConnectionId == jugador.ConnectionId)
                    {
                       await Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("RespuestaPeticionCambio", cambio);
                    }
                    else if (partida.JugadorDos.ConnectionId == jugador.ConnectionId)
                    {
                        await Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("RespuestaPeticionCambio", cambio);
                    }
                }
            }
        }

        public void EliminarPeticionDeUsuario(string connectionId)
        {
            if (connectionId == Context.ConnectionId)
            {
                Manejador.EliminarPeticion(connectionId);
            }
        }

    }
}
