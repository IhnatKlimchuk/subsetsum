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

        public UInt32RecursionSubsetSumSolver(ILogger<UInt32RecursionSubsetSumSolver> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IReadOnlyCollection<uint>> SolveAsync(uint sum, IEnumerable<uint> set, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting calcultaion...");
            var result = Solve(set.ToArray(), 0, sum, cancellationToken);
            logger.LogDebug("Calcultaion completed.");
            return Task.FromResult<IReadOnlyCollection<uint>>(result);
        }

        public static IImmutableList<uint> Solve(ReadOnlySpan<uint> subSet, uint currentSum, uint requiredSum, CancellationToken cancellationToken)
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
            return Solve(nextSubSet, newSum, requiredSum, cancellationToken)?.Add(subSet[0]) ?? Solve(nextSubSet, currentSum, requiredSum, cancellationToken);
        }
    }
}
