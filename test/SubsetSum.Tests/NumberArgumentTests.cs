using System;
using Xunit;

namespace SubsetSum.Tests
{
    public class NumberArgumentTests
    {
        [Theory]
        [InlineData("9223372036854775807.871821321321363464352", "9223372036854775807", "871821321321363464352", false, false)]
        [InlineData("-100000000000000000000000000.00000000000000000000000001", "100000000000000000000000000", "00000000000000000000000001", true, false)]
        [InlineData("0.1", "", "1", false, false)]
        [InlineData("-0.1", "", "1", true, false)]
        [InlineData("1.1", "1", "1", false, false)]
        [InlineData("1", "1", "", false, false)]
        [InlineData("-1", "1", "", true, false)]
        [InlineData("+1", "1", "", false, false)]
        [InlineData("0", "", "", false, true)]
        [InlineData("+0", "", "", false, true)]
        [InlineData("-0", "", "", false, true)]
        [InlineData(".0", "", "", false, true)]
        [InlineData("+.0", "", "", false, true)]
        [InlineData("-.0", "", "", false, true)]
        [InlineData("0.", "", "", false, true)]
        [InlineData("+0.", "", "", false, true)]
        [InlineData("-0.", "", "", false, true)]
        [InlineData(".", "", "", false, true)]
        [InlineData("+.", "", "", false, true)]
        [InlineData("-.", "", "", false, true)]
        public void NumberArgument_can_parse_valid_numbers(string value, string integerPart, string fractionalPart, bool isNegative, bool isNeutral)
        {
            var number = NumberArgument.Parse(value);
            Assert.Equal(value, number.Original);
            Assert.Equal(fractionalPart, number.FractionalPart);
            Assert.Equal(integerPart, number.IntegerPart);
            Assert.Equal(isNegative, number.IsNegative);
            Assert.Equal(isNeutral, number.IsNeutral);
        }

        [Theory]
        [InlineData("--0")]
        [InlineData("++0")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0,1")]
        [InlineData("0.1.")]
        [InlineData("a")]
        [InlineData("๑")]
        [InlineData("0.-")]
        [InlineData("0.a")]
        public void NumberArgument_throws_argument_exception_while_parse_invalid_numbers(string value)
        {
            Assert.Throws<ArgumentException>(() => NumberArgument.Parse(value));
        }
    }
}
