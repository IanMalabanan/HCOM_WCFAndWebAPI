using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Hcom.Web.Api.Utilities.Security
{
    /// <summary>
    /// Summary description for Crypto
    /// </summary>
    public class RijndaelEncryptionProvider
    {

        static RijndaelEncryptionProvider _instance;
        public static RijndaelEncryptionProvider GetInstance()
        {
            return _instance = _instance ?? new RijndaelEncryptionProvider(
                RijndaelEncryptionProvider.ConvertKeyToBytes("cti2020-$xnE&JQ*m7dhhhF8xq$@g&Gf", 32), //Encryption Key, 32 character, 256-bit
                RijndaelEncryptionProvider.ConvertKeyToBytes("cti2020-EjypH2&8", 16));
        }

        private readonly byte[] bytKey;

        //Byte vector required for Rijndael.  This is randomly generated and recommended you change it on a per-application basis.
        //It is 16 bytes.
        //private static byte[] bytIV = new Byte[] { 63, 74, 69, 32, 30, 32, 30, 72, 63, 255, 50, 38, 56, 68, 33, 24 };
        private readonly byte[] bytIV;        

        //String to display on error for functions that return strings. {0} is Exception.Message.
        private string strTextErrorString = "#ERROR - {0}";

        //Size in bytes of the key length.  Rijndael takes either a 128, 192, or 256 bit key.  
        //If it is under this, pad with chrKeyFill. If it is over this, truncate to the length.
        private const int intKeySize = 32;

        //null char
        private static char vbNullChar = Convert.ToChar(0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptionKey">Byte key required for Rijndael. Rijndael takes either a 128, 192, or 256 bit key</param>
        /// <param name="encryptionIV">Byte vector required for Rijndael.  This is randomly generated and recommended you change it on a per-application basis. It is 16 bytes.</param>
        public RijndaelEncryptionProvider(byte[] encryptionKey, byte[] encryptionIV)
        {
            bytKey = encryptionKey;
            bytIV = encryptionIV;
        }

        public static byte[] ConvertKeyToBytes(string strKey, int bytSize)
        {
            //Character to pad keys with to make them at least intMinKeySize.
            const char chrKeyFill = 'X';

            try
            {
                int intLength = strKey.Length;

                if (intLength < bytSize)
                {
                    strKey = strKey.PadRight(bytSize, chrKeyFill);
                }
                else
                {
                    strKey = strKey.Substring(0, bytSize);
                }

                return Encoding.UTF8.GetBytes(strKey);
            }
            catch
            {
                return null;
            }
        }

        public string Encrypt(string dataToEncrypt)
        {
            try
            {
                byte[] bytPlainText;
                byte[] bytEncoded;

                dataToEncrypt = dataToEncrypt.Replace(vbNullChar.ToString(), String.Empty);

                bytPlainText = Encoding.UTF8.GetBytes(dataToEncrypt);

                MemoryStream objMemoryStream = new MemoryStream();
                RijndaelManaged objRijndaelManaged = new RijndaelManaged();
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,
                    objRijndaelManaged.CreateEncryptor(bytKey, bytIV),
                    CryptoStreamMode.Write);

                objCryptoStream.Write(bytPlainText, 0, bytPlainText.Length);
                objCryptoStream.FlushFinalBlock();

                bytEncoded = objMemoryStream.ToArray();
                objMemoryStream.Close();
                objCryptoStream.Close();

                return Convert.ToBase64String(bytEncoded);
            }
            catch (Exception ex)
            {
                return String.Format(strTextErrorString, ex.Message);
            }
        }

        public string Decrypt(string dataToDecrypt)
        {
            try
            {
                byte[] bytCryptText;

                bytCryptText = Convert.FromBase64String(dataToDecrypt);

                byte[] bytTemp = new byte[bytCryptText.Length];

                MemoryStream objMemoryStream = new MemoryStream(bytCryptText);
                RijndaelManaged objRijndaelManaged = new RijndaelManaged();

                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,
                    objRijndaelManaged.CreateDecryptor(bytKey, bytIV),
                    CryptoStreamMode.Read);

                objCryptoStream.Read(bytTemp, 0, bytTemp.Length);

                objMemoryStream.Close();
                objCryptoStream.Close();

                return Encoding.UTF8.GetString(bytTemp).Replace(vbNullChar.ToString(), String.Empty);
            }
            catch (Exception ex)
            {
                return String.Format(strTextErrorString, ex.Message);
            }
        }
    }
}