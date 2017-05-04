using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// PostiveFraction is a Fraction that only allows positive values
    /// </summary>
    public class NonNegativeFraction : Fraction
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="numerator">Fraction numerator</param>
        /// <param name="denominator">Fraction denominator</param>
        public NonNegativeFraction(int numerator, int denominator)
        {
            //Sets values using properties to ensure valid values have been passed
            Numerator = numerator;
            Denominator = denominator;

        }

        /// <summary>
        /// Getter/setter for numerator
        /// If non-positive value is passed, throws an exception
        /// </summary>
        public override int Numerator
        {
            get
            {
                return base.Numerator;
            }
            protected set
            {
                if (value >= 0)
                {
                    base.Numerator = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "Numerator must be non-negative.");
                }
            }
        }
        /// <summary>
        ///Getter/setter for denominator
        ///If non-positive value is passed, throws exception
        /// </summary>
        public override int Denominator
        {
            get
            {
                return base.Denominator;
            }
            protected set
            {
                if (value > 0)
                {
                    base.Denominator = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "Denominator cannot be non-positive.");
                }

            }
        }

        /// <summary>
        /// Overloaded + operator allows for more natural Fraction addition
        /// </summary>
        /// <param name="addend1">NonNegativeFraction addend</param>
        /// <param name="addend2">Fraction addend</param>
        /// <returns>Sum of addends</returns>
        public static NonNegativeFraction operator +(NonNegativeFraction addend1, Fraction addend2)
        {
            NonNegativeFraction fracCopy = addend1.Copy();
            fracCopy.Add(addend2);
            return fracCopy;

        }

        /// <summary>
        /// Overloaded + operator allows for more natural addition of integers to Fractions
        /// </summary>
        /// <param name="fractionAddend">NonNegativeFraction addend</param>
        /// <param name="integerAddend">Integer addend</param>
        /// <returns>Sum of addends</returns>
        public static NonNegativeFraction operator +(NonNegativeFraction fractionAddend, int integerAddend)
        {
            NonNegativeFraction fracCopy = fractionAddend.Copy();
            fracCopy.Add(integerAddend);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded - operator allows for more natural subtraction of a Fraction from a Fraction
        /// </summary>
        /// <param name="minuend">NonNegativeFraction minuend</param>
        /// <param name="subtrahend">Fraction subtrahend</param>
        /// <returns>NonNegativeFraction difference</returns>
        public static NonNegativeFraction operator -(NonNegativeFraction minuend, Fraction subtrahend)
        {
            NonNegativeFraction fracCopy = minuend.Copy();
            fracCopy.Subtract(subtrahend);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded - operator allows for more natural subtraction of an integer from a Fraction
        /// </summary>
        /// <param name="minuend">NonNegativeFraction minuend</param>
        /// <param name="subtrahend">Integer subtrahend</param>
        /// <returns>NonNegativeFraction difference</returns>
        public static NonNegativeFraction operator -(NonNegativeFraction fraction, int num)
        {
            NonNegativeFraction fracCopy = fraction.Copy();
            fraction.Subtract(num);
            return fraction;
        }

        /// <summary>
        /// Overloaded * operator allows for more natural multiplication of Fraction objects
        /// </summary>
        /// <param name="factor1">NonNegativeFraction factor</param>
        /// <param name="factor2">Fraction factor</param>
        /// <returns>Product of factors</returns>
        public static NonNegativeFraction operator *(NonNegativeFraction factor1, Fraction factor2)
        {
            NonNegativeFraction fracCopy = factor1.Copy();
            fracCopy.MultiplyBy(factor2);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded * operator allows for more natural multiplication of a Fraction by an integer
        /// </summary>
        /// <param name="fractionFactor">NonNegativeFraction factor</param>
        /// <param name="integerFactor">Integer factor</param>
        /// <returns>Product of factors</returns>
        public static NonNegativeFraction operator *(NonNegativeFraction fractionFactor, int integerFactor)
        {
            NonNegativeFraction fracCopy = fractionFactor.Copy();
            fractionFactor.MultiplyBy(integerFactor);
            return fractionFactor;
        }

        /// <summary>
        /// Overloaded / operator allows for more natural division of Fractions
        /// </summary>
        /// <param name="dividend">NonNegativeFraction dividend</param>
        /// <param name="divisor">Fraction divisor</param>
        /// <returns>NonNegativeFraction quotient</returns>
        public static NonNegativeFraction operator /(NonNegativeFraction dividend, Fraction divisor)
        {
            NonNegativeFraction fracCopy = dividend.Copy();
            fracCopy.DivideBy(divisor);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded / operator allows for more natural division of a Fraction by an integer
        /// </summary>
        /// <param name="dividend">NonNegativeFraction dividend</param>
        /// <param name="divisor">Integer divisor</param>
        /// <returns>NonNegativeFraction quotient</returns>
        public static NonNegativeFraction operator /(NonNegativeFraction dividend, int divisor)
        {
            NonNegativeFraction fracCopy = dividend.Copy();
            dividend.DivideBy(divisor);
            return dividend;
        }
        /// <summary>
        /// The Add method adds a Fraction to the NonNegativeFraction
        /// If the passed Fraction would cause the NonNegativeFraction to be negative, throws an ArgumentOutOfRangeException
        /// </summary>
        /// <param name="fraction">Fraction to add</param>
        protected override void Add(Fraction fraction)
        {
            //if the fractionAddend is less than zero, checks if it will cause the 
            if (fraction.CompareTo(0) < 0)
            {
                Fraction checkingFrction = Copy();
                checkingFrction += fraction;
                if (checkingFrction.CompareTo(0) < 0)
                {
                    throw new ArgumentOutOfRangeException("fraction", "Argument cannot make NonNegativeFraction negative.");
                }
            }

            base.Add(fraction);
        }

        /// <summary>
        /// Adds an integer to the NonNegativeFraction
        /// If the passed integer would cause the NonNegativeFraction to be negative, throws an ArgumentOutOfRangeException
        /// </summary>
        /// <param name="num">Integer to add to NonNegativeFraction</param>
        protected override void Add(int num)
        {
            if (num < 0)
            {
                Fraction checkingFrction = Copy();
                checkingFrction += num;
                if (checkingFrction.CompareTo(0) < 0)
                {
                    throw new ArgumentOutOfRangeException("fraction", "Argument cannot make NonNegativeFraction negative.");
                }
            }
            base.Add(num);

        }

        /// <summary>
        /// The Subtract method subtracts a Fraction from the NonNegativeFraction
        /// If the passed Fraction would cause the NonNegativeFraction to be negative, throws an ArgumentOutOfRangeException
        /// </summary>
        /// <param name="fraction">Fraction to subtract</param>
        protected override void Subtract(Fraction fraction)
        {
            fraction = fraction.Copy();
            fraction *= -1;
            Add(fraction);
        }

        /// <summary>
        /// The Subtract method subtracts an integer from the NonNegativeFraction
        /// If the passed integer would cause the NonNegativeFraction to be negative, throws an ArgumentOutOfRangeException
        /// </summary>
        /// <param name="num">Integer to subtract</param>
        protected override void Subtract(int num)
        {
            Add(num * -1);
        }

        /// <summary>
        /// The MultiplyBy method multiplies the NonNegativeFraction by a Fraction
        /// Throws an ArgumentOutOfRangeException if Fraction is less than zero
        /// </summary>
        /// <param name="fraction">Fraction by which to multiply</param>
        protected override void MultiplyBy(Fraction fraction)
        {
            if (fraction.CompareTo(0) < 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot multiply NonNegativeFraction by a negative fraction.");
            }
            base.MultiplyBy(fraction);
        }

        /// <summary>
        /// The MultiplyBy method multiplies the NonNegativeFraction by an integer
        /// Throws an ArgumentOutOfRangeException if integer is less than zero
        /// </summary>
        /// <param name="multiplier">Integer by which to multiply</param>
        protected override void MultiplyBy(int multiplier)
        {
            if (multiplier < 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot multiply NonNegativeFraction by a negative number.");
            }
            base.MultiplyBy(multiplier);
        }

        /// <summary>
        /// The DivideBy method divides the NonNegativeFraction by a Fraction
        /// Throws an ArgumentOutOfRangeException if divisor is less than  zero
        /// </summary>
        /// <param name="divisor">Divisor fractionAddend</param>
        protected override void DivideBy(Fraction divisor)
        {
            if (divisor.CompareTo(0) < 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot multiply NonNegativeFraction by a negative fraction.");
            }
            base.DivideBy(divisor);
        }

        /// <summary>
        /// The DivideBy method divides the NonNegativeFraction by an integer
        /// Throws an ArgumentOutOfRangeException if integer is less than zero
        /// </summary>
        /// <param name="divisor"></param>
        protected override void DivideBy(int divisor)
        {
            //does check here to avoid using Fraction.CompareTo when not necessary
            if (divisor <= 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot divide NonNegativeFraction by a non-positive number.");
            }
            /**
             * Calls base because already verified that it is a valid value, 
             *  and does not need to be validated by NonNegativeFraction's MultiplyBy function
             */
            base.DivideBy(divisor);
        }

        /// <summary>
        /// The Copy method copies the fractionAddend
        /// </summary>
        /// <returns>Copy of the Fraction object</returns>
        public new NonNegativeFraction Copy()
        {
            return new NonNegativeFraction(Numerator, Denominator);
        }

    }
}
