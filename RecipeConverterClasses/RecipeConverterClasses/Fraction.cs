using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    public class Fraction : IComparable<Fraction>
    {
        //integers to hold the fraction 
        private int _numerator;
        private int _denominator;

        //getters and setters
        protected virtual int Numerator
        {
            get
            {
                return _numerator;
            }
            set
            {
                _numerator = value;
            }
        }
        protected virtual int Denominator
        {
            get
            {
                return _denominator;
            }
            set
            {
                _denominator = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Fraction numerator</param>
        /// <param name="denominator">Fraction denominator</param>
        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// No-arg constructor allows for child classes with validation before construction
        /// </summary>
        public Fraction()
        {
        }

        /// <summary>
        /// The Reciprocal method returns the reciprocal of the fraction
        /// </summary>
        /// <returns>Fraction object representing reciprocal of this fraction object</returns>
        public Fraction Reciprocal()
        {
            return new Fraction(Denominator, Numerator);
        }

       
        public static  Fraction operator +(Fraction fraction1, Fraction fraction2)
        {

            Fraction fracCopy = fraction1.Copy();
            fracCopy.Add(fraction2);
            return fracCopy;
        }
        public static Fraction operator +(Fraction fraction, int num)
        {
            Fraction fracCopy = fraction.Copy();
            fracCopy.Add(num);
            return fracCopy;
        }

        public static Fraction operator -(Fraction fraction1, Fraction fraction2)
        {
            Fraction fracCopy = fraction1.Copy();
            fracCopy.Subtract(fraction2);
            return fracCopy;
        }

        public static Fraction operator -(Fraction fraction, int num)
        {
            Fraction fracCopy = fraction.Copy();
            fracCopy.Subtract(num);
            return fracCopy;
        }
        public static Fraction operator *(Fraction fraction1, Fraction fraction2)
        {
            Fraction fracCopy = fraction1.Copy();
            fracCopy.MultiplyBy(fraction2);
            return fracCopy;
        }

        public static Fraction operator *(Fraction fraction, int num)
        {
            Fraction fracCopy = fraction.Copy();
            fraction.MultiplyBy(num);
            return fraction;
        }

        public static Fraction operator /(Fraction fraction1, Fraction fraction2)
        {
            Fraction fracCopy = fraction1.Copy();
            fracCopy.DivideBy(fraction2);
            return fracCopy;
        }

        public static Fraction operator /(Fraction fraction, int num)
        {
            Fraction fracCopy = fraction.Copy();
            fraction.DivideBy(num);
            return fraction;
        }

        /// <summary>
        ///Add the passed fraction to this fraction 
        /// </summary>
        /// <param name="fraction"></param>
        protected virtual void Add(Fraction fraction)
        {
            SetToCommonDenominators(this, fraction);
            Numerator += fraction.Numerator;
        }

        /// <summary>
        /// Add an integer to the fraction
        /// </summary>
        /// <param name="num">Integer to add to the fraction</param>
        protected virtual void Add(int num)
        {
            Numerator += num * Denominator;
        }

        /// <summary>
        /// Subtract the passed fraction to this fraction
        /// </summary>
        /// <param name="fraction">Fraction to subtract from this fraction</param>
        protected virtual void Subtract(Fraction fraction)
        {
            fraction = fraction.Copy();
            fraction.Numerator *= -1;

            Add(fraction);
        }

        /// <summary>
        /// Overloaded Subtact method allows for subtraction by integers
        /// </summary>
        /// <param name="num">Integer to subtract from fraction</param>
        protected virtual void Subtract(int num)
        {
            num *= -1;
            Add(num);
        }

        /// <summary>
        /// Multiply this fraction by the passed fraction
        /// </summary>
        /// <param name="fraction">Fraction by which to multiply this fraction</param>
        protected virtual void MultiplyBy(Fraction fraction)
        {
            Numerator *= fraction.Numerator;
            Denominator *= fraction.Denominator;
        }

        /// <summary>
        /// Overloaded MultiplyBy method allows for multiplication by an integer, rather than a fraction
        /// </summary>
        /// <param name="multiplier">Integer by which to multiply fraction</param>
        protected virtual void MultiplyBy(int multiplier)
        {
            Numerator *= multiplier;
        }

        /// <summary>
        /// Divide this fraction by the passed fraction
        /// </summary>
        /// <param name="fraction">Fraction by which to divide this fraction</param>
        protected virtual void DivideBy(Fraction fraction)
        {
            if (fraction.Numerator == 0)
            {
                throw new DivideByZeroException("Cannot divide by a fraction with a numerator of 0.");
            }
            MultiplyBy(fraction.Reciprocal());
        }

        /// <summary>
        /// Divide this fraction by the passed fraction
        /// </summary>
        /// <param name="divisor">Number by which to divide the fraction</param>
        protected virtual void DivideBy(int divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            Denominator *= divisor;
        }

        /// <summary>
        /// The Div function performs the equivelant of integer division (division returning an int) on a fraction
        /// </summary>
        /// <param name="dividend">Fraction to divide</param>
        /// <param name="divisor">Fraction by which to divide the dividend</param>
        /// <returns>integer result of division</returns>
        public static int Div(Fraction dividend, Fraction divisor)
        {
            dividend.DivideBy(divisor);
            return dividend.WholePart();
        }
        /// <summary>
        /// ToString method returns a string representation of the Fraction
        /// </summary>
        /// <returns>Fraction in numerator/denominator format</returns>
        public override string ToString()
        {
            return Numerator + @"/" + Denominator;
        }
        public virtual string ToHTMLFormattedString()
        {
            return "<sup>" + Numerator + "</sup>&frasl;<sub>" + Denominator + "</sub>";
        }
        /// <summary>
        /// The Equals method returns whether a Fraction is the equivelant object to this object
        /// Does not return true if mathematical values are equivelant if objects do not contain the same numerators
        /// and denominators
        /// </summary>
        /// <param name="fraction">Fraction to compare to</param>
        /// <returns>Whether the Fraction is equal to this fraction</returns>
        public bool Equals(Fraction fraction)
        {
            return Numerator == fraction.Numerator && Denominator == fraction.Denominator;
        }

        /// <summary>
        /// The EqualsValue method returns whether a given integer is the equivalant value of the Fraction
        /// </summary>
        /// <param name="num">Integer to compare to Fraction</param>
        /// <returns>Whether integer value is equal to the value of the Fraction</returns>
        public bool EqualsValue(int num)
        {
            return WholePart() == num && Numerator % Denominator == 0;
        }
        /// <summary>
        /// The EqualsValue method returns whether a Fraction's mathematical value
        /// is equivalent to the value of this fraction
        /// </summary>
        /// <param name="fraction">Fraction to compare to this Fraction</param>
        /// <returns>Whether fractions have equal mathematical values</returns>
        public bool EqualsValue(Fraction fraction)
        {
            return Numerator * fraction.Denominator == fraction.Numerator * Denominator;
        }

        /// <summary>
        /// The CompareTo fraction is implemented as part of the CompareTo interface.
        /// </summary>
        /// <param name="other">Fraction to compare to this fraction</param>
        /// <returns>Positive number if this fraction is greater, negative number if the passed fraction is greater, zero if they are equal</returns>
        public int CompareTo(Fraction other)
        {
            SetToCommonDenominators(this, other);
            return Numerator - other.Numerator;
        }

        /// <summary>
        /// Overloaded CompareTo method
        /// Allows comparisons with integers
        /// </summary>
        /// <param name="num">Integer to compare to Fraction</param>
        /// <returns>Positive number if this fraction is greater, negative number if the passed number is greater, zero if they are equal</returns>
        public int CompareTo(int num)
        {
            int wholePart = WholePart();
            int remainder = Numerator % Denominator;
            if (wholePart == num)
            {
                if (remainder == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else 
            {
                return wholePart - num;
            }
            //return CompareTo(new Fraction(num, 1));
        }

        /// <summary>
        /// The WholePart method gets what would be the whole number part of a simplified fraction 
        /// </summary>
        /// <returns>Whole part of fraction when simplified</returns>
        public int WholePart()
        {
            return Numerator / Denominator;
        }

        /// <summary>
        /// The Copy method copies the fraction
        /// </summary>
        /// <returns>Copy of the Fraction object</returns>
        public virtual Fraction Copy()
        {
            return new Fraction(Numerator, Denominator);
        }

        /// <summary>
        /// Simplify the fraction. 
        /// A fraction with a numerator greater than the denominator can still be considered simplified in this context,
        /// as there is no whole part of the fraction 
        /// </summary>
        public void Simplify()
        {
            int gcd = GetGreatestCommonDivisor(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;
        }


        /// <summary>
        /// The SetToCommonDenominators method sets two fractions to their least common-denominator
        /// </summary>
        /// <param name="fraction1"></param>
        /// <param name="fraction2"></param>
        public static void SetToCommonDenominators(Fraction fraction1, Fraction fraction2)
        {
            //if the fractions do not have the same denominator, set to common denominator
            if (fraction1.Denominator != fraction2.Denominator)
            {
                int lcm = GetLeastCommonMultiple(fraction1.Denominator, fraction2.Denominator);
                //set each fraction to the equivelant fraction with the least common multiple as the denominator
                fraction1.Numerator *= lcm / fraction1.Denominator;
                fraction1.Denominator *= lcm / fraction1.Denominator;

                fraction2.Numerator *= lcm / fraction2.Denominator;
                fraction2.Denominator *= lcm / fraction2.Denominator;
            }
        }
        /// <summary>
        /// The GetLeastCommonMultiple method returns the least common multiple of two integers
        /// </summary>
        /// <param name="a">Integer to find common multiple for</param>
        /// <param name="b">Integer to find common multiple for</param>
        /// <returns>Least common multiple</returns>
        private static int GetLeastCommonMultiple(int a, int b)
        {
            return (Math.Abs(a * b)) / GetGreatestCommonDivisor(a, b);
        }

        /// <summary>
        /// The GetGreatestCommonDivisor method returns the greatest common divisor of two integers.
        /// </summary>
        /// <param name="a">One integer for which to find common divisor</param>
        /// <param name="b">Second integer for which to find common divisor</param>
        /// <returns></returns>
        private static int GetGreatestCommonDivisor(int a, int b)
        {
            // Uses Euclid's algorithm 
            if (b != 0)
            {
                int r = 1;
                while (r != 0)
                {
                    r = a % b;
                    a = b;
                    b = r;
                }
            }

            return a;
        }


    }


}
