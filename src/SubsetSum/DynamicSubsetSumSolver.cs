using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum
{
    public class DynamicSubsetSumSolver : ISubsetSumSolver<NumberArgument>
    {
        private readonly ILogger logger;
        private readonly AlgorithmOptions options;
        private readonly CultureInfo cultureInfo;

        public DynamicSubsetSumSolver(
            AlgorithmOptions options,
            CultureInfo cultureInfo,
            ILogger logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        }

        public Task<IReadOnlyCollection<NumberArgument>> SolveAsync(NumberArgument sum, NumberArgument[] set, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
