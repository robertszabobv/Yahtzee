using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Yahtzee.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private const int TotalNumberOfDice = 5;
        private int scoreCategoryIsIrelevant = 0;

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

            HoldAndRoll(diceSet, secondDieIndex, forthDieIndex);
            
            Assert.AreEqual(valueOfTheSecondDieBeingHold, dice[secondDieIndex].Value);
            Assert.AreEqual(valueOfTheForthDieBeingHold, dice[forthDieIndex].Value);
        }

        [TestMethod]
        public void Holding2DiceAfterTheSecondThrowWillFreezeTheValuesOfThoseDiceOnTheThirdThrow()
        {
            const int secondDieIndex = 1;
            const int forthDieIndex = 3;
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);
            diceSet.Roll();

            HoldAndRoll(diceSet, secondDieIndex, forthDieIndex);
            
            const int firstDieIndex = 0;
            const int thirdDieIndex = 2;
            int valueOfTheFirstDieBeingHold = dice[firstDieIndex].Value;
            int valueOfTheThirdDieBeingHold = dice[thirdDieIndex].Value;

            HoldAndRoll(diceSet, firstDieIndex, thirdDieIndex);
            
            Assert.AreEqual(valueOfTheFirstDieBeingHold, dice[firstDieIndex].Value);
            Assert.AreEqual(valueOfTheThirdDieBeingHold, dice[thirdDieIndex].Value);
        }

        private void HoldAndRoll(DiceSet diceSet, params int[] diceIndexes)
        {
            for (int dieIndex = 0; dieIndex < diceIndexes.Length; dieIndex++)
            {
                diceSet.Hold(diceIndexes[dieIndex]);
            }
            diceSet.Roll();
        }

        [TestMethod]
        public void NotHoldingAnyDieOnTheFirstThrowWillRollAllDiceInTheSet()
        {
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);
            diceSet.Roll();
            diceSet.Roll();

            Assert.IsTrue(dice.All(d => d.NumberOfThrows == 2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndingATurnContainingMoreThan3RollsOfTheDiceSetThrowsException()
        {
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);

            var turn = new Turn(diceSet);
            diceSet.Roll();
            diceSet.Roll();
            diceSet.Roll();
            diceSet.Roll();
            turn.GetScore(scoreCategoryIsIrelevant);
        }

        [TestMethod]
        public void EndingATurnContainingExactly3RollsOfTheDiceSetDoesNotThrowException()
        {
            const int upperScoreCategory = 3;
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);

            var turn = new Turn(diceSet);
            diceSet.Roll();
            diceSet.Roll();
            diceSet.Roll();
            turn.GetScore(upperScoreCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndingATurnContainingLessThan3RollsOfTheDiceSetThrowsException()
        {
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);

            var turn = new Turn(diceSet);
            diceSet.Roll();
            diceSet.Roll();
            turn.GetScore(scoreCategoryIsIrelevant);
        }

        [TestMethod]
        public void ScoringATurnInTheUpperScoresCategory_SumsTheValuesOfTheChosenCategory()
        {
            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);

            const int upperScoresCategory = 3;

            var turn = new Turn(diceSet);
            diceSet.Roll();
            diceSet.Roll();
            diceSet.Roll();
            int actualScore = turn.GetScore(upperScoresCategory);

            Assert.IsTrue(actualScore > 0);
            Assert.IsTrue(actualScore % 3 == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScoringATurnInTheUpperScoresCategory_WithAZeroCategoryValueThrowsException()
        {
            const int invalidCategoryValue = 0;

            Die[] dice = CreateDice(TotalNumberOfDice);
            var diceSet = new DiceSet(dice);

            var turn = new Turn(diceSet);
            diceSet.Roll();
            diceSet.Roll();
            diceSet.Roll();
            turn.GetScore(invalidCategoryValue);
        }


    }
}
