using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Extensions
{
    public static class PathExtensions
    {
        public static string GetFileFullPath(this string path, string filename) 
        {  
            string fullPath = Path.Combine(path, filename);

            return fullPath;
        }

    }
}
