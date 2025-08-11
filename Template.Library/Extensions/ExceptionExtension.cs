using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetAllMessages(this Exception exp)
        {
            string message = string.Empty;

            Exception? innerException = exp;

            do
            {
                message += innerException.Message + ".";
                //message += innerException.Message + innerException.StackTrace + ".";
                innerException = innerException.InnerException;
            }
            while (innerException != null);

            return message;
        }
    }
}
