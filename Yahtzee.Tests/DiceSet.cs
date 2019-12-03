using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Tests
{
    internal class DiceSet
    {
        private const int TotalNumberOfDice = 5;
        private readonly Die[] dice;
        private readonly List<Die> frozenDice = new List<Die>(TotalNumberOfDice);

        public DiceSet(Die[] dice)
        {
            if (dice == null  || dice.Length != TotalNumberOfDice)
            {
                throw new InvalidOperationException("Dice set must contain at most 5 dice");
            }

            this.dice = dice;
        }

        public void Roll()
        {
            foreach (Die die in dice.Except(frozenDice))
            {
                die.Throw();
            }
        }

        internal void Hold(int dieIndex)
        {
            frozenDice.Add(dice[dieIndex]);
        }
    }
}