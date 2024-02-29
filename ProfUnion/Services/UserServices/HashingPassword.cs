using Konscious.Security.Cryptography;
using Profunion.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Profunion.Services.UserServices
{
    public class HashingPassword
    {
        public string HashPassword(string password)
        {
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                var salt = GenerateSalt();
                hasher.Salt = salt;
                // You can tweak parameters as needed
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 65536; // 64MB
                hasher.Iterations = 10;

                return Convert.ToBase64String(hasher.GetBytes(32)); // 32 bytes for a secure hash

            }
        }
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // Adjust the size of the salt as needed
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
