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

        public static string GetEntertainmentMovieAnimationPath(string base_drive_path)
        {
            string path = Path.Combine(base_drive_path, "entertaiment", "movie", "animations");

            if (!File.Exists(path)) throw new Exception($"Path: {path} was not found");

            return path;
        }

        public static string GetEntertainmentMovieActionPath(string base_drive_path)
        {
            string path = Path.Combine(base_drive_path, "entertaiment", "movie", "actions");

            if (!File.Exists(path)) throw new Exception($"Path: {path} was not found");

            return path;
        }
    }
}
