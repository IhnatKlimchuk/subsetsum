using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum.Experimental
{
    public class SubsetSumSolver : ISubsetSumSolver<string>
    {
        private readonly ILogger logger;
        private readonly AlgorithmOptions options;
        private readonly CultureInfo cultureInfo;

        public SubsetSumSolver(
            AlgorithmOptions options,
            ILogger<UInt32RecursionSubsetSumSolver> logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cultureInfo = CultureInfo.InvariantCulture;
        }

        public Task<IReadOnlyCollection<string>> SolveAsync(string sum, string[] set, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
