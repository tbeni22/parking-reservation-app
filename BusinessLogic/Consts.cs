using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Consts
    {
        public const int UsernameMinLength = 5;

        /*
        Contains at least one digit.
        Contains at least one lowercase letter.
        Contains at least one uppercase letter.
        Contains at least one special character from the provided set: !@#$%^&*{|}?~_=+.-
        Contains at least one character that is not a letter or digit.
        Doesn't contain whitespace.
        Has a length between 6 and 24 characters.
        */
        public const string PasswordRegex =
            @"^(?=.*\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\s).{6,24}$";

        public const string UsernameLengthValidationError = "Username must have more than 5 characters.";
        public const string EmailValidationError = "Email must have valid format.";

        public const string PasswordValidationError =
            "Password must have more than 6 characters, min. 1 uppercase, min. 1 lowercase, min. 1 special characters.";
    }
}
