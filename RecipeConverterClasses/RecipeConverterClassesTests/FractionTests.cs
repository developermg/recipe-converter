using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeConverterClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses.Tests
{
    [TestClass()]
    public class FractionTests
    {
        [TestMethod()]
        public void Reciprocal()
        {
            //arrange
            Fraction fraction = new Fraction(-11, 34);
            Fraction expected = new Fraction(34, -11);
            //act
            Fraction returned = fraction.Reciprocal();
            //assert
            Assert.IsTrue(returned.Equals(expected));
        }
        [TestMethod()]
        public void AddFraction_Test()
        {
            //arrange
            Fraction fraction = new Fraction(1, 2);
            Fraction add = new Fraction(4, 3);
            Fraction expected = new Fraction(11,6);
            //act
            fraction.Add(add);
            //assert
            Assert.IsTrue(fraction.Equals(expected));
        }
        [TestMethod()]
        public void AddInteger_Test()
        {
            //arrange
            Fraction frac = new Fraction(3, 4);
            Fraction expected = new Fraction(7, 4);
            //act
            frac.Add(1);
            //assert
            Assert.IsTrue(frac.Equals(expected));
        }

        [TestMethod()]
        public void SubtractFraction_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(11, 45);
            Fraction frac2 = new Fraction(15, 24);
            Fraction expected = new Fraction(-137, 360);
            //act
            frac1.Subtract(frac2);
            //assert
            Assert.IsTrue(frac1.Equals(expected));
        }

        [TestMethod()]
        public void SubtractInt_Test()
        {
            //arrange
            Fraction frac = new Fraction(7, 2);
            int num = 4;
            Fraction expected = new Fraction(-1, 2);
            //act
            frac.Subtract(num);
            //assert
            Assert.IsTrue(frac.Equals(expected));
        }

        [TestMethod()]
        public void MultiplyByFraction_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(3, 42);
            Fraction frac2 = new Fraction(2, 2);
            Fraction expected = new Fraction(6, 84);
            //act
            frac1.MultiplyBy(frac2);
            //assert
            Assert.IsTrue(frac1.Equals(expected));
        }

        [TestMethod()]
        public void MultiplyByInt_Test()
        {
            //arrange
            Fraction frac = new Fraction(3, 42);
            int multiplier = 3;
            Fraction expected = new Fraction(9, 42);
            //act
            frac.MultiplyBy(multiplier);
            //assert
            Assert.IsTrue(frac.Equals(expected));
        }


        [TestMethod()]
        public void DivideByFraction_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(3, 42);
            Fraction frac2 = new Fraction(4, 6);
            Fraction expected = new Fraction(18, 168);
            //act
            frac1.DivideBy(frac2);
            //assert
            Assert.IsTrue(frac1.Equals(expected));
        }

        [TestMethod()]
        public void DivideByInt_Test()
        {
            //arrange
            Fraction frac = new Fraction(3, 42);
            int divisor = 3;
            Fraction expected = new Fraction(3, 126);
            //act
            frac.DivideBy(divisor);
            //assert
            Assert.IsTrue(frac.Equals(expected));
        }

        [TestMethod()]
        public void ToString_Test()
        {
            //arrange
            int num = 4;
            int den = 5;
            string expected = num + "/" + den;
            Fraction fraction = new Fraction(num,den);
            //act
            string returned = fraction.ToString();
            //assert
            Assert.AreEqual(expected, returned);
        }
        [TestMethod()]
        public void EqualsFraction_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(52, 23);
            Fraction frac2 = new Fraction(52, 23);
            //act
            bool equals = frac1.Equals(frac2);
            //assert
            Assert.IsTrue(equals);
        }

        [TestMethod()]
        public void EqualsInt_Test()
        {
            //arrange
            Fraction frac = new Fraction(-16, -4);
            int num = 4;
            //act
            bool equals = frac.EqualsValue(num);
            //assert
            Assert.IsTrue(equals);
        }

        [TestMethod()]
        public void CompareToFraction_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(3, 4);
            Fraction frac2 = new Fraction(-4, 3);
            int expected = 25;
            //act
            int actual = frac1.CompareTo(frac2);
            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CompareToInt_Test()
        {
            //arrange
            Fraction fraction = new Fraction(17, 8);
            int compareTo = 3;
            int expected = -7;
            //act
            int actual = fraction.CompareTo(compareTo);
            //assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void WholePart_Test()
        {
            //arrange
            Fraction fraction = new Fraction(17, 8);
            int expected = 2;
            //act
            int actual = fraction.WholePart();
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Copy_Test()
        {
            //arrange
            Fraction fraction = new Fraction(17, 8);
            Fraction expected = new Fraction(17, 8);
            //act
            Fraction copy = fraction.Copy();
            //assert
            Assert.IsTrue(expected.Equals(copy)&&(copy!=fraction));
        }
        [TestMethod()]
        public void Simplify_Test()
        {
            //arrange
            Fraction fraction = new Fraction(1100, 11);
            Fraction expected = new Fraction(100, 1);
            //act
            fraction.Simplify();
            //assert
            Assert.IsTrue(fraction.Equals(expected));
        }
        [TestMethod()]
        public void SetToCommonDenominators_Test()
        {
            //arrange
            Fraction frac1 = new Fraction(1, 2);
            Fraction frac2 = new Fraction(3,4);
            string expected = "2/4";
            //act
            Fraction.SetToCommonDenominators(frac1, frac2);
            //assert
            Assert.AreEqual(expected, frac1.ToString());
        }
    }
}