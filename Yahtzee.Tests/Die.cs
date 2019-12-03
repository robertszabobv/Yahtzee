using System;

namespace Yahtzee.Tests
{
    internal class Die
    {
        private static readonly Random ValueGenerator = new Random();

        public void Throw()
        {
            Value = ValueGenerator.Next(1, 6);
        }

        public int Value { get; private set; }
    }
}