using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SubsetSum.Tests
{
    public class SubsetSumSolverTests
    {
        private readonly ISubsetSumSolver<string> subsetSumSolver;
        private readonly AlgorithmOptions options;
        private readonly ILogger logger;

        public SubsetSumSolverTests()
        {
            options = Substitute.For<AlgorithmOptions>();
            options.AlgorithmType = AlgorithmType.Recursion;
            logger = Substitute.For<ILogger>();
            subsetSumSolver = new SubsetSumSolver(
                options,
                CultureInfo.InvariantCulture,
                logger);
        }
        
        [Fact]
        public async Task SolveAsync_should_pass_happy_pass_with_success_result()
        {
            var result = await subsetSumSolver.SolveAsync("13", new string[] { "3", "4", "6", "8" }, CancellationToken.None);
            result.Should().BeEquivalentTo(new string[] { "3", "6", "4" });
        }

        [Fact]
        public async Task SolveAsync_should_handle_duplicates_with_success_result()
        {
            var result = await subsetSumSolver.SolveAsync("9", new string[] { "3", "3", "8", "3" }, CancellationToken.None);
            result.Should().BeEquivalentTo(new string[] { "3", "3", "3" });
        }

        [Fact]
        public async Task SolveAsync_should_handle_duplicates_with_different_strings_with_success_result()
        {
            var result = await subsetSumSolver.SolveAsync("9", new string[] { "3.0", "03", "8", "3" }, CancellationToken.None);
            result.Should().BeEquivalentTo(new string[] { "3.0", "03", "3" });
        }

        [Fact]
        public async Task SolveAsync_should_handle_neutal_elements_with_success_result()
        {
            var result = await subsetSumSolver.SolveAsync("9", new string[] { "0", ".", "0.0", "-0", "-0.0", ".0", "0.", "3", "3", "8", "3" }, CancellationToken.None);
            result.Should().BeEquivalentTo(new string[] { "3", "3", "3" });
        }
    }
}
