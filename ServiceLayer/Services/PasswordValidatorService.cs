using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    /// <summary>
    /// Contains all variables and methods pertaining to the
    /// "Have I Been Pwned" API and its integration with our
    /// web application
    /// </summary>
    public class PasswordValidatorService
    {
        public string _HashedPlaintext { get; private set; }
        public string _HashPrefix { get; private set; }
        public string _HashSuffix { get; private set; }
        public string _ApiResponse { get; private set; }
        public string _HashResponse { get; private set; }
        public int _NumberOfBreaches { get; private set; }

        /// <summary>
        /// The constructor for performing the various methods
        /// for integrating the "Have I Been Pwned" API
        /// </summary>
        /// <param name="plaintext">The plaintext password we are checking</param>
        public PasswordValidatorService(string plaintext)
        {
            // Hashes the plaintext into SHA-1
            _HashedPlaintext = SecurityService.ReturnSHA1Hash(plaintext);

            // Retrieves the prefix (first 5 chars) of the hashed plaintext
            _HashPrefix = GetHashPrefix(_HashedPlaintext);

            // Retrieves the suffix (the hashed plaintext exluding the first 5 chars)
            _HashSuffix = GetHashSuffix(_HashedPlaintext);

            // The full response of the "Have I Been Pwned" API
            _ApiResponse = ConsumePasswordAPI(_HashPrefix).Result;

            // The hash we are looking for in the API
            _HashResponse = FindSpecificHash(_ApiResponse);

            // The number of times the password being used has been breached
            _NumberOfBreaches = GetNumberOfBreaches();
        }

        /// <summary>
        /// Perform an HTTP request on the "Have I Been Pwned" API and
        /// retrieve the full response of the API
        /// </summary>
        /// <param name="hashPrefix">The parameter that the API will accepts</param>
        /// <returns>A string pertaining to the full resposne of the API</returns>
        private async Task<string> ConsumePasswordAPI(string hashPrefix)
        {
            try
            {
                // Used to consume the REST Service.... use HttpClient
                HttpClient httpClient = new HttpClient();

                // Holds the response of the "Have I Been Pwned" API
                var apiResponse = await httpClient.GetAsync("https://api.pwnedpasswords.com/range/" + hashPrefix);

                // Reads the response as a string and returns it
                return await apiResponse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                // HttpRequestException is caught and a message is displayed
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message : {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtains the first 5 charactes of the hashed plaintext
        /// </summary>
        /// <param name="sha1HashedPlaintext">The plaintext hashed using SHA-1</param>
        /// <returns>The substring of just the first 5 characterrs</returns>
        private string GetHashPrefix(string sha1HashedPlaintext)
        {
            // Obtains the first 5 chars of the hashed plaintext
            return sha1HashedPlaintext.Substring(0, 5);
        }

        /// <summary>
        /// Obtains the entire hashed plaintext exluding the first 5 characters
        /// </summary>
        /// <param name="sha1HashedPlaintext">The plaintext hashed using SHA-1</param>
        /// <returns>The substring of the hashed plaintext excluding the first 5 characters</returns>
        private string GetHashSuffix(string sha1HashedPlaintext)
        {
            // Obtains the hashed plaintext excluding the first 5 chars
            return sha1HashedPlaintext.Substring(5, sha1HashedPlaintext.Length - 5);
        }

        /// <summary>
        /// Locates the specific hash of the password if it can be found within the API
        /// </summary>
        /// <param name="apiResponse">The response that was retrieved from the consuming the API</param>
        /// <returns>The target hash that was found in the API. If not found, the target hash is null</returns>
        private string FindSpecificHash(string apiResponse)
        {
            // The location in the response HashList of the suffix that corresponds to the password we want
            int startingIndex = apiResponse.IndexOf(_HashSuffix);

            // If the hash suffix is not found in the API response, the password is secure therefore return null
            if (startingIndex == -1)
            {
                return null;
            }

            // If the target hash suffix is found, get its location via substring
            string targetHash = apiResponse.Substring(startingIndex, _ApiResponse.Length - startingIndex);

            // Obtains the index of a new line delimiter
            int indexOfNewLine = targetHash.IndexOf(Environment.NewLine);

            // Checks if there is no line break... signified by the index being -1
            if (indexOfNewLine == -1)
            {
                // Return the last string in the response since it does not have a break line
                return targetHash.Substring(0, apiResponse.Length - startingIndex);
            }
            // Checks if there is a line break
            else
            {
                // Returns the full string in the response somewhere other than the end of the response
                return targetHash.Substring(0, indexOfNewLine);
            }
        }

        /// <summary>
        /// Manipulated the target hash to isolate the number of breaches of the password
        /// from the API response
        /// </summary>
        /// <returns>The number of breaches that has occured for the password</returns>
        private int GetNumberOfBreaches()
        {
            //Check if password was found in the api
            if (string.IsNullOrEmpty(_HashResponse))
            {
                // Return -1 if there were no breaches for the password
                return -1;
            }
            // The starting index we want... start at ":"
            int start = _HashResponse.IndexOf(":") + 1;

            // The ending index we want to retrieve the remainder of the string
            int end = _HashResponse.Length - start;

            // Parses the breach frequency of the plaintext into an int and returns it
            return Int32.Parse(_HashResponse.Substring(start, end));
        }

        /// <summary>
        /// Validates if the number of breaches for the password exceeds the max number
        /// of breaches as specified by business rules
        /// </summary>
        /// <param name="numOfBreaches">The number of breaches that has occured for the password</param>
        /// <returns>True if the password is less than or equal to the max number of breaches.
        /// False if the password has been breached before.</returns>
        public bool ValidateBreachFrequencyRange(int numOfBreaches)
        {
            // Password is secure if it is in range of 2 breaches
            int maxNumberofBreaches = 2;
            // Checks if the password is less than or equal to the max number of breaches
            if (numOfBreaches <= maxNumberofBreaches)
            {
                // Return true if the password has less than or equal to the max number of breaches
                return true;
            }
            else
            {
                // The password has been breached more than the max number and is therefore deemed not seucre
                return false;
            }
        }
    }
}
