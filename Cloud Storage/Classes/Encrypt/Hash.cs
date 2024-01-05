using System;
using System.Security.Cryptography;
using System.Text;

namespace Cloud_Storage.Controllers
{
    public static class Hash
    {
        public static string Encode(string InputData)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {

                if (InputData != null)
                {

                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(InputData));

                    StringBuilder builder = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                        builder.Append(bytes[i].ToString("x2"));

                    return builder.ToString();

                }

                else
                {

                    return "none";

                }

            }

        }
        public static bool check_match(string str_1, string str_2) {

            return (Hash.Encode(str_1) == str_2) ? true : false;

        }

    }
}
