using MatchingGame.Server.Helpers;
using MatchingGame.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Hubs
{
    public class PartidaHub : Hub
    {
        private GameManager manejador { get; set; }
        private Partida partida;

        public PartidaHub(GameManager manejador)
        {
            this.manejador = manejador;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var partida = manejador.BuscarPartidaPorJugadorId(Context.ConnectionId);

            if(partida != null)
            {
                if (partida.JugadorUnoId == Context.ConnectionId)
                    await Clients.Client(partida.JugadorDosId).SendAsync("JugadorDesconectado", partida.JugadorUnoNombre);

                else if (partida.JugadorDosId == Context.ConnectionId)
                    await Clients.Client(partida.JugadorUnoId).SendAsync("JugadorDesconectado", partida.JugadorDosNombre);

                manejador.EliminarPartida(partida);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task EstablecerPartida(int id, string usuario)
        {
            var partida = manejador.EstablecerPartida(id, Context.ConnectionId, usuario);

            if(partida != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("RecibirPartida", true, partida.JugadorUnoNombre, partida.JugadorDosNombre);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("RecibirPartida", false, " ", " ");
            }
        }

        public async Task IniciarPartida(int id, string usuario)
        {
            var partida = manejador.IniciarPartida(id, Context.ConnectionId);
            
            if(partida != null)
            {
                await Clients.Client(partida.JugadorUnoId).SendAsync("IniciarPartida", partida.Iniciada);
                await Clients.Client(partida.JugadorDosId).SendAsync("IniciarPartida", partida.Iniciada);
            }
        }

        public async Task RecibirParejaEncontrada(int id)
        {
            partida = manejador.MarcarParejaEncontrada(id, Context.ConnectionId);

            if(partida != null)
            {
                string jugador;
                int parejas;

                if (partida.JugadorUnoId == Context.ConnectionId)
                {
                    jugador = partida.JugadorUnoNombre;
                    parejas = partida.GetAnimalesJugadorUno();
                }
                else
                {
                    jugador = partida.JugadorDosNombre;
                    parejas = partida.GetAnimalesJugadorDos();
                }

                Task j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUnoId).SendAsync("RecibirMovimiento", jugador, parejas));
                Task j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDosId).SendAsync("RecibirMovimiento", jugador, parejas));

                Task.WaitAll(new[] { j1, j2 });

                //Clients.Client(partida.JugadorUnoId).SendAsync("RecibirMovimiento", jugador, parejas);
                //Clients.Client(partida.JugadorDosId).SendAsync("RecibirMovimiento", jugador, parejas);

                partida.Terminar();
            }
        }
    }
}
