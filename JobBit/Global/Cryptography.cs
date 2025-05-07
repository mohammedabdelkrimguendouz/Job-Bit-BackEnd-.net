using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobBit.Global
{
    public class Cryptography
    {
        public static string ComputeHash(string Input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] HashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Input));
                return BitConverter.ToString(HashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
