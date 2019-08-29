using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class EncryptionService
    {
        private string key;
        private string IV;
        private string stringToEncrypt;

        public EncryptionService(int id)
        {
            string date = DateTime.Now.ToString();
            this.stringToEncrypt = id.ToString() + "_" + date;
            AesCryptoServiceProvider myAes = new AesCryptoServiceProvider();
            myAes.Padding = PaddingMode.PKCS7;
            myAes.Mode = CipherMode.CBC;
            myAes.KeySize = 256;
            key = Convert.ToBase64String(myAes.Key);
            IV = Convert.ToBase64String(myAes.IV);
        }

        public EncryptionService(int id, string key, string IV)
        {
            string date = DateTime.Now.ToString();
            this.stringToEncrypt = id.ToString() + "_" + date;
            this.key = key;
            this.IV = IV;
        }

        public string getEncryption()
        {
            byte[] keyByte = Convert.FromBase64String(key);
            byte[] vectorByte = Convert.FromBase64String(IV);
            byte[] encrypted = EncryptStringToBytes_Aes(stringToEncrypt, keyByte, vectorByte);
            string encryptedId = Convert.ToBase64String(encrypted);
            return encryptedId;
        }

        public int decryptString(string encryptedString)
        {
            byte[] encrypted = Convert.FromBase64String(encryptedString);
            byte[] aesKey = Convert.FromBase64String(this.key);
            byte[] aesIV = Convert.FromBase64String(this.IV);
            string decrypted = DecryptStringFromBytes_Aes(encrypted, aesKey, aesIV);
            int indexOf = decrypted.IndexOf("_");
            string stringId = decrypted.Substring(0, indexOf); //check
            int id = Int32.Parse(stringId);//check
            return id;

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

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
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

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
