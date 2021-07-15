using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Client.Shared
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
    }
}
