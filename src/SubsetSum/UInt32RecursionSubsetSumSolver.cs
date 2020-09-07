using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum
{
    public sealed class UInt32RecursionSubsetSumSolver : ISubsetSumSolver<uint>
    {
        private readonly ILogger logger;
        private readonly AlgorithmOptions options;

        public UInt32RecursionSubsetSumSolver(
            AlgorithmOptions options, 
            ILogger<UInt32RecursionSubsetSumSolver> logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IReadOnlyCollection<uint>> SolveAsync(uint sum, IEnumerable<uint> set, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting calcultaion...");
            var result = Solve(set.ToArray(), sum, cancellationToken);
            logger.LogDebug("Calcultaion completed.");
            return Task.FromResult<IReadOnlyCollection<uint>>(result);
        }

        public static IImmutableList<uint> Solve(ReadOnlySpan<uint> subSet, uint currentSum, CancellationToken cancellationToken)
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
                return (subSet[0] > currentSum ? null : Solve(nextSubSet, currentSum - subSet[0], cancellationToken))?.Add(subSet[0])
                    ?? Solve(nextSubSet, currentSum, cancellationToken);
            }
        }
    }
}
