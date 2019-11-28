using System;
using System.Security.Cryptography;
using System.Text;

namespace Giant.Core
{
    public static class MD5Helper
    {
        private static readonly MD5 md5Crypter = new MD5CryptoServiceProvider();

        public static string Encrypt(string content)
        {
            var securityBytes = md5Crypter.ComputeHash(Encoding.UTF8.GetBytes(content));
            return BitConverter.ToString(securityBytes).Replace("-", "");
        }
    }
}
