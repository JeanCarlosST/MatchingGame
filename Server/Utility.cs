using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Server
{
    public class Utility
    {
        public static string Encrypt(string password)
        {
            return password;
            //var provider = MD5.Create();
            //string salt = "S0m3R@nd0mSalt";
            //byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + password));
            //return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
