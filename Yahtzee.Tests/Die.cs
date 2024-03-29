﻿using System;

namespace Yahtzee.Tests
{
    internal class Die
    {
        private static readonly Random ValueGenerator = new Random();
        private int numberOfThrows;


        public void Throw()
        {
            Value = ValueGenerator.Next(1, 6);
            numberOfThrows++;
        }

        public int Value { get; private set; }

        public int NumberOfThrows => numberOfThrows;
    }
}