using CommandLine;

namespace geocells
{
    [Verb("gen", HelpText = "Generates random well locations")]
    class GenOptions
    {
        [Value(1, Required = true, HelpText = "A destination file path for a comma-delimited well location file (X, Y)")]
        public string WellPath { get; set; }

        [Option('n', "count", HelpText = "Number of wells to generate. Default is 5.")]
        public int Count { get; set; } = 5;

        [Option('h', "horizon", HelpText = "A horizon file. Used to get the inline/crossline range")]
        public string HorizonPath { get; set; }
    }
}
