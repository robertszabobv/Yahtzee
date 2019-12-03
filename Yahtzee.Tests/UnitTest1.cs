using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private const int TotalNumberOfDice = 5;

        [TestMethod]
        public void ADieYieldsAScoreBetween1and6()
        {
            var die = new Die();
            die.Throw();

            Assert.IsTrue(1 <= die.Value && die.Value <= 6);
        }

        [TestMethod]
        public void RollingADiceSetRollsAllDice()
        {
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);
            diceSet.Roll();

            Assert.IsTrue(dice.All(IsValueValidFor));
        }

        private static bool IsValueValidFor(Die d)
        {
            return 1 <= d.Value && d.Value <= 6;
        }

        private Die[] CreateDice(int howMany)
        {
            var diceList = new List<Die>(howMany);
            for (int i = 0; i < howMany; i++)
            {
                var die = new Die();
                diceList.Add(die);
            }

            return diceList.ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ADiceSetCannotContainMoreThan5Dice()
        {
            const int invalidNumberOfDice = 6;
            Die[] dice = CreateDice(invalidNumberOfDice);
            var diceSet = new DiceSet(dice);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ADiceSetCannotContainLessThan5Dice()
        {
            const int invalidNumberOfDice = 4;
            Die[] dice = CreateDice(invalidNumberOfDice);
            var diceSet = new DiceSet(dice);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ADiceSetCannotNullDice()
        {
            Die[] nullDice = null;
            var diceSet = new DiceSet(nullDice);
        }

        [TestMethod]
        public void HoldingADieAfterTheFirstThrowWillFreezeTheValueOfThatDieOnTheSecondThrow()
        {
            const int secondDieIndex = 1;
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);
            diceSet.Roll();
            int valueOfTheDiceBeingHold = dice[secondDieIndex].Value;
            diceSet.Hold(secondDieIndex);
            diceSet.Roll();

            Assert.AreEqual(valueOfTheDiceBeingHold, dice[secondDieIndex].Value);
        }

        [TestMethod]
        public void Holding2DiceAfterTheFirstThrowWillFreezeTheValuesOfThoseDiceOnTheSecondThrow()
        {
            const int secondDieIndex = 1;
            const int forthDieIndex = 3;
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);
            diceSet.Roll();
            int valueOfTheSecondDieBeingHold = dice[secondDieIndex].Value;
            int valueOfTheForthDieBeingHold = dice[forthDieIndex].Value;
            diceSet.Hold(secondDieIndex);
            diceSet.Hold(forthDieIndex);
            diceSet.Roll();

            Assert.AreEqual(valueOfTheSecondDieBeingHold, dice[secondDieIndex].Value);
            Assert.AreEqual(valueOfTheForthDieBeingHold, dice[forthDieIndex].Value);
        }


    }
}
