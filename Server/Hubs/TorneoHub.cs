using MatchingGame.Server.Helpers;
using MatchingGame.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Hubs
{
    public class TorneoHub : Hub
    {
        private GameManager Manejador { get; set; }

        public TorneoHub(GameManager manager)
        {
            this.Manejador = manager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var torneo = Manejador.MarcarJugadorAusente(Context.ConnectionId);

            if(torneo != null && !torneo.Iniciado)
            {
                foreach (var jug in torneo.Jugadores)
                    await Clients.Client(jug.ConnectionId).SendAsync("RecibirJugadores", torneo.Jugadores);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void JugadorAbandonado()
        {
            Manejador.MarcarJugadorAbandonado(Context.ConnectionId);
        }

        public async Task RecibirPeticion(TorneoJugador jugador, string peticionStr)
        {
            Peticion peticion = new Peticion(jugador, Context.ConnectionId, peticionStr);
            
            if (!Manejador.AgregarPeticion(peticion))
                return;

            var torneo = Manejador.EncontrarTorneo(peticion);

            if(torneo != null)
            {
                foreach (var jug in torneo.Jugadores)
                    await Clients.Client(jug.ConnectionId).SendAsync("RecibirJugadores", torneo.Jugadores);
                
                if(torneo.Iniciado)
                {
                    foreach (var jug in torneo.Jugadores)
                        await Clients.Client(jug.ConnectionId).SendAsync("IniciarTorneo", torneo.Partidas);
                }
            }
        }

        public void RecibirParejaEncontrada(TorneoPartida partida, Jugador jugador, PartidaJugadorDetalle jugDetalle)
        {
            if (jugador.ConnectionId != Context.ConnectionId)
                return;

            partida = Manejador.ActualizarDatosPartida(partida, jugador, jugDetalle);

            if(partida != null)
            {
                Manejador.TerminarPartidaTorneo(partida);
                EnviarPartidaAMetodo(partida, "RecibirMovimiento");

                //Task j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("RecibirMovimiento", partida));
                //Task j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("RecibirMovimiento", partida));

                //Task.WaitAll(new[] { j1, j2 });
                
                if(partida.Terminada)
                {
                    //EnviarPartidaAMetodo(partida, "RecibirMovimiento");
                    //j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.ConnectionId).SendAsync("RecibirMovimiento", partida));
                    //j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.ConnectionId).SendAsync("RecibirMovimiento", partida));

                    //Task.WaitAll(new[] { j1, j2 });
                }
            }
        }

        private void EnviarPartidaAMetodo(TorneoPartida partida, string metodo)
        {
            Task j1 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorUno.ConnectionId).SendAsync(metodo, partida));
            Task j2 = Task.Factory.StartNew(() => Clients.Client(partida.JugadorDos.ConnectionId).SendAsync(metodo, partida));

            Task.WaitAll(new[] { j1, j2 });
        }

        public async Task ActualizarPartidas(TorneoPartida partida)
        {
            Torneo torneo = Manejador.BuscarTorneoPorPartida(partida);

            foreach(var jugador in torneo.Jugadores)
            {
                await Clients.Client(jugador.ConnectionId).SendAsync("ActualizarPartidas", torneo.Partidas);
                await Clients.Client(jugador.ConnectionId).SendAsync("RefrescarTorneo");

            }
        }
    }
}
