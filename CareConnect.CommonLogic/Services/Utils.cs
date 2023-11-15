using Newtonsoft.Json;
using CareConnect.CommonLogic.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CareConnect.CommonLogic.Enums;

namespace CareConnect.CommonLogic.Services
{
    public static class Utils
    {

        private const string keyString = "c55d79f3f4184e2f8f3c979b367821b1";
        private const string ClientKey = "-@!8A0P.!nm099(+";
        private const string ClientSalt = "i+!_Ay(1_9-*!71O";

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new ();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string RemoveSpecialCharacters2(string str)
        {
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool IsNumeric(this string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Encrypt string.
        /// /// </summary>
        /// <param name="text"></param>
        /// <returns name="result">Decrypted string</returns>
        public static string Encrypt(string text)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            var iv = aesAlg.IV;

            var decryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + decryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypt string.
        /// /// </summary>
        /// <param name="cipherText"></param>
        /// <returns name="result">Encrypted string</returns>
        public static string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            string result;
            using (var msDecrypt = new MemoryStream(cipher))
            {
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            }

            return result;
        }

        public static string DecryptStringAES(string cipherText)
        {
            try
            {
                var secretkey = Encoding.UTF8.GetBytes(ClientKey);
                var ivKey = Encoding.UTF8.GetBytes(ClientSalt);

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, secretkey, ivKey);
                return decriptedFromJavascript;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string EncryptStringAES(string plainText)
        {
            try
            {
                var secretkey = Encoding.UTF8.GetBytes(ClientKey);
                var ivKey = Encoding.UTF8.GetBytes(ClientSalt);

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                // var encrypted = Convert.ToBase64String(plainBytes);
                var encryptedFromJavascript = EncryptStringToBytes(plainText, secretkey, ivKey);
                // _logger.LogWarning($" decriptedFromJavascript: {encryptedFromJavascript}");
                return encryptedFromJavascript;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {

            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = Aes.Create("AesManaged"))
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for encryption.
                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
            //Convert.ToBase64String(encrypted);
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = Aes.Create("AesManaged"))
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                // rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using var msDecrypt = new MemoryStream(cipherText);
                    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using var srDecrypt = new StreamReader(csDecrypt);
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
                catch (Exception)
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static async Task<IpInfo> GetUserLocationByIp(string ip)
        {
            _ = new IpInfo();
            IpInfo ipInfo;
            try
            {
                string info = await new HttpClient().GetStringAsync("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo = null;
            }

            return ipInfo;
        }

        public static double CalculateDistance(Coordinate point1, Coordinate point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }        
    }
}
