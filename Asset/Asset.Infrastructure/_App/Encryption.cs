using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Asset.Infrastructure._App
{
    public class Encryption
    {
        private static AesCryptoServiceProvider AesProvider = (AesCryptoServiceProvider)null;
        private static string AesKey256 = string.Empty;
        public static string PasswordSalt = "*|*IchKenneNichtNichts*|*";
        static Encryption()
        {
            string str1 = AppSettings.EncryptionKey;
            if (string.IsNullOrWhiteSpace(str1))
                throw new Exception("EncryptionKey not found in config file. We need 'EncryptionKey' in Infrastructure DLL.");
            string str2 = new string("$alam#Chetori?62".Reverse<char>().ToArray<char>());
            AesKey256 = GetMd5Hash(str1 + str2);
            AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
            int num1 = 128;
            cryptoServiceProvider.BlockSize = num1;
            int num2 = 256;
            cryptoServiceProvider.KeySize = num2;
            byte[] bytes1 = Encoding.UTF8.GetBytes("$alam#Chetori?62");
            cryptoServiceProvider.IV = bytes1;
            byte[] bytes2 = Encoding.UTF8.GetBytes(AesKey256);
            cryptoServiceProvider.Key = bytes2;
            int num3 = 1;
            cryptoServiceProvider.Mode = (CipherMode)num3;
            int num4 = 2;
            cryptoServiceProvider.Padding = (PaddingMode)num4;
            AesProvider = cryptoServiceProvider;
        }

        public static string Encrypt(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            using (ICryptoTransform encryptor = AesProvider.CreateEncryptor())
                return Convert.ToBase64String(encryptor.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string Decrypt(string text)
        {
            byte[] inputBuffer = Convert.FromBase64String(text);
            using (ICryptoTransform decryptor = AesProvider.CreateDecryptor())
                return Encoding.Unicode.GetString(decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
        }

        public static string GetMd5Hash(string input)
        {

            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string ComputeSHAHash(string valueToHash)
        {
            HashAlgorithm algorithm = SHA512.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));

            return Convert.ToBase64String(hash);
        }
        public static string CreateSalt()
        {
            int size = 64;
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

    }
}
