using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobBit.Global
{
    public class Validation
    {
       


        public static bool ValidatePassword(string  Password)
        {
            string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            var regex = new Regex(Pattern);
            return regex.IsMatch(Password);
        }

        public static bool ValidateLink(string Link)
        {
            string Pattern = @"^(https?:\/\/)?([\w.-]+)\.([a-zA-Z]{2,})(\/[^\s]*)?$";
            var regex = new Regex(Pattern);
            return regex.IsMatch(Link);
        }

        public static bool ValidatePhone(string Phone)
        {
            string Pattern = @"^(?:\+213|0)(5|6|7)[0-9]{8}$|^(?:\+213|0)(21|23|24|25|26|27|29|31|32|33|34|35|36|37|38|39|41|43|44|45|46|47|48|49)[0-9]{6,7}$";
            var regex = new Regex(Pattern);
            return regex.IsMatch(Phone);
        }

        public static bool ValidateEmail(string EmailAddress)
        {
            var Pattern = @"^[a-zA-Z0-9._%+-]{6,30}@gmail\.com$";
            var regex =new Regex(Pattern);
            return regex.IsMatch(EmailAddress);
        }
        public static bool ValidateInteger(string Number)
        {
            var Pattern = @"^\d+$";
            var regex = new Regex(Pattern);
            return regex.IsMatch(Number);
        }
        public static bool validateFloat(string Number)
        {
            var Pattern = @"^-?\d+(\.\d+)?$";
            var regex = new Regex(Pattern);
            return regex.IsMatch(Number);
        }
        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number)||validateFloat(Number)) ;
        }

    }
}
