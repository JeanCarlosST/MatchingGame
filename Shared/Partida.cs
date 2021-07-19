using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Partida
    {
        public int PartidaId { get; set; }
        public Jugador1v1 JugadorUno { get; set; } = new Jugador1v1();
        public Jugador1v1 JugadorDos { get; set; } = new Jugador1v1();
        public bool Iniciada = false;
        public bool Terminada = false;

        public void JugadorUnoParEncontrado()
        {
            JugadorUno.ParesEncontrados++;
            JugadorUno.terminado = JugadorUno.ParesEncontrados == JugadorUno.ParesTotal;
        }

        public void JugadorDosParEncontrado()
        {
            JugadorDos.ParesEncontrados++;
            JugadorDos.terminado = JugadorDos.ParesEncontrados == JugadorDos.ParesTotal;
        }

        public int ObtenerJugadorUnoPares()
        {
            return JugadorUno.ParesEncontrados;
        }

        public int ObtenerJugadorDosPares()
        {
            return JugadorDos.ParesEncontrados;
        }

        public void Terminar()
        {
            Terminada = JugadorUno.terminado && JugadorDos.terminado;
            if(Terminada)
            {
                Iniciada = false;
                JugadorUno.listo = false;
                JugadorDos.listo = false;

                if (JugadorUno.Puntos > JugadorDos.Puntos)
                    JugadorUno.Victorias++;
                else
                    JugadorDos.Victorias++;
            }
        }

    }
}
