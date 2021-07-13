using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Partida
    {
        public int PartidaId { get; set; }
        public string JugadorUnoId { get; set; }
        public string JugadorUnoNombre { get; set; }
        public string JugadorDosId { get; set; }
        public string JugadorDosNombre { get; set; }
        private int animalesJugadorUno { get; set; } = -1;
        private int animalesJugadorDos { get; set; } = -1;
        public bool Iniciada = false;
        public bool Terminada = false;

        public void SumarAnimalesJugadorUno()
        {
            animalesJugadorUno++;
        }

        public void SumarAnimalesJugadorDos()
        {
            animalesJugadorDos++;
        }

        public int GetAnimalesJugadorUno()
        {
            return animalesJugadorUno;
        }

        public int GetAnimalesJugadorDos()
        {
            return animalesJugadorDos;
        }

        public void Terminar()
        {
            Terminada = animalesJugadorUno == 8 || animalesJugadorDos == 8;
            if(Terminada)
            {
                Iniciada = false;
                animalesJugadorUno = -1;
                animalesJugadorDos = -1;
            }
        }

    }
}
