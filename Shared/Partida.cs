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

        public Jugador Terminar()
        {
            Terminada = 
                ((JugadorUnoDetalle.ParesEncontrados == ParesTotales) && 
                (JugadorDosDetalle.ParesEncontrados == ParesTotales)) || 
                ((JugadorUnoDetalle.Puntos == -1) && (JugadorDosDetalle.ParesEncontrados == ParesTotales)) || 
                ((JugadorDosDetalle.Puntos == -1) && (JugadorUnoDetalle.ParesEncontrados == ParesTotales)) || 
                ((JugadorUnoDetalle.Puntos == -1) && (JugadorDosDetalle.Puntos == -1));

            if(Terminada)
            {
                if(Modo == Modo.Contrarreloj)
                {
                    if (JugadorUnoDetalle.Tiempo > JugadorDosDetalle.Tiempo)
                    {
                        JugadorUno.Victorias++;
                        return JugadorUno;
                    }
                    else if (JugadorUnoDetalle.Tiempo == JugadorDosDetalle.Tiempo)
                    {
                        if (JugadorUnoDetalle.Puntos > JugadorDosDetalle.Puntos)
                        {
                            JugadorUno.Victorias++;
                            return JugadorUno;
                        }    
                    }

                    JugadorDos.Victorias++;
                    return JugadorDos;
                }
                else
                {
                    if (JugadorUnoDetalle.Tiempo < JugadorDosDetalle.Tiempo)
                    {
                        JugadorUno.Victorias++;
                        return JugadorUno;
                    }
                    else if (JugadorUnoDetalle.Tiempo == JugadorDosDetalle.Tiempo)
                    {
                        if (JugadorUnoDetalle.Puntos > JugadorDosDetalle.Puntos)
                        {
                            JugadorUno.Victorias++;
                            return JugadorUno;
                        }
                    }

                    JugadorDos.Victorias++;
                    return JugadorDos;
                }
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Partida))
                return false;

            Partida partida = (Partida)obj;

            if (!JugadorUno.Equals(partida.JugadorUno))
                return false;

            if (!JugadorDos.Equals(partida.JugadorDos))
                return false;

            if (Modo != partida.Modo)
                return false;

            if (Dificultad != partida.Dificultad)
                return false;

            if (ParesTotales != partida.ParesTotales)
                return false;

            if (Iniciada != partida.Iniciada)
                return false;

            if (Terminada != partida.Terminada)
                return false;

            return true;
        }
    }

    public class PartidaJugadorDetalle
    {
        public int ParesEncontrados { get; set; }
        public float Tiempo { get; set; }
        public int Puntos { get; set; }
    }
}
