using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSum
{
    public class RecursiveSubsetSumSolver : ISubsetSumSolver<NumberArgument>
    {
        private readonly ILogger logger;
        private readonly AlgorithmOptions options;
        private readonly CultureInfo cultureInfo;

        public RecursiveSubsetSumSolver(
            AlgorithmOptions options,
            CultureInfo cultureInfo,
            ILogger logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        }

        public async Task<IReadOnlyCollection<NumberArgument>> SolveAsync(NumberArgument sum, NumberArgument[] set, CancellationToken cancellationToken)
        {
            if (sum.IsNegative || set.Any(element => element.IsNegative))
            {
                throw new NotImplementedException("Negative numbers are not supported.");
            }

            NumberType minSupportedNumber = CalculateMinimalRequiredNumberType(sum, set);
            switch (minSupportedNumber)
            {
                case NumberType.Uint32:
                    return await Uint32SolveAsync(sum, set, cancellationToken);
                case NumberType.UInt64:
                case NumberType.BigInteger:
                default:
                    throw new NotImplementedException();
            }
        }



        private NumberType CalculateMinimalRequiredNumberType(NumberArgument sum, NumberArgument[] set)
        {
            NumberType value = CalculateMinimalRequiredNumberType(sum);
            foreach (var item in set)
            {
                NumberType currentValue = CalculateMinimalRequiredNumberType(item);
                value = value > currentValue ? value : currentValue;
            }
            return value;
        }

        private NumberType CalculateMinimalRequiredNumberType(NumberArgument number)
        {
            const int UInt32MaxValueLength = 10;
            const int UInt64MaxValueLength = 10;

            if (number.IsNeutral)
            {
                return NumberType.Uint32;
            }
            var length = number.IntegerPart.Length + number.FractionalPart.Length;
            if (length < UInt32MaxValueLength)
            {
                return NumberType.Uint32;
            }
            if (length < UInt64MaxValueLength)
            {
                return NumberType.UInt64;
            }
            return NumberType.BigInteger;
        }

        private async Task<IReadOnlyCollection<NumberArgument>> Uint32SolveAsync(NumberArgument sum, NumberArgument[] set, CancellationToken cancellationToken)
        {
            var map = MapNumberArguments(set, element => uint.Parse(element.IntegerPart, cultureInfo));
            var solver = new UInt32RecursionSubsetSumSolver(options, logger);
            var result = await solver.SolveAsync(uint.Parse(sum.IntegerPart, cultureInfo), map.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value.Count)).ToArray(), cancellationToken);
            return MapToOriginalValues(result, map);
        }

        private NumberArgument[] MapToOriginalValues<T>(IReadOnlyCollection<T> elements, IDictionary<T, Queue<NumberArgument>> map)
        {
            return elements.Select(element => map[element].Dequeue()).ToArray();
        }

        private IDictionary<T, Queue<NumberArgument>> MapNumberArguments<T>(NumberArgument[] set, Func<NumberArgument, T> convert)
        {
            var map = new Dictionary<T, Queue<NumberArgument>>();
            foreach (var item in set)
            {
                T value = convert(item);
                if (map.ContainsKey(value))
                {
                    map[value].Enqueue(item);
                }
                else
                {
                    var queue = new Queue<NumberArgument>();
                    queue.Enqueue(item);
                    map.Add(value, queue);
                }
            }
            return map;
        }
    }
}
