using MatchingGame.Server.Helpers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Hubs
{
    public class PeticionHub : Hub
    {
        public GameManager manejador { get; set; }

        public PeticionHub(GameManager manejador)
        {
            this.manejador = manejador;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            manejador.EliminarPeticionDeUsuario(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task RecibirPeticion(string nombre, string peticionStr)
        {
            Peticion peticion = new Peticion() 
            { 
                UsuarioId = Context.ConnectionId, 
                Nombre = nombre,
                Descripcion = peticionStr
            };

            if (!manejador.AgregarPeticion(peticion))
                return;

            var partida = manejador.EncontrarPartida(peticion);

            if(partida != null)
            {
                await Clients.Client(partida.JugadorUnoId).SendAsync("RecibirPartida", partida.PartidaId);
                await Clients.Client(partida.JugadorDosId).SendAsync("RecibirPartida", partida.PartidaId);
            }
        }
    }
}
