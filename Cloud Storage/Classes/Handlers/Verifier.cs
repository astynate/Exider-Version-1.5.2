using Cloud_Storage.Models;
using System.Text.RegularExpressions;

namespace Cloud_Storage.wwwroot.cs_classes
{

    public static class Verifier
    {

        private static readonly UserContext _userContext = new UserContext();

        private static readonly Regex _emailRegex = new Regex(@"^(?i)[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");

        public static bool VerifyRegister(params string[] userData)
        {

            foreach (var data in userData)
            {

                if (string.IsNullOrEmpty(data))
                {

                    return false;

                }

            }

            if (_userContext.Users.FirstOrDefault(x => x.Nickname == userData[2]) != null)
            {

                return false;

            }

            else if (_userContext.Users.FirstOrDefault(x => x.Email == userData[3]) != null)
            {

                return false;

            }

            else if (!_emailRegex.IsMatch(userData[3]))
            {

                return false;

            }

            return true;

        }

    }
}
