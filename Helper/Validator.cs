using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Helper
{
    public static class Validator
    {
        public static bool IsNotNullOrWhiteSpace(params string[] values)
        {
            return values.All(value => !string.IsNullOrWhiteSpace(value));
        }

        public static bool IsDigit(char e) => char.IsDigit(e);
        public static bool IsLetter(char e) => char.IsLetter(e);
        public static bool IsSymbol(char e) => char.IsSymbol(e);

    }
}
