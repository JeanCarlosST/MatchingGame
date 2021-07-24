using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Partida
    {
        public int PartidaId { get; set; }
        public Jugador JugadorUno { get; set; }
        public Jugador JugadorDos { get; set; }
        public PartidaJugadorDetalle JugadorUnoDetalle { get; set; }
        public PartidaJugadorDetalle JugadorDosDetalle { get; set; }
        public Modo Modo { get; set; }
        public Dificultad Dificultad { get; set; }
        public int ParesTotales { get; set; }
        public bool Iniciada { get; set; }
        public bool Terminada { get; set; }
        public DateTime Fecha { get; set; }

        public Partida(Modo modo, Dificultad dificultad)
        {
            this.Modo = modo;
            this.Dificultad = dificultad;
            this.ParesTotales = Emojis.ObtenerParesTotales(dificultad);
            JugadorUnoDetalle = new();
            JugadorDosDetalle = new();
        }
    }

    public class PartidaJugadorDetalle
    {
        public int ParesEncontrados { get; set; }
        public float Tiempo { get; set; }
        public int Puntos { get; set; }
    }
        //public int PartidaId { get; set; }
        //public Jugador1v1 JugadorUno { get; set; } = new Jugador1v1();
        //public Jugador1v1 JugadorDos { get; set; } = new Jugador1v1();
        //public bool Iniciada = false;
        //public bool Terminada = false;

        //public void JugadorUnoParEncontrado()
        //{
        //    JugadorUno.ParesEncontrados++;
        //    JugadorUno.terminado = JugadorUno.ParesEncontrados == JugadorUno.ParesTotal;
        //}

        //public void JugadorDosParEncontrado()
        //{
        //    JugadorDos.ParesEncontrados++;
        //    JugadorDos.terminado = JugadorDos.ParesEncontrados == JugadorDos.ParesTotal;
        //}

        //public int ObtenerJugadorUnoPares()
        //{
        //    return JugadorUno.ParesEncontrados;
        //}

        //public int ObtenerJugadorDosPares()
        //{
        //    return JugadorDos.ParesEncontrados;
        //}

        //public void Terminar()
        //{
        //    Terminada = JugadorUno.terminado && JugadorDos.terminado;
        //    if(Terminada)
        //    {
        //        Iniciada = false;
        //        JugadorUno.listo = false;
        //        JugadorDos.listo = false;

        //        if (JugadorUno.Puntos > JugadorDos.Puntos)
        //            JugadorUno.Victorias++;
        //        else
        //            JugadorDos.Victorias++;
        //    }
        //}

}
