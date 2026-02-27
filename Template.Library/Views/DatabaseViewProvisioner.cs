using System.Reflection;

namespace Template.Library.Views
{
    public static class DatabaseViewProvisioner
    {
        /// <summary>
        /// Reads all embedded .sql files from the Views/Sql folder
        /// </summary>
        public static IEnumerable<(string Name, string Sql)> GetViewDefinitions()
        {
            var assembly = typeof(DatabaseViewProvisioner).Assembly;
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(r => r.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r);

            foreach (var resourceName in resourceNames)
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) continue;

                using var reader = new StreamReader(stream);
                var sql = reader.ReadToEnd();

                var viewName = Path.GetFileNameWithoutExtension(
                    resourceName.Split('.').Reverse().Skip(1).FirstOrDefault() ?? resourceName);

                yield return (viewName, sql);
            }
        }
    }
}
