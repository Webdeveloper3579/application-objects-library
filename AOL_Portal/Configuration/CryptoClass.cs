using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AOL_Portal.Configuration
{
    
    
    public class CryptoClass
    {

      
        public static string Decrypt(string inkey, string message)
        {

          
            try
            {
                byte[] cipherText = new byte[message.Length / 2];
                for (int i = 0; i < message.Length; i += 2)
                {

                    cipherText[i / 2] = byte.Parse(message[i].ToString() + message[i + 1].ToString(),
                        System.Globalization.NumberStyles.HexNumber);
                }

                char[] CharArray = inkey.ToCharArray();
                byte[] Key = new byte[32];
                for (int i = 0; i < CharArray.Length; i++)
                {
                    Key[i] = Convert.ToByte(CharArray[i]);
                }

                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("checkKey");

                string plaintext = null;

                using (var rijAlg = Aes.Create())
                {
                    rijAlg.Key = Key;
                    rijAlg.Mode = CipherMode.ECB;

                    ICryptoTransform dycryptor = rijAlg.CreateDecryptor();

                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypot = new CryptoStream(msDecrypt, dycryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypot))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }

                    }
                }


                return plaintext;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Encrypt(string encryptKey, string message)
        {

            try
            {
               
                char[] CharArray = encryptKey.ToCharArray();

                byte[] Key = new byte[32];

                for (int i = 0; i < CharArray.Length; i++)
                {
                    Key[i] = Convert.ToByte(CharArray[i]);
                }

                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("setKey");

                byte[] encrypted;
                using ( var rijAlg = Aes.Create())
                {
                    rijAlg.Key = Key;
                    rijAlg.Mode = CipherMode.ECB;

                    ICryptoTransform encryptor = rijAlg.CreateEncryptor();

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swWriter = new StreamWriter(csEncrypt))
                            {
                                swWriter.Write(message);
                            }

                            encrypted = msEncrypt.ToArray();
                        }

                    }
                }

                return BitConverter.ToString(encrypted).Replace("-", "");
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
