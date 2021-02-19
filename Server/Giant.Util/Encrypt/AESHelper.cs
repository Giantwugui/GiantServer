using System;
using System.Security.Cryptography;

namespace Giant.Util
{
    public static class AESHelper
    {
        private static readonly byte[] AESIV;
        public static readonly int KeyLength128 = 16;
        public static readonly int KeyLength256 = 32;

        static AESHelper()
        {
            AESIV = new byte[KeyLength128];
            using RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetBytes(AESIV);
        }

        public static string Encrypt(string encryptKey, string content, CipherMode mode)
        {
            try
            {
                byte[] keyArray = encryptKey.FromBase64String();
                byte[] toEncryptArray = content.FromUTF8String();

                using RijndaelManaged rDel = new RijndaelManaged()
                {
                    Key = keyArray,
                    Mode = mode,
                    Padding = PaddingMode.PKCS7,
                    IV = AESIV
                };
                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return resultArray.ToBase64String();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static byte[] Encrypt(byte[] encryptKey, byte[] content, CipherMode mode)
        {
            using RijndaelManaged rDel = new RijndaelManaged()
            {
                Key = encryptKey,
                Mode = mode,
                Padding = PaddingMode.PKCS7,
                IV = AESIV
            };
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(content, 0, content.Length);
            return resultArray;
        }

        public static string Dencrypt(string encryptKey, string content, CipherMode mode)
        {
            Byte[] keyArray = encryptKey.FromBase64String();
            Byte[] toEncryptArray = content.FromBase64String();

            using RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = mode,
                Padding = PaddingMode.PKCS7,
                IV = AESIV
            };
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray.ToUTF8String();
        }

        public static byte[] Dencrypt(byte[] encryptKey, byte[] content, CipherMode mode)
        {
            using RijndaelManaged managed = new RijndaelManaged
            {
                Key = encryptKey,
                Mode = mode,
                Padding = PaddingMode.PKCS7,
                IV = AESIV
            };
            ICryptoTransform cTransform = managed.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(content, 0, content.Length);
            return resultArray;
        }
    }
}

