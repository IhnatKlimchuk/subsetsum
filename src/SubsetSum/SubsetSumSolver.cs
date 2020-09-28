using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum
{
    public class SubsetSumSolver : ISubsetSumSolver<string>
    {
        private readonly ILogger logger;
        private readonly AlgorithmOptions options;
        private readonly CultureInfo cultureInfo;

        public SubsetSumSolver(
            AlgorithmOptions options,
            CultureInfo cultureInfo,
            ILogger logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        }

        public async Task<IReadOnlyCollection<string>> SolveAsync(string sum, string[] set, CancellationToken cancellationToken)
        {
            var (argumentSum, argumentSet) = Parse(sum, set);

            options.AlgorithmType = options.AlgorithmType == AlgorithmType.Auto 
                ? CalculateOptimalAlgorithmType(argumentSum, argumentSet) 
                : options.AlgorithmType;

            ISubsetSumSolver<NumberArgument> subSolver = null;
            switch (options.AlgorithmType)
            {
                case AlgorithmType.Recursion:
                    subSolver = new RecursiveSubsetSumSolver(options, cultureInfo, logger);
                    break;
                case AlgorithmType.Dynamic:
                    subSolver = new DynamicSubsetSumSolver(options, cultureInfo, logger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unexpected algorithm type.");
            }

            var result = await subSolver.SolveAsync(argumentSum, argumentSet, cancellationToken);
            return result.Select(element => element.Original).ToArray();
        }

        private (NumberArgument argumentSum, NumberArgument[] argumentSet) Parse(string sum, string[] set)
        {
            var argumentSum = NumberArgument.Parse(sum);
            if (argumentSum.IsNeutral)
            {
                throw new ArgumentException("Sum can't be 0.");
            }

            var argumentSet = set
                .Select(NumberArgument.Parse)
                .Where(element =>
                {
                    if (element.IsNeutral)
                    {
                        logger.LogWarning($"{element.Original} is neutral and will be ignored.");
                        return false;
                    }
                    return true;
                }).ToArray();

            int maxFractionalPart = Math.Max(argumentSet.Max(element => element.FractionalPart.Length), argumentSum.FractionalPart.Length);
            argumentSum.ReduceFractionalPart(maxFractionalPart);
            foreach (var element in argumentSet)
            {
                element.ReduceFractionalPart(maxFractionalPart);
            }
            return (argumentSum, argumentSet);
        }

        private AlgorithmType CalculateOptimalAlgorithmType(NumberArgument argumentSum, NumberArgument[] argumentSet)
        {
            return AlgorithmType.Recursion;
        }
    }
}
