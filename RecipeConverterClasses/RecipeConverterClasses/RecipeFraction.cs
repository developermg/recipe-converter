using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// RecipeFraction is a NonNegativeFraction with string representations ideal for recipes
    /// </summary>
    public class RecipeFraction : NonNegativeFraction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">RecipeFraction numerator</param>
        /// <param name="denominator">RecipeFractionDenominator</param>
        public RecipeFraction(int numerator, int denominator) : base(numerator, denominator) { }

        //Overloaded operators required in class definition because cannot use overloaded operators from parent class as returns type of parent class

        /// <summary>
        /// Overloaded + operator allows for more natural addition of a Fraction to a RecipeFraction
        /// </summary>
        /// <param name="addend1">RecipeFraction addend</param>
        /// <param name="addend2">Fraction addend</param>
        /// <returns>Sum of addends</returns>
        public static RecipeFraction operator +(RecipeFraction addend1, Fraction addend2)
        {
            RecipeFraction fracCopy = addend1.Copy();
            fracCopy.Add(addend2);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded + operator allows for more natural addition of integers to Fractions
        /// </summary>
        /// <param name="fractionAddend">RecipeFraction addend</param>
        /// <param name="integerAddend">Integer addend</param>
        /// <returns>Sum of addends</returns>
        public static RecipeFraction operator +(RecipeFraction fractionAddend, int integerAddend)
        {
            RecipeFraction fracCopy = fractionAddend.Copy();
            fracCopy.Add(integerAddend);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded - operator allows for more natural subtraction of a Fraction from a RecipeFraction
        /// </summary>
        /// <param name="minuend">RecipeFraction minuend</param>
        /// <param name="subtrahend">Fraction subtrahend</param>
        /// <returns>RecipeFraction difference</returns>
        public static RecipeFraction operator -(RecipeFraction minuend, Fraction subtrahend)
        {
            RecipeFraction fracCopy = minuend.Copy();
            fracCopy.Subtract(subtrahend);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded - operator allows for more natural subtraction of an integer from a RecipeFraction
        /// </summary>
        /// <param name="minuend">RecipeFraction minuend</param>
        /// <param name="subtrahend">Integer subtrahend</param>
        /// <returns>RecipeFraction difference</returns>
        public static RecipeFraction operator -(RecipeFraction minuend, int subtrahend)
        {
            RecipeFraction fracCopy = minuend.Copy();
            fracCopy.Subtract(subtrahend);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded * operator allows for more natural multiplication of a RecipeFraction by a Fraction
        /// </summary>
        /// <param name="factor1">RecipeFraction factor</param>
        /// <param name="factor2">Fraction factor</param>
        /// <returns>Product of factors</returns>
        public static RecipeFraction operator *(RecipeFraction factor1, Fraction factor2)
        {
            RecipeFraction fracCopy = factor1.Copy();
            fracCopy.MultiplyBy(factor2);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded * operator allows for more natural multiplication of a RecipeFraction by an integer
        /// </summary>
        /// <param name="fractionFactor">RecipeFraction factor</param>
        /// <param name="integerFactor">Integer factor</param>
        /// <returns>Product of factors</returns>
        public static RecipeFraction operator *(RecipeFraction fractionFactor, int integerFactor)
        {
            RecipeFraction fracCopy = fractionFactor.Copy();
            fractionFactor.MultiplyBy(integerFactor);
            return fractionFactor;
        }

        /// <summary>
        /// Overloaded / operator allows for more natural division of a RecipeFraction by a Fraction
        /// </summary>
        /// <param name="dividend">RecipeFraction dividend</param>
        /// <param name="divisor">Fraction divisor</param>
        /// <returns>Fraction quotient</returns>
        public static RecipeFraction operator /(RecipeFraction dividend, Fraction divisor)
        {
            RecipeFraction fracCopy = dividend.Copy();
            fracCopy.DivideBy(divisor);
            return fracCopy;
        }

        /// <summary>
        /// Overloaded / operator allows for more natural division of a RecipeFraction by an integer
        /// </summary>
        /// <param name="dividend">RecipeFraction dividend</param>
        /// <param name="divisor">Integer divisor</param>
        /// <returns>RecipeFraction quotient</returns>
        public static RecipeFraction operator /(RecipeFraction dividend, int divisor)
        {
            RecipeFraction fracCopy = dividend.Copy();
            dividend.DivideBy(divisor);
            return dividend;
        }

        /// <summary>
        /// The ToString method returns a string representation of the fractionAddend
        /// </summary>
        /// <returns>String representation of the fractionAddend</returns>
        public override string ToString()
        {
            Simplify();
            //does not return the denominator as part of the string if the denominator is 1
            if (Denominator == 1)
            {
                return Numerator.ToString();
            }
            else
            {
                int wholePart = WholePart();
                if (wholePart > 0)
                {
                    return wholePart + " " + (Numerator - (Denominator * wholePart) + "/" + Denominator);
                }
                else
                {
                    return base.ToString();
                }
            }
        }

        /// <summary>
        /// The ToHTMLFormattedString method returns a string that includes HTML tags to better display fractions
        /// </summary>
        /// <returns>String containing HTML-formatted representation of RecipeFraction </returns>
        public override string ToHTMLFormattedString()
        {
            Simplify();
            //does not return the denominator as part of the string if the denominator is 1
            if (Denominator == 1)
            {
                return Numerator.ToString();
            }
            else
            {
                int wholePart = WholePart();
                if (wholePart > 0)
                {
                    return wholePart.ToString() + "<sup>" + (Numerator - (Denominator * wholePart) + "</sup>&frasl;<sub>" + Denominator + "</sub>");

                }
                else
                {
                    return base.ToHTMLFormattedString();
                }
            }
        }

        /// <summary>
        /// The Copy method returns a deep copy of the RecipeFraction
        /// </summary>
        /// <returns>RecipeFraction copy</returns>
        public new RecipeFraction Copy()
        {
            return new RecipeFraction(Numerator, Denominator);
        }


    }
}
