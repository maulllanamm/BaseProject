using System.Security.Cryptography;
using System.Text;
using Services.Interface;

namespace Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string ComputeHash(string password, string salt, string papper, int iteration)
        {
            if (iteration <= 0)
            {
                return password;
            }

            var sha256 = SHA256.Create();
            var passwordSaltPapper = $"{password}{salt}{papper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPapper);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, papper, iteration - 1);
        }

        public string GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }
    }
}
