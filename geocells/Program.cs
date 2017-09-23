using System;
using System.Linq;
using CommandLine;

namespace geocells
{
    [Verb("crunch", HelpText = "Takes tab-delimited horizon data and creates spreadsheet of 'cells'")]
    class CrunchOptions
    {
        [Value(1, Required = true, HelpText = "A file name is required for a tab-delimited horizon file")]

        public string FilePath { get; set; }
    }

    public class HorizonSample
    {
        public long Inline { get; set; }
        public long Crossline { get; set; }
        public double Z { get; set; }
        public double Porosity { get; set; }
        public double Amplitude { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = Console.Out);
            var result = parser.ParseArguments<
                CrunchOptions>
                (args);
            try
            {
                result
                    .WithParsed<CrunchOptions>(opts => Crunch(opts));
                return 0;
            }
            catch (Exception exception)
            {
                using (ChangeConsoleColor.To(ConsoleColor.Black, ConsoleColor.Yellow))
                    Console.Error.WriteLine(exception.Message);
                using (ChangeConsoleColor.To(ConsoleColor.Yellow, ConsoleColor.Black))
                    Console.Error.WriteLine(exception.StackTrace);
                return -1;
            }
        }

        private static void Crunch(CrunchOptions opts)
        {
            var horizonSamples = CsvReading.ReadCsvFromFile<HorizonSample>(opts.FilePath, "\t")
                .ToArray();
            Console.WriteLine($"I was able to read {horizonSamples.Length} samples.");
        }
    }
}
