using System;
using System.Linq;

namespace Yahtzee.Tests
{
    internal class Turn
    {
        private DiceSet diceSet;

        public Turn(DiceSet diceSet)
        {
            this.diceSet = diceSet;
        }

        public int GetScore(int upperScoresCategory)
        {
            if (upperScoresCategory == 0)
            {
                throw new InvalidOperationException();
            }

            if (diceSet.NumberOfRolls != 3)
            {
                throw new InvalidOperationException();
            }

            Die[] diceShowingCategoryValue = diceSet.GetAllDiceShowingValue(upperScoresCategory);

            return diceShowingCategoryValue.Sum(d => d.Value);
        }
    }
}