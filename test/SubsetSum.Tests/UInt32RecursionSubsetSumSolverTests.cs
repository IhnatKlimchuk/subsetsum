using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SubsetSum.Tests
{
    public class UInt32RecursionSubsetSumSolverTests
    {
        private readonly ISubsetSumSolver<uint> subsetSumSolver;

        public UInt32RecursionSubsetSumSolverTests()
        {
            subsetSumSolver = new UInt32RecursionSubsetSumSolver(Substitute.For<ILogger<UInt32RecursionSubsetSumSolver>>());
        }
        
        [Fact]
        public async Task SolveAsync_should_return_expected_success_result()
        {
            var result = await subsetSumSolver.SolveAsync(13, new uint[] { 3, 4, 6, 8 }, CancellationToken.None);
            result.Should().BeEquivalentTo(new uint[] { 3, 6, 4 });
        }

        [Fact]
        public async Task SolveAsync_should_return_null_when_no_result_exists()
        {
            var result = await subsetSumSolver.SolveAsync(13, new uint[] { 3, 4, 8 }, CancellationToken.None);
            result.Should().BeNull();
        }

        [Fact]
        public async Task SolveAsync_should_result_correctly_without_overflow()
        {
            var result = await subsetSumSolver.SolveAsync(uint.MaxValue - 1, new uint[] { uint.MaxValue - 2, uint.MaxValue - 3,  5}, CancellationToken.None);
            result.Should().BeNull();
        }
    }
}
