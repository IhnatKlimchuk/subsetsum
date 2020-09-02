using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Immutable;
using System.Threading;

namespace SubsetSum.Benchmark
{
    public class UInt32RecursionBenchmark
    {
        private const uint sum = 13;
        private readonly uint[] set = new uint[] { 3, 4, 6, 8 };
        public UInt32RecursionBenchmark()
        {
        }

        [Benchmark]
        public IImmutableList<uint> FromZero()
        {
            return FromZeroRecursion(set, 0, sum, CancellationToken.None);
        }

        public static IImmutableList<uint> FromZeroRecursion(ReadOnlySpan<uint> subSet, uint currentSum, uint requiredSum, CancellationToken cancellationToken)
        {
            if (subSet.IsEmpty || currentSum > requiredSum || cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var newSum = currentSum + subSet[0];
            if (newSum == requiredSum)
            {
                return ImmutableList.Create(subSet[0]);
            }

            var nextSubSet = subSet.Slice(1);
            return FromZeroRecursion(nextSubSet, newSum, requiredSum, cancellationToken)?.Add(subSet[0]) ?? FromZeroRecursion(nextSubSet, currentSum, requiredSum, cancellationToken);
        }

        [Benchmark]
        public IImmutableList<uint> ToZero()
        {
            return ToZeroRecursion(set, sum, CancellationToken.None);
        }

        public static IImmutableList<uint> ToZeroRecursion(ReadOnlySpan<uint> subSet, uint currentSum, CancellationToken cancellationToken)
        {
            if (subSet.IsEmpty || cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            if (subSet[0] == currentSum)
            {
                return ImmutableList.Create<uint>().Add(subSet[0]);
            }
            else 
            {
                var nextSubSet = subSet.Slice(1);
                return (subSet[0] > currentSum ? null : ToZeroRecursion(nextSubSet, currentSum - subSet[0], cancellationToken))?.Add(subSet[0])
                    ?? ToZeroRecursion(nextSubSet, currentSum, cancellationToken);
            }
        }
    }
}
