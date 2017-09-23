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

        [Value(3, Required = true, HelpText = "Output path for comma-delimited file")]
        public string OutputPath { get; set; }
    }
}
