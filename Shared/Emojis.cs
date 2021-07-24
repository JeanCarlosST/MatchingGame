using MatchingGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Shared
{
    public class Emojis
    {
        public static List<string> ListaAnimales = new List<string>()
        {
            "🐵", "🐶", "🐺", "🐱", "🦁", "🐯", 
            "🦒", "🦊", "🦝", "🐮", "🐷", "🐗", 
            "🐭", "🐹", "🐰", "🐻", "🐨", "🐼", 
            "🐸", "🦓", "🐴", "🦄", "🐔", "🐲", 
            "🐟", "🐠", "🐡", "🦐", "🦑", "🐙", 
            "🦞", "🦀", "🐥", "🦜", "🦇", "🦋", 
        };

        public static List<string> ObtenerListaAnimales(int filas, int cols)
        {
            int FxC = filas * cols;
            int parejas = FxC / 2;

            List<string> lista = new List<string>();

            for(int i = 0; i < parejas; i++)
            {
                lista.Add(ListaAnimales[i]);
                lista.Add(ListaAnimales[i]);
            }

            return lista;
        }

        public static List<string> ObtenerListaAnimales(Dificultad dificultad)
        {
            int parejas = ObtenerParesTotales(dificultad);

            List<string> lista = new List<string>();

            for (int i = 0; i < parejas; i++)
            {
                lista.Add(ListaAnimales[i]);
                lista.Add(ListaAnimales[i]);
            }

            return lista;
        }

        public static int ObtenerParesTotales(Dificultad dificultad)
        {
            switch (dificultad)
            {
                case Dificultad._4x4:
                    return 8;
                case Dificultad._4x5:
                    return 10;
                case Dificultad._4x6:
                    return 12;
                case Dificultad._4x7:
                    return 14;
                case Dificultad._4x8:
                case Dificultad._6x6:
                    return 16;
                case Dificultad._6x7:
                    return 21;
                case Dificultad._6x8:
                    return 24;
                case Dificultad._8x8:
                    return 32;
            }

            return -1;
        }
    }
}
