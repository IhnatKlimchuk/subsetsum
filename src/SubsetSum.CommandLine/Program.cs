using CommandLine;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            var result = await SolveAsync(options.Set, options.Sum.Value, loggerFactory);

            if (result == null)
            {
                Console.Error.WriteLine("No solution found.");
            }
            else
            {
                Console.Out.WriteLine($"Solution found: {string.Join(", ", result)}.");
            }
        }

        private static Task<ICollection<uint>> SolveAsync(IEnumerable<uint> set, uint sum, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger("subsetsum");
            logger.LogInformation("Emulating solving with no results.");
            return Task.FromResult<ICollection<uint>>(null);
        }
    }
}
