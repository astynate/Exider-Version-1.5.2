using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cloud_Storage.Controllers
{
    public static class Options
    {

        public const string Issuer = "ZIXE COMPANY";
        public const string Audience = "USER";
        public const string UniqueKey = "THIS IS THE ENCRIPTION KEY OF THE ZIXE COMPANY";
        public const string Path = @"D:\(5) Zixe Projects\Cloud Storage\Cloud Storage\__system__\";
        public const string _userAvatarPath = Path + @"__users__\profile-image\";
        public const string _userHeaderPath = Path + @"__users__\profile-header\";
        public static string[] ImageTypes = { "png", "jpeg", "jpg", "ico", "gif", "tif", "webp", "svg", "raw" };

        public static SymmetricSecurityKey GetSecurityKey()
        {

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UniqueKey));

        }    

    }
}
