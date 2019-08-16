using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Atum.DAL.Helpers
{
    internal class CryptoHelper
    {
        private static byte[] _key = new byte[] { 125, 42, 53, 124, 75, 56, 87, 38, 9, 10, 161, 132, 183, 91, 105, 16, 117, 218, 149, 230, 221, 212, 235, 64 };
        private static byte[] _iv = new byte[] { 83, 71, 26, 58, 54, 35, 22, 11, 83, 71, 26, 58, 54, 35, 22, 11 };

        
        internal static string Decrypt(string data)
        {
            try
            {

                var t = data[0];
        
                byte[] inBytes = Convert.FromBase64String(data.Trim());
                var memoryStream = new MemoryStream(inBytes, 0, inBytes.Length);

                var aes = new RijndaelManaged();
                var cs = new CryptoStream(memoryStream, aes.CreateDecryptor(_key, _iv), CryptoStreamMode.Read);
                var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch (Exception exc)
            {
                try
                {

                    byte[] inBytes = Convert.FromBase64String(data.Substring(1));
                    var memoryStream = new MemoryStream(inBytes, 0, inBytes.Length);

                    var aes = new RijndaelManaged();
                    var cs = new CryptoStream(memoryStream, aes.CreateDecryptor(_key, _iv), CryptoStreamMode.Read);
                    var sr = new StreamReader(cs);

                    return sr.ReadToEnd();
                }
                catch (Exception exc2)
                {
                    Debug.WriteLine(exc2.Message);

                }

            }

            return null;
        }

        internal static string Encrypt(string data)
        {
            try
            {
                var utf8 = new UTF8Encoding();
                byte[] inBytes = utf8.GetBytes(data);
                var memoryStream = new MemoryStream();
                var aes = new RijndaelManaged();
                var cs = new CryptoStream(memoryStream, aes.CreateEncryptor(_key, _iv), CryptoStreamMode.Write);
                cs.Write(inBytes, 0, inBytes.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            return null;
        }
    }
}
