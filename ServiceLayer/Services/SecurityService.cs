using System.Text;
using System.Security.Cryptography;

namespace ServiceLayer.Services
{
    /// <summary>
    /// The class that will deal with all security hashes and encryption
    /// </summary>
    public class SecurityService
    {
        /// <summary>
        /// This function uses SHA-1 to hash a plaintext.
        /// Uses the built-in Cryptography library to perform 
        /// a SHA-1 hash on the plaintext
        /// </summary>
        /// <param name="plaintext">The plaintext to be hashed using SHA-1</param>
        /// <returns>The plaintext hashed in SHA-1</returns>
        public static string ReturnSHA1Hash(string plaintext)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            // Use the SHA1 object to encrypt the plaintext in hash
            byte[] hashList = sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

            // Store the hashed value from bytes to string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashList.Length; i++)
                // Append the hashed bytes into a String Builder
                // Transform hash to all caps using X2
                sb.Append(hashList[i].ToString("X2"));

            // Return the hashed value as a string
            return sb.ToString();
        }
    }
}
