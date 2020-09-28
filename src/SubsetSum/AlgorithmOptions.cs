namespace SubsetSum
{
    public class AlgorithmOptions
    {
        public uint MaxConcurrency { get; set; } = 1;
        public uint MinSubTreeHeight { get; set; } = 26;
        public AlgorithmType AlgorithmType { get; set; } = AlgorithmType.Recursion;
    }
}
