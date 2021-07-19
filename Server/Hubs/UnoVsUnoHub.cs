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

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var peticion = Manejador.BuscarPeticionPorJugadorId(Context.ConnectionId);

            if (peticion != null)
                Manejador.EliminarPeticion(peticion);

            var partida = Manejador.BuscarPartidaPorJugadorId(Context.ConnectionId);

            if (partida != null)
            {
                if (partida.JugadorUno.Id == Context.ConnectionId)
                    await Clients.Client(partida.JugadorDos.Id).SendAsync("JugadorDesconectado");

                else if (partida.JugadorDos.Id == Context.ConnectionId)
                    await Clients.Client(partida.JugadorUno.Id).SendAsync("JugadorDesconectado");

                Manejador.EliminarPartida(partida);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task RecibirPeticion(Jugador1v1 jugador, string peticionStr)
        {
            Peticion peticion = new Peticion(jugador, Context.ConnectionId, peticionStr);

            if (!Manejador.AgregarPeticion(peticion))
                return;

            var partida = Manejador.EncontrarPartida(peticion);

            if (partida != null)
            {
                await Clients.Client(partida.JugadorUno.Id).SendAsync("RecibirPartida", partida.PartidaId, partida.JugadorUno, partida.JugadorDos);
                await Clients.Client(partida.JugadorDos.Id).SendAsync("RecibirPartida", partida.PartidaId, partida.JugadorUno, partida.JugadorDos);
            }
        }

        public async Task MarcarPartidaListo(int id, Jugador1v1 jugador)
        {
            Partida partida;

            if (jugador.Id != Context.ConnectionId)
                return;

            partida = Manejador.MarcarPartidaListo(id, jugador);

            if (partida != null)
            {
                await Clients.Client(partida.JugadorUno.Id).SendAsync("IniciarPartida", partida.Iniciada);
                await Clients.Client(partida.JugadorDos.Id).SendAsync("IniciarPartida", partida.Iniciada);
            }
        }

        public void RecibirParejaEncontrada(int id, Jugador1v1 jugador)
        {
            Partida partida;

            if (jugador.Id != Context.ConnectionId)
                return;
            
            partida = Manejador.MarcarParejaEncontrada(id, jugador);

            if (partida != null)
            {
                Task j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.Id).SendAsync("RecibirMovimiento", partida.JugadorUno, partida.JugadorDos));
                Task j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.Id).SendAsync("RecibirMovimiento", partida.JugadorUno, partida.JugadorDos));

                Task.WaitAll(new[] { j1, j2 });

                partida.Terminar();
                if(partida.Terminada)
                {
                    j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.Id).SendAsync("RecibirGanador", partida.JugadorUno, partida.JugadorDos));
                    j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.Id).SendAsync("RecibirGanador", partida.JugadorUno, partida.JugadorDos));

                    Task.WaitAll(new[] { j1, j2 });
                }
            }
        }
    }
}
