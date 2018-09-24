using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaseClasses
{
    public class CypherUtility
    {
        //默认密钥向量
        private static readonly byte[] Keys = { 0x11, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static string SHA1(string src)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytesSha1In = Encoding.UTF8.GetBytes(src);
            byte[] bytesSha1Out = sha1.ComputeHash(bytesSha1In);
            string strSha1Out = BitConverter.ToString(bytesSha1Out);
            strSha1Out = strSha1Out.Replace("-", "");
            return strSha1Out;
        }
        public static string Md5(string src)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(src));
            StringBuilder b = new StringBuilder();
            foreach (byte d in data)
                b.Append(d.ToString("x2"));
            return b.ToString();
        }


        public static string Md5(byte[] src)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(src);
            StringBuilder b = new StringBuilder();
            foreach (byte d in data)
                b.Append(d.ToString("x2"));
            return b.ToString();
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        private static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }


        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        private static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }



    }
}
