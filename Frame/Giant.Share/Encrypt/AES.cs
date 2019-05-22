using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Giant.Share
{
    public static class AES
    {
        /// <summary>
        /// AES 加密
        /// </summary>
        public static string Encrypt(string encryptKey, string bizContent, CipherMode mode)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(encryptKey);
                byte[] toEncryptArray = Encoding.UTF8.GetBytes(bizContent);

                RijndaelManaged rDel = new RijndaelManaged()
                {
                    Key = keyArray,
                    Mode = mode,
                    Padding = PaddingMode.PKCS7,
                    IV = mAESIV
                };
                ICryptoTransform cTransform = rDel.CreateEncryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public static string Dencrypt(string encryptKey, string bizContent, CipherMode mode)
        {
            try
            {
                Byte[] keyArray = Encoding.UTF8.GetBytes(encryptKey);
                Byte[] toEncryptArray = Convert.FromBase64String(bizContent);

                RijndaelManaged rDel = new RijndaelManaged
                {
                    Key = keyArray,
                    Mode = mode,
                    Padding = PaddingMode.PKCS7,
                    IV = mAESIV
                };

                ICryptoTransform cTransform = rDel.CreateDecryptor(rDel.Key, rDel.IV);

                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        public static string Encrypt_Aes(string encryptKey, string bizContent)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(encryptKey);
            byte[] resultArray = EncryptStringToBytes_Aes(bizContent, keyArray, mAESIV);

            return Convert.ToBase64String(resultArray);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public static string Dencrypt_Aes(string encryptKey, string bizContent)
        {
            Byte[] keyArray = Encoding.UTF8.GetBytes(encryptKey);
            Byte[] toEncryptArray = Convert.FromBase64String(bizContent);

            return DecryptStringFromBytes_Aes(toEncryptArray, keyArray, mAESIV);
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        static byte[] InitIv(int blockSize)
        {
            byte[] iv = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                iv[i] = (byte)0x0;
            }
            return iv;
        }

        /// <summary>
        /// 128位0向量
        /// </summary>
        static byte[] mAESIV = InitIv(16);
    }
}

