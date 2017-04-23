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
            //Sets values using the getter/setters to ensure valid values are passed
            Numerator = numerator;
            Denominator = denominator;

        }

        /// <summary>
        /// Getter/setter for numerator
        /// If non-positive value is passed, throws an exception
        /// </summary>
        protected override int Numerator
        {
            get
            {
                return base.Numerator;
            }
            set
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
        protected override int Denominator
        {
            get
            {
                return base.Denominator;
            }
            set
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

        public static  NonNegativeFraction operator +(NonNegativeFraction fraction1, Fraction fraction2)
        { 
            NonNegativeFraction fracCopy = fraction1.Copy();
            fracCopy.Add(fraction2);
            return fracCopy;
           
        }

        public static NonNegativeFraction operator +(NonNegativeFraction fraction, int num)
        {
            NonNegativeFraction fracCopy = fraction.Copy();
            fracCopy.Add(num);
            return fracCopy;
        }

        public static NonNegativeFraction operator -(NonNegativeFraction fraction1, Fraction fraction2)
        {
            NonNegativeFraction fracCopy = fraction1.Copy();
            fracCopy.Add(fraction2);
            return fracCopy;
        }

        public static NonNegativeFraction operator -(NonNegativeFraction fraction, int num)
        {
            NonNegativeFraction fracCopy = fraction.Copy();
            fraction.Add(fraction);
            return fraction;
        }
        /// <summary>
        /// The Add method adds a Fraction to the NonNegativeFraction
        /// If the passed Fraction would cause the NonNegativeFraction to be negative, throws an ArgumentOutOfRangeException
        /// </summary>
        /// <param name="fraction">Fraction to add</param>
        protected override void Add(Fraction fraction)
        {
            //if the fraction is less than zero, checks if it will cause the 
            if (fraction.CompareTo(0) < 0)
            {
                Fraction checkingFrction = Copy();
                checkingFrction+=fraction;
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
            fraction.MultiplyBy(-1);
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
        public override void MultiplyBy(Fraction fraction)
        {
            if (fraction.CompareTo(0) < 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot multiply NonNegativeFraction by a negative number.");
            }
            base.MultiplyBy(fraction);
        }

        /// <summary>
        /// The MultiplyBy method multiplies the NonNegativeFraction by an integer
        /// Throws an ArgumentOutOfRangeException if integer is less than zero
        /// </summary>
        /// <param name="multiplier">Integer by which to multiply</param>
        public override void MultiplyBy(int multiplier)
        {
            if (multiplier < 0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot multiply NonNegativeFraction by a negative number.");
            }
            base.MultiplyBy(new Fraction(multiplier, 1));
        }

        /// <summary>
        /// The DivideBy method divides the NonNegativeFraction by a Fraction
        /// Throws an ArgumentOutOfRangeException if divisor is less than  zero
        /// </summary>
        /// <param name="divisor">Divisor fraction</param>
        public override void DivideBy(Fraction divisor)
        {
            MultiplyBy(divisor.Reciprocal());
        }

        /// <summary>
        /// The DivideBy method divides the NonNegativeFraction by an integer
        /// Throws an ArgumentOutOfRangeException if integer is less than zero
        /// </summary>
        /// <param name="divisor"></param>
        public override void DivideBy(int divisor)
        {
            //does check here to avoid using Fraction.CompareTo when not necessary
            if (divisor <=0)
            {
                throw new ArgumentOutOfRangeException("fraction", "Cannot divide NonNegativeFraction by a non-positive number.");
            }
            /**
             * Calls base because already verified that it is a valid value, 
             *  and does not need to be validated by NonNegativeFraction's MultiplyBy function
             */
            base.MultiplyBy(new Fraction(1, divisor));
        }
        /// <summary>
        /// The Copy method copies the fraction
        /// </summary>
        /// <returns>Copy of the Fraction object</returns>
        public new NonNegativeFraction Copy()
        {
            return new NonNegativeFraction(Numerator, Denominator);
        }

    }
}
