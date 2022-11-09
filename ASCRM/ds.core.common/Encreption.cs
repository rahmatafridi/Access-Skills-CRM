using System;
using System.Text;

namespace ds.portal.ui.Models
{
    public static class Encryption
    {
        public static string Encrypt(string clearText)
        {
            try
            {
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(clearText));
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                return Encoding.Unicode.GetString(Convert.FromBase64String(cipherText));
            }
            catch
            {
                return "";
            }
        }
    }
}
