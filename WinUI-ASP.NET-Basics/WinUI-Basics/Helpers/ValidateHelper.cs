using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI_Basics.Helpers
{
    class ValidateHelper
    {
        static public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        static public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            bool hasMinimumLength = password.Length >= 8;
            bool hasUpperCaseLetter = password.Any(char.IsUpper);
            bool hasLowerCaseLetter = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasMinimumLength && hasUpperCaseLetter && hasLowerCaseLetter && hasDigit && hasSymbol;
        }
    }
}
