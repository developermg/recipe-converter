using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// RecipeFraction is a PositiveFraction as desired for recipes
    /// </summary>
    public class RecipeFraction : NonNegativeFraction
    {
        
        public RecipeFraction(int numerator, int denominator) : base(numerator, denominator) { }

        /// <summary>
        /// The ToString method returns a string representation of the fraction
        /// </summary>
        /// <returns>String representation of the fraction</returns>
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
        /// The Copy method returns a copy of the RecipeFraction
        /// </summary>
        /// <returns>RecipeFraction copy</returns>
        public new RecipeFraction Copy()
        {
            return new RecipeFraction(Numerator, Denominator);
        }

     

    }
}
