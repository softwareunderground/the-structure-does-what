using System.Collections.Generic;
using System.IO;
using ServiceStack;

namespace geocells
{
    internal static class CsvReading
    {
        internal static IEnumerable<T> ReadCsvFromFile<T>(string path, string separator = ",")
        {
            using (var stream = File.OpenRead(path))
                return ReadCsv<T>(stream, separator);
        }

        internal static IEnumerable<T> ReadCsv<T>(Stream stream, string separator = ",")
        {
            var streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            return ReadCsv<T>(text, separator);
        }

        internal static IEnumerable<T> ReadCsv<T>(string text, string separator = ",")
        {
            // https://stackoverflow.com/a/32963939/483776
            ServiceStack.Text.CsvConfig.ItemSeperatorString = separator;
            var listings = text.FromCsv<List<T>>();
            ServiceStack.Text.CsvConfig.ItemSeperatorString = ","; // Reset the global separator to the default comma
            return listings;
        }
    }
}
