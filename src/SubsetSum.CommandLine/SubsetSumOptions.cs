using CommandLine;
using System.Collections.Generic;

namespace SubsetSum.CommandLine
{
    public class SubsetSumOptions
    {
        [Option('v', "verbose", Default = false, Required = false, HelpText = "Prints all messages to standard output.")]
        public bool IsVerbose { get; set; }

        [Value(0, Required = true, HelpText = "Sum that is required to achieve.", MetaName = "Sum")]
        public uint? Sum { get; set; }

        [Value(1, Min = 1, Required = true, HelpText = "Set with all possible numbers.", MetaName = "Set")]
        public IEnumerable<uint> Set { get; set; }
    }
}
