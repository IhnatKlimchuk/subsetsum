using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum
{
    public interface ISubsetSumSolver<T>
    {
        Task<IReadOnlyCollection<T>> SolveAsync(T sum, IEnumerable<T> set, CancellationToken cancellationToken);
    }
}
