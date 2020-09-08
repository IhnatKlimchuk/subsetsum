using System;

namespace SubsetSum.Experimental
{
    public sealed class NumberArgument
    {
        private const char Dot = '.';
        private const char Minus = '-';
        private const char Plus = '+';
        private const char Zero = '0';
        private const char Nine = '9';

        public string Original { get; private set; }
        public string IntegerPart { get; private set; }
        public string FractionalPart { get; private set; }
        public bool IsNegative { get; private set; }
        public bool IsNeutral { get; private set; }

        private NumberArgument(string original, string integerPart, string fractionalPart, bool isNegative, bool isNeutral)
        {
            Original = original;
            IntegerPart = integerPart;
            FractionalPart = fractionalPart;
            IsNegative = isNegative;
            IsNeutral = isNeutral;
        }

        public static NumberArgument Parse(string original)
        {
            var number = original.AsSpan();
            
            ReadOnlySpan<char> integerPart;
            ReadOnlySpan<char> fractionalPart;
            int splitIndex = number.IndexOf(Dot);
            if (splitIndex >= 0)
            {
                integerPart = number.Slice(0, splitIndex);
                fractionalPart = number.Slice(splitIndex + 1);
            }
            else
            {
                integerPart = number;
                fractionalPart = ReadOnlySpan<char>.Empty;
            }

            bool isNegative = false;
            var firstCharacter = integerPart[0];
            if (firstCharacter == Minus)
            {
                isNegative = true;
                integerPart = integerPart.Slice(1);
            }
            else if (firstCharacter == Plus)
            {
                integerPart = integerPart.Slice(1);
            }

            integerPart = integerPart.TrimStart(Zero);
            fractionalPart = fractionalPart.TrimEnd(Zero);

            bool isNeutral = true;
            char unknownCharacter = default;
            if (!IsOnlyDigits(integerPart, ref unknownCharacter, ref isNeutral) || !IsOnlyDigits(fractionalPart, ref unknownCharacter, ref isNeutral))
            {
                throw new ArgumentException($"Unexpected symbol '{unknownCharacter}' in {number.ToString()}");
            }

            return new NumberArgument(original, integerPart.ToString(), fractionalPart.ToString(), isNegative, isNeutral);
        }

        private static bool IsOnlyDigits(ReadOnlySpan<char> number, ref char unknownCharacter, ref bool isNeutral)
        {
            foreach (var character in number)
            {
                if (character < Zero || character > Nine)
                {
                    unknownCharacter = character;
                    return false;
                }
                isNeutral &= character == Zero;
            }
            return true;
        }
    }
}
