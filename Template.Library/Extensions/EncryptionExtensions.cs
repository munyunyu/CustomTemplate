using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Extensions
{
    public static class EncryptionExtensions
    {
        public static string GetSHA256(this string text)
        {
            using SHA256 sha256 = SHA256.Create();

            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

            return Convert.ToBase64String(hashBytes);
        }

        public static string GetConcatenatedProperties<T>(this T entity)
        {
            return string.Join("~", typeof(T).GetProperties()
                .OrderBy(p => p.Name) // Order properties by name in ascending order
                .Select(p => p.GetValue(entity)?.ToString())); // Get property values and convert to string
        }
    }
}
