using System;
using System.Linq;
using CommandLine;

namespace geocells
{
    [Verb("crunch", HelpText = "Takes tab-delimited horizon data and creates spreadsheet of 'cells'")]
    class CrunchOptions
    {
        [Value(1, Required = true, HelpText = "A file path for a tab-delimited horizon file")]
        public string HorizonPath { get; set; }

        [Value(2, Required = true, HelpText = "A file path for a comma-delimited well location file (X, Y)")]
        public string WellPath { get; set; }
    }

    [Verb("gen", HelpText = "Generates random well locations")]
    class GenOptions
    {
        [Value(1, Required = true, HelpText = "A destination file path for a comma-delimited well location file (X, Y)")]
        public string WellPath { get; set; }

        [Option('n', "count", HelpText = "Number of wells to generate. Default is 5.")]
        public int Count { get; set; } = 5;
    }

    class Program
    {
        static int Main(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = Console.Out);
            var result = parser.ParseArguments<
                CrunchOptions,
                GenOptions>
                (args);
            try
            {
                result
                    .WithParsed<CrunchOptions>(opts => Crunch(opts))
                    .WithParsed<GenOptions>(opts => Gen(opts));
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
            var horizonSamples = CsvReading.ReadCsvFromFile<HorizonSample>(opts.HorizonPath, "\t")
                .ToArray();
            Console.WriteLine($"Read {horizonSamples.Length} horizon samples.");
            var wells = CsvReading.ReadCsvFromFile<Location>(opts.WellPath)
                .ToArray();
            Console.WriteLine($"Read {wells.Length} well locations.");
        }

        private static Random _rnd = new Random();

        private static void Gen(GenOptions opts)
        {
            // Hard-coded values based on sample data...
            var xMin = 1000.0;
            var xMax = 1300.0;
            var dx = xMax - xMin;
            var yMin = 2000.0;
            var yMax = 2245.0;
            var dy = yMax - yMin;
            var locations = new Location[opts.Count];
            for (int i = 0; i < opts.Count; i++)
            {
                var x = _rnd.NextDouble() * dx + xMin;
                var y = _rnd.NextDouble() * dy + xMin;
                locations[i] = new Location { X = x, Y = y };
            }
            CsvReading.WriteCsvToFile(opts.WellPath, locations);
        }
    }
}
