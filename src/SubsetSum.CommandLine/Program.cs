using CommandLine;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum.CommandLine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<SubsetSumOptions>(args)
                .WithParsedAsync(SolveSubsetSumAsync);
        }

        private static async Task SolveSubsetSumAsync(SubsetSumOptions options)
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                if (options.IsVerbose)
                {
                    builder
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Debug);
                }
            });

            var result = await SolveAsync(options.Sum.Value, options.Set.ToArray(), options.MaxConcurrency, loggerFactory);

            if (result == null)
            {
                Console.Error.WriteLine("No solution found.");
            }
            else
            {
                Console.Out.WriteLine($"Solution found: {string.Join(", ", result)}.");
            }
        }

        private static async Task<IReadOnlyCollection<uint>> SolveAsync(uint sum, uint[] set, uint maxConcurrency, ILoggerFactory loggerFactory)
        {
            var solver = new UInt32RecursionSubsetSumSolver(
                options: new AlgorithmOptions 
                {
                     MaxConcurrency = maxConcurrency > Environment.ProcessorCount ? (uint)Environment.ProcessorCount : maxConcurrency
                },
                logger: loggerFactory.CreateLogger<UInt32RecursionSubsetSumSolver>());;
            return await solver.SolveAsync(sum, set, CancellationToken.None);
        }
    }
}
