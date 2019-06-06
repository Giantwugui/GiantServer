using Giant.Share;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.Test
{
    class Test_AES
    {
        static readonly int KeyLength128 = 16;
        static readonly int KeyLength256 = 32;

        public static void Test()
        {
            //加密字符串 "U"
            //秘钥 "A000000000000000"

            //加解密模式 
            //CipherMode.CBC
            //CipherMode.CFB
            //CipherMode.ECB

            //加解密填充模式 
            //PaddingMode.PKCS7 解密后字符串："U"
            //PaddingMode.ISO10126 解密后字符串："U"
            //PaddingMode.ANSIX923 解密后字符串："U"
            //PaddingMode.Zeros "U\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"
            //微软官方范例 解密后字符串："U"

            {
                string key = "PPBUBAE77CFA2U11";
                //string key = "618947DE24148P25";


                string aimStr = "1";

                //string tempS = Encoding.UTF8.GetString(Convert.FromBase64String(jm));

                string resultCBC = AES.Encrypt(key, aimStr, CipherMode.CBC);
                string resultCFB = AES.Encrypt(key, aimStr, CipherMode.CFB);
                string resultECB = AES.Encrypt(key, aimStr, CipherMode.ECB);
                string resultCTS = AES.Encrypt(key, aimStr, CipherMode.CTS);
                string resultOFB = AES.Encrypt(key, aimStr, CipherMode.OFB);
                string resultMS = AES.Encrypt_Aes(key, aimStr);

                key = GetLegalKey(key);

                string jm = "Ml0Pc1SnD85xWpB3psCD3A==";

                string decStrCBC = AES.Dencrypt(key, jm, CipherMode.CBC);
                string decStrECB = AES.Dencrypt(key, jm, CipherMode.ECB);

                string decStrCFB = AES.Dencrypt(key, jm, CipherMode.CFB);
                string decStrCTS = AES.Dencrypt(key, resultCTS, CipherMode.CTS);
                string decStrOFB = AES.Dencrypt(key, resultOFB, CipherMode.OFB);
                string decStrMS = AES.Dencrypt_Aes(key, resultMS);

                Console.WriteLine("加密前: " + aimStr);
                Console.WriteLine("加密后 CipherMode.CBC: " + resultCBC);
                Console.WriteLine("加密后 CipherMode.CFB: " + resultCFB);
                Console.WriteLine("加密后 CipherMode.CTS: " + resultCTS);
                Console.WriteLine("加密后 CipherMode.ECB: " + resultECB);
                Console.WriteLine("加密后 CipherMode.OFB: " + resultOFB);
                Console.WriteLine("加密后 resultMS: " + resultMS);

                Console.WriteLine("解密后 CipherMode.CBC: " + decStrCBC);
                Console.WriteLine("解密后 CipherMode.CFB: " + decStrCFB);
                Console.WriteLine("解密后 CipherMode.CTS: " + decStrCTS);
                Console.WriteLine("解密后 CipherMode.ECB: " + decStrECB);
                Console.WriteLine("解密后 CipherMode.OFB: " + decStrOFB);
                Console.WriteLine("解密后 decStrMS: " + decStrMS);
            }

            while (true)
            {
                string key = Console.ReadLine();
                key = GetLegalKey(key);

                string readStr = Console.ReadLine();
                Console.WriteLine("加密后: " + AES.Encrypt_Aes(key, readStr));
            }
        }

        /// <summary>
        /// 获取合法key值
        /// </summary>
        public static string GetLegalKey(string key)
        {
            int keylength = key.Length;

            if (keylength != KeyLength128 && keylength != KeyLength256)
            {
                if (key.Length > KeyLength256)
                {
                    key = key.Substring(0, KeyLength256);
                }
                else if (key.Length > KeyLength128)
                {
                    key = key.Substring(0, KeyLength128);
                }
                else
                {
                    for (int i = 0; i < KeyLength128 - keylength; ++i)
                    {
                        key += "0";
                    }
                }
            }

            return key;
        }
    }


    static class AesCsp
    {

        public static byte[] EncryptBytes(byte[] input, byte[] aesKey, byte[] aesIV)
        {
            Aes aes = new AesCryptoServiceProvider
            {
                Padding = PaddingMode.None,
                Mode = CipherMode.ECB
            };
            var enc = aes.CreateEncryptor(aesKey, aesIV);
            return enc.TransformFinalBlock(input, 0, input.Length);
        }

        public static byte[] DecryptBytes(byte[] encryptedOutput, byte[] aesKey, byte[] aesIV)
        {
            Aes aes = new AesCryptoServiceProvider
            {
                Padding = PaddingMode.None
            };
            var dec = aes.CreateDecryptor(aesKey, aesIV);
            return dec.TransformFinalBlock(encryptedOutput, 0, encryptedOutput.Length);
        }

        public static string EncryptHexStrings(string input, byte[] aesKey, byte[] aesIV)
        {
            byte[] bytes = HexStringToByteArray(input);
            byte[] encBytes = EncryptBytes(bytes, aesKey, aesIV);
            return ByteArrayToHexString(encBytes);
        }

        public static string DecryptHexStrings(string encryptedOutput, byte[] aesKey, byte[] aesIV)
        {
            byte[] bytes = HexStringToByteArray(encryptedOutput);
            byte[] decBytes = DecryptBytes(bytes, aesKey, aesIV);
            return ByteArrayToHexString(decBytes);
        }

        public static string EncryptHexStrings(string input, string aesKey, string aesIV)
        {
            byte[] key = HexStringToByteArray(aesKey);
            byte[] iv = HexStringToByteArray(aesIV);
            return EncryptHexStrings(input, key, iv);
        }

        public static string DecryptHexStrings(string encryptedOutput, string aesKey, string aesIV)
        {
            byte[] key = HexStringToByteArray(aesKey);
            byte[] iv = HexStringToByteArray(aesIV);
            return DecryptHexStrings(encryptedOutput, key, iv);
        }

        public static byte[] HexStringToByteArray(string s)
        {
            byte[] ret = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                ret[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }
            return ret;
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.AppendFormat("{0:X2}", b);
            return sb.ToString();
        }

    }

}
