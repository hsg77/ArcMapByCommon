using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ArcMapByCommon
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public static class CryptoUtil
    {
        static void EncryptOrDecryptStream(SymmetricAlgorithm symmetric, Stream stream, Stream outStream, string password, bool isEncrypt)
        {
            byte[] desIV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            byte[] desKey = desIV;
            for (int i = 0; i < password.Length; i++)
            {
                if (i < desKey.Length)
                {
                    desKey[i] = (byte)password[i];
                }
            }
            symmetric.IV = desIV;
            symmetric.Key = desKey;

            //创建加密流
            ICryptoTransform cryptoTransform = isEncrypt ? symmetric.CreateEncryptor() : symmetric.CreateDecryptor();
            CryptoStream encryptStream = new CryptoStream(outStream, cryptoTransform, CryptoStreamMode.Write);

            int completedLength = 0;
            byte[] insertData = new byte[100];
            //从输入文件中读取流，然后加密到输出文件中
            while (completedLength < stream.Length)
            {
                //每次写入加密文件的数据大小
                int Length = stream.Read(insertData, 0, 100);
                encryptStream.Write(insertData, 0, Length);
                completedLength = completedLength + Length;
            }
            //关闭流
            encryptStream.FlushFinalBlock();
            encryptStream.Close();
        }
        static void EncryptOrDecryptStream(Stream stream, Stream outStream, string password, bool isEncrypt)
        {
            using (SymmetricAlgorithm symmetric = new DESCryptoServiceProvider())
            {
                CryptoUtil.EncryptOrDecryptStream(symmetric, stream, outStream, password, isEncrypt);
            }
        }

        /// <summary>
        /// 加密流
        /// </summary>
        /// <param name="stream">需要加密的流</param>
        /// <param name="outStream">加密后输出流</param>
        /// <param name="password">密码</param>
        public static void EncryptStream(Stream stream, Stream outStream, string password)
        {
            EncryptOrDecryptStream(stream, outStream, password, true);
        }
        public static void EncryptStream(SymmetricAlgorithm symmetric, Stream stream, Stream outStream, string password)
        {
            CryptoUtil.EncryptOrDecryptStream(symmetric, stream, outStream, password, true);
        }
        /// <summary>
        /// 解密流
        /// </summary>
        /// <param name="stream">需要解密的流</param>
        /// <param name="outStream">解密后输出流</param>
        /// <param name="password">密码</param>
        public static void DecryptStream(Stream stream, Stream outStream, string password)
        {
            EncryptOrDecryptStream(stream, outStream, password, false);
        }
        public static void DecryptStream(SymmetricAlgorithm symmetric, Stream stream, Stream outStream, string password)
        {
            CryptoUtil.EncryptOrDecryptStream(symmetric, stream, outStream, password, false);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns>加密后的结果字符串</returns>
        public static string EncryptString(string text, string password)
        {
            string res = string.Empty;
            using (SymmetricAlgorithm symmetric = new DESCryptoServiceProvider())
            {
                res = CryptoUtil.EncryptString(symmetric, text, password);
            }
            return res;
        }
        public static string EncryptString(SymmetricAlgorithm symmetric, string text, string password)
        {
            MemoryStream srcStream = null;
            srcStream = new MemoryStream(Encoding.Default.GetBytes(text));
            MemoryStream outStream = new MemoryStream();
            CryptoUtil.EncryptStream(symmetric, srcStream, outStream, password);

            StringBuilder ret = new StringBuilder();
            foreach (byte b in outStream.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            srcStream.Dispose();
            outStream.Dispose();

            return ret.ToString();
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="text">需要解密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns>解密后的结果字符串</returns>
        public static string DecryptString(string text, string password)
        {
            string res = string.Empty;
            using (SymmetricAlgorithm symmetric = new DESCryptoServiceProvider())
            {
                res = CryptoUtil.DecryptString(symmetric, text, password);
            }
            return res;
        }
        public static string DecryptString(SymmetricAlgorithm symmetric, string text, string password)
        {
            MemoryStream srcStream = null;
            byte[] inputByteArray = new byte[text.Length / 2];
            for (int x = 0; x < text.Length / 2; x++)
            {
                int i = (Convert.ToInt32(text.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            srcStream = new MemoryStream(inputByteArray);
            MemoryStream outStream = new MemoryStream();
            CryptoUtil.DecryptStream(symmetric, srcStream, outStream, password);

            string str = Encoding.Default.GetString(outStream.ToArray());
            srcStream.Dispose();
            outStream.Dispose();

            return str;
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="srcFile">需要加密的源文件</param>
        /// <param name="outFile">加密后的文件</param>
        /// <param name="password">密码</param>
        public static void EncryptFile(SymmetricAlgorithm symmetric, string srcFile, string outFile, string password)
        {
            FileStream srcFileStream = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            FileStream encryptFileStream = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);

            CryptoUtil.EncryptStream(symmetric, srcFileStream, encryptFileStream, password);

            encryptFileStream.Close();
            encryptFileStream.Dispose();
            srcFileStream.Close();
            srcFileStream.Dispose();
        }
        public static void EncryptFile(string srcFile, string outFile, string password)
        {
            using (SymmetricAlgorithm symm = new DESCryptoServiceProvider())
            {
                CryptoUtil.EncryptFile(symm, srcFile, outFile, password);
            }
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="srcFile">需要解密的源文件</param>
        /// <param name="outFile">解密后的文件</param>
        /// <param name="password">密码</param>
        public static void DecryptFile(SymmetricAlgorithm symmetric, string srcFile, string outFile, string password)
        {
            FileStream srcFileStream = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            FileStream encryptFileStream = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);

            CryptoUtil.DecryptStream(symmetric, srcFileStream, encryptFileStream, password);

            encryptFileStream.Close();
            encryptFileStream.Dispose();
            srcFileStream.Close();
            srcFileStream.Dispose();
        }
        public static void DecryptFile(string srcFile, string outFile, string password)
        {
            using (SymmetricAlgorithm symm = new DESCryptoServiceProvider())
            {
                CryptoUtil.DecryptFile(symm, srcFile, outFile, password);
            }
        }
        /// <summary>
        /// 创建RSA密钥
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privateKey">私钥</param>
        public static void RSACreateKey(out string publicKey, out string privateKey)
        {
            AsymmetricAlgorithm asymmetric = new RSACryptoServiceProvider();
            publicKey = asymmetric.ToXmlString(false);
            privateKey = asymmetric.ToXmlString(true);
        }
        /// <summary>
        /// RSA加密文本
        /// </summary>
        /// <param name="text">需要加密的文本</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>加密后的文本</returns>
        public static string RSAEncryptString(string text, string publicKey)
        {
            RSACryptoServiceProvider asymmetric = new RSACryptoServiceProvider();
            asymmetric.Clear();
            asymmetric.FromXmlString(publicKey);
            byte[] bytes = Encoding.Default.GetBytes(text);
            return Convert.ToBase64String(asymmetric.Encrypt(bytes, false));
        }
        /// <summary>
        /// RSA解密文本
        /// </summary>
        /// <param name="text">需要解密的文本</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>解密后的文本</returns>
        public static string RSADecryptString(string text, string privateKey)
        {
            byte[] bytes = Convert.FromBase64String(text);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.Clear();
            rsa.FromXmlString(privateKey);
            byte[] bytes_Plain_Text = rsa.Decrypt(bytes, false);
            return Encoding.Default.GetString(bytes_Plain_Text);
        }
        /// <summary>
        /// 获取文件的HASH码
        /// </summary>
        /// <param name="fileName">文件</param>
        /// <returns>HASH码</returns>
        public static string GetFileHash(string fileName)
        {
            FileStream fs = System.IO.File.Open(fileName, FileMode.Open);
            string hashCode = GetStreamHash(fs);
            fs.Close();
            fs.Dispose();
            return hashCode;
        }
        /// <summary>
        /// 获取流的HASH码
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>HASH码</returns>
        public static string GetStreamHash(Stream stream)
        {
            using (HashAlgorithm hash = new MD5CryptoServiceProvider())
            {
                byte[] hashCode = hash.ComputeHash(stream);
                return Convert.ToBase64String(hashCode);
            }
        }
        /// <summary>
        /// 获取文本的HASH码
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>HASH码</returns>
        public static string GetStringHash(string text)
        {
            using (HashAlgorithm hash = new MD5CryptoServiceProvider())
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(text);
                byte[] result = hash.ComputeHash(data);
                return Convert.ToBase64String(result);
            }
        }
    }

}
