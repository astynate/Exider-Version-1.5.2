using System.IdentityModel.Tokens.Jwt;

namespace Cloud_Storage.Controllers
{
    public static class JwtHandler
    {

        public static string? GetEmailByToken(string? token)
        {

            try
            {

                var handler = new JwtSecurityTokenHandler();
                var converted_token = handler.ReadJwtToken(token);
                var email = converted_token.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

                return email;

            }

            catch(Exception ex)
            {

                Console.WriteLine(ex);

            }

            return null;

        }

    }
}
