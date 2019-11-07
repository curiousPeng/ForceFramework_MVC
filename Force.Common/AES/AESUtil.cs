using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Force.Common.AES
{
    public static class AESUtil
    {
        #region 加解密
        public static byte[] EncryptStr(string message, byte[] secret_key, byte[] auth_key)
        {
            var msg_byte = Encoding.UTF8.GetBytes(message);

            return AESThenHMAC.SimpleEncrypt(msg_byte, secret_key, auth_key);
        }

        public static string DecryptStr(string encrypt_message, byte[] secret_key, byte[] auth_key)
        {
            var encrypt_str = Convert.FromBase64String(encrypt_message);
            var bytes = AESThenHMAC.SimpleDecrypt(encrypt_str, secret_key, auth_key);

            return Encoding.UTF8.GetString(bytes);
        }

        public static string Md5(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", string.Empty);
        }
        #endregion
    }
}
