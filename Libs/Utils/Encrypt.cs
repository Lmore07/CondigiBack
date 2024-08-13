using System.Security.Cryptography;
using System.Text;

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

        public static string EncryptString(string plainText, string key)
        {
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = ConvertStringToByteArray(key);

            // Ensure the key is of a valid size
            if (aesAlg.Key.Length != 16 && aesAlg.Key.Length != 24 && aesAlg.Key.Length != 32)
            {
                throw new ArgumentException("Invalid key size. Key must be 16, 24, or 32 bytes long.");
            }

            aesAlg.IV = IvKeyStore.IvKey;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            byte[] encrypted;

            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            return System.Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] cipherBytes = System.Convert.FromBase64String(cipherText);

                string plaintext = null;

                using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return plaintext;
            }
        }
    }
}