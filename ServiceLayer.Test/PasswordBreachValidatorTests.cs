using System;
using ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    [TestClass]
    public class PasswordValidatorServiceTests
    {
        [TestMethod]
        public void ConsumePasswordAPI_CheckValidSHA_Pass()
        {
            // Arrange
            var plaintext = "password";

            // The expected SHA-1 hash for the plaintext
            var expected = "5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8".ToUpper();
            var actual = "";

            // Act
            actual = SecurityService.ReturnSHA1Hash(plaintext);

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ConsumePasswordAPI_PasswordIsSecure_Pass()
        {
            // Arrange
            var plaintext = "Th!sP@assw0rd1sS3cuRed";
            PasswordValidatorService ValidatePassword = new PasswordValidatorService(plaintext);
            var numberOfBreaches = ValidatePassword._NumberOfBreaches;
            var expected = true;
            var actual = false;

            // Act
            if (numberOfBreaches < 0) actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNumberOfBreaches_BreachesWithinRange_Pass()
        {
            // Arrange
            var plaintext = "password";
            PasswordValidatorService ValidatePassword = new PasswordValidatorService(plaintext);
            var expected = false;
            var actual = false;

            int numberOfBreaches = ValidatePassword._NumberOfBreaches;

            // Act
            actual = ValidatePassword.ValidateBreachFrequencyRange(numberOfBreaches);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashPrefix_ValidPrefix_Pass()
        {
            // Arrange
            string password = "password";
            PasswordValidatorService ValidatePassword = new PasswordValidatorService(password);
            string hash = "5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8".ToUpper();

            // The expected SHA-1 hash for the plaintext
            var expected = "5BAA6";
            var actual = "";

            // Act
            actual = ValidatePassword._HashPrefix;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashSuffix_ValidSuffix_Pass()
        {
            // Arrange
            string password = "password";
            PasswordValidatorService ValidatePassword = new PasswordValidatorService(password);
            string hash = "5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8".ToUpper();

            // The expected SHA-1 hash for the plaintext
            var expected = "1e4c9b93f3f0682250b6cf8331b7ee68fd8".ToUpper();
            var actual = "";

            // Act
            actual = ValidatePassword._HashSuffix;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PasswordValidator_PrefixMatches_Pass()
        {
            // Arrange
            var plaintext = "password";
            PasswordValidatorService pw = new PasswordValidatorService(plaintext);
            var expected = "5baa6".ToUpper();
            var actual = "";

            // Act
            actual = pw._HashPrefix;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
