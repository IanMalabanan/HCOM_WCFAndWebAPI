using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities.Security
{
    public class SecurityUtil
    {

        public static string EncryptString(string text)
        {
            string keyString = "jA0919erU15esRgY1Epe3MaB31Lpd822";
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] key = Encoding.UTF8.GetBytes(keyString);

            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        byte[] iv = aesAlg.IV;

                        byte[] decryptedContent = msEncrypt.ToArray();

                        byte[] result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText)
        {
            string keyString = "jA0919erU15esRgY1Epe3MaB31Lpd822";

            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
            byte[] key = Encoding.UTF8.GetBytes(keyString);

            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (MemoryStream msDecrypt = new MemoryStream(cipher))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
