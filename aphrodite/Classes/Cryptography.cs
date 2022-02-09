using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace aphrodite {
    internal class Cryptography {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 12_000;

        // Const to determine acceptable characters to generate a password.
        private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[{]};:',<.>/?";

        /// <summary>
        /// Gets the current users' defined password. If none exists, it will generate one.
        /// </summary>
        public static string GetLocalPassword {
            get {
                RegistryKey CV = Registry.CurrentUser.OpenSubKey("SOFTWARE\\aphrodite", false);

                if (CV == null) {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\aphrodite");
                    CV = Registry.CurrentUser.OpenSubKey("SOFTWARE\\aphrodite", true);
                    CV.SetValue("Generated", GenerateRandomPassword());
                }

                return CV.GetValue("Generated") as string;
            }
        }

        private static string GenerateRandomPassword(int PasswordLength = 32) {
            StringBuilder NewString = new();
            using RNGCryptoServiceProvider Randomness = new();
            byte[] Buffer = new byte[sizeof(uint)];
            while (PasswordLength-- > 0) {
                Randomness.GetBytes(Buffer);
                uint num = BitConverter.ToUInt32(Buffer, 0);
                NewString.Append(ValidCharacters[(int)(num % (uint)ValidCharacters.Length)]);
            }
            return NewString.ToString();
        }

        public static string Encrypt(string PlainText, string PassPhrase = null) {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            byte[] saltStringBytes = Generate256BitsOfRandomEntropy();
            byte[] ivStringBytes = Generate256BitsOfRandomEntropy();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(PlainText);

            using Rfc2898DeriveBytes password = new(PassPhrase ?? GetLocalPassword, saltStringBytes, DerivationIterations);
            byte[] keyBytes = password.GetBytes(Keysize / 8);

            using RijndaelManaged symmetricKey = new();
            symmetricKey.BlockSize = 256;
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;

            using ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
            byte[] cipherTextBytes = saltStringBytes;
            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string CipherText, string PassPhrase = null) {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(CipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            byte[] saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            byte[] ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            byte[] cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using Rfc2898DeriveBytes password = new(PassPhrase ?? GetLocalPassword, saltStringBytes, DerivationIterations);
            byte[] keyBytes = password.GetBytes(Keysize / 8);

            using RijndaelManaged symmetricKey = new();
            symmetricKey.BlockSize = 256;
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
            using MemoryStream memoryStream = new(cipherTextBytes);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        private static byte[] Generate256BitsOfRandomEntropy() {
            byte[] randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using RNGCryptoServiceProvider rngCsp = new();
            // Fill the array with cryptographically secure random bytes.
            rngCsp.GetBytes(randomBytes);

            return randomBytes;
        }

    }
}
