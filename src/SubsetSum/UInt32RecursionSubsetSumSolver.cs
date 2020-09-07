using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public async Task<IReadOnlyCollection<uint>> SolveAsync(uint sum, uint[] set, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting calcultaion...");
            IImmutableList<uint> result;
            if (options.MaxConcurrency == 1 || set.Length <= options.MinSubTreeHeight)
            {
                result = Solve(sum, set, cancellationToken);
            }
            else
            {
                result = await SolveParallelAsync(sum, set, cancellationToken);
            }
            logger.LogDebug("Calcultaion completed.");
            return result;
        }

        private IImmutableList<uint> Solve(uint currentSum, ReadOnlySpan<uint> subSet, CancellationToken cancellationToken)
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
                return (subSet[0] > currentSum ? null : Solve(currentSum - subSet[0], nextSubSet, cancellationToken))?.Add(subSet[0])
                    ?? Solve(currentSum, nextSubSet, cancellationToken);
            }
        }

        private async Task<IImmutableList<uint>> SolveParallelAsync(uint sum, uint[] set, CancellationToken cancellationToken)
        {
            IImmutableList<uint> globalResult = null;
            using (SemaphoreSlim semaphore = new SemaphoreSlim((int)options.MaxConcurrency))
            using (CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                try
                {
                    var mainQueue = new Queue<SubTask>();
                    mainQueue.Enqueue(new SubTask() { FixedSet = ImmutableList<uint>.Empty, Set = set, Sum = sum });
                    while (mainQueue.Count > 0)
                    {
                        var subTask = mainQueue.Dequeue();
                        if (mainQueue.Count >= options.MaxConcurrency || subTask.Set.Length < options.MinSubTreeHeight)
                        {
                            await semaphore.WaitAsync(cancellationTokenSource.Token);
                            _ = Task.Run(() =>
                            {
                                try
                                {
                                    var subResult = Solve(subTask.Sum, subTask.Set.Span, cancellationTokenSource.Token);
                                    if (subResult != null)
                                    {
                                        globalResult = subResult.AddRange(subTask.FixedSet);
                                        cancellationTokenSource.Cancel();
                                    }
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            });
                        }
                        else
                        {
                            var nextElement = subTask.Set.Span[0];
                            if (nextElement == sum)
                            {
                                return subTask.FixedSet.Add(nextElement);
                            }
                            var newSet = subTask.Set.Slice(1);
                            if (nextElement < sum)
                            {
                                mainQueue.Enqueue(new SubTask() { FixedSet = subTask.FixedSet.Add(nextElement), Set = newSet, Sum = subTask.Sum - nextElement });
                            }
                            mainQueue.Enqueue(new SubTask() { FixedSet = subTask.FixedSet, Set = newSet, Sum = subTask.Sum });
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    //ignore
                }
                finally
                {
                    for (int i = 0; i < options.MaxConcurrency; i++)
                    {
                        await semaphore.WaitAsync();
                    }
                }
            }
            return globalResult;
        }

        private class SubTask
        {
            public IImmutableList<uint> FixedSet { get; set; }
            public ReadOnlyMemory<uint> Set { get; set; }
            public uint Sum { get; set; }
        }
    }
}
