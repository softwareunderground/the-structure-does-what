using System;
using System.Linq;
using CommandLine;

namespace geocells
{
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

        private const double NullValue = 1e30;

        private static void Crunch(CrunchOptions opts)
        {
            HorizonSample[] horizonSamples = ReadHorizon(opts.HorizonPath);
            var wells = CsvReading.ReadCsvFromFile<Location>(opts.WellPath)
                .ToArray();
            Console.WriteLine($"Read {wells.Length} well locations.");
            if (horizonSamples[0].DistanceToNearestWell == 0)
            {
                Console.WriteLine("Calculating distances...");
                foreach (var sample in horizonSamples)
                    sample.DistanceToNearestWell = DistanceToNearestWell(sample, wells);
            }
            else
                Console.WriteLine("Skipping distance calculation because distance already in horizon file.");
            Console.WriteLine("Writing output...");
            CsvReading.WriteCsvToFile(opts.OutputPath, horizonSamples);
            Console.WriteLine("Done.");
            Console.WriteLine("You're welcome.");
        }

        private static HorizonSample[] ReadHorizon(string horizonPath)
        {
            var horizonSamples = CsvReading.ReadCsvFromFile<HorizonSample>(horizonPath, "\t")
                .Where(s => s.Amplitude != NullValue && s.Porosity != NullValue && s.Z != NullValue)
                .ToArray();
            Console.WriteLine($"Read {horizonSamples.Length} horizon samples.");
            return horizonSamples;
        }

        static double DistanceToNearestWell(ILocation sample, ILocation[] wells)
        {
            var squareDistances = wells.Select(well => well.DistanceSquared(sample));
            var squareNearest = squareDistances.Min();
            return Math.Sqrt(squareNearest);
        }

        private static Random _rnd = new Random();

        private static void Gen(GenOptions opts)
        {
            // Default range is 0 to 1000
            var xMin = 0.0;
            var xMax = 1000.0;
            var yMin = 0.0;
            var yMax = 1000.0;
            if (opts.HorizonPath != null)
            {
                var horizonSamples = ReadHorizon(opts.HorizonPath);
                xMin = horizonSamples.Min(s => s.X);
                xMax = horizonSamples.Max(s => s.X);
                yMin = horizonSamples.Min(s => s.Y);
                yMax = horizonSamples.Max(s => s.Y);
            }
            Console.WriteLine($"Using range x={xMin}...{xMax}, y={yMin}...{yMax}.");
            var dx = xMax - xMin;
            var dy = yMax - yMin;
            var locations = new Location[opts.Count];
            for (int i = 0; i < opts.Count; i++)
            {
                var x = _rnd.NextDouble() * dx + xMin;
                var y = _rnd.NextDouble() * dy + yMin;
                locations[i] = new Location { X = x, Y = y };
            }
            CsvReading.WriteCsvToFile(opts.WellPath, locations);
        }
    }
}
