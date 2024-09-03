using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;

namespace CondigiBack.Libs.Utils
{
    public class Encrypt
    {
        public static string GenerateHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static bool VerifyHash(string input, string hash)
        {
            string hashOfInput = GenerateHash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static void GenerateRandomIv()
        {
            var iv = new byte[16];
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(iv);
            IvKeyStore.IvKey = iv;
        }

        public static byte[] ConvertStringToByteArray(string? input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static byte[] EncryptString(string plainText, string key)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(key),
                IvKeyStore.IvKey,
                32,
                HashAlgorithmName.SHA256, 
                32
            );
        }
    }
}