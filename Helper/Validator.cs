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
    }
}
