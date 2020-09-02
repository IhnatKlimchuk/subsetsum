using BenchmarkDotNet.Running;

namespace SubsetSum.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<UInt32RecursionBenchmark>();
        }
    }
}
