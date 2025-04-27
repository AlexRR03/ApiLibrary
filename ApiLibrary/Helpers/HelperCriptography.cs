using System.Security.Cryptography;
using System.Text;

namespace ApiLibrary.Helpers
{
    public class HelperCriptography
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 0; i < 50; i++)
            {
                int rand = random.Next(1, 255);
                char letter = Convert.ToChar(rand);
                salt += letter;
            }
            return salt;
        }

        public static bool ComparePass(byte[] a, byte[] b)
        {
            bool result = true;
            if (a.Length != b.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;

        }

        public static byte[] EncryptPass(string password, string salt)
        {
            string hash = password + salt;
            SHA512 managed = SHA512.Create();
            byte[] output = Encoding.UTF8.GetBytes(hash);
            for (int i = 0; i < 10; i++)
            {
                output = managed.ComputeHash(output);
            }
            managed.Clear();
            return output;
        }
    }
}
