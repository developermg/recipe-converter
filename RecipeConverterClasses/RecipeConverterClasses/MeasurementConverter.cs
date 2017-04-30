using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    public class MeasurementConverter
    {
        //Strings Regex for numbers:
        private const string INTEGER_PATTERN = @"(?<integer>\d+)";
        private const string SLASHES = @"(?:\u2044|\u002F|\u2215|[\u0337-\u0338])"; //different slash characters
        private const string SLASH_FRACTION_PATTERN = @"(?<slash_fraction>" + INTEGER_PATTERN + SLASHES + INTEGER_PATTERN + @")";
        private const string UNICODE_FRACTION_PATTERN = @"(?<unicode_fraction>[\u00BC-\u00BE]|[\u2150-\u215E])";
        private const string FRACTION_PATTERN = @"(?<fraction>" + UNICODE_FRACTION_PATTERN + @"|" + SLASH_FRACTION_PATTERN + @")";
        private const string MIXED_FRACTION_PATTERN = "(?<mixed_fraction>" + INTEGER_PATTERN + @"(?:\s*)" + FRACTION_PATTERN + ")";
        private const string RANGE_PATTERN = "(?<range>" + NUMBER_PATTERN + @"(\s*)-(\s*)" + NUMBER_PATTERN + ")";
        private const string NUMBER_PATTERN = "(?<number>" + MIXED_FRACTION_PATTERN + @"|" + FRACTION_PATTERN + @"|" + INTEGER_PATTERN + ")";

        //String for regex of units
        private static string EndOfUnitPattern = @"(?i)(?=$|\W)"; //after the unit must be end of line or a non-word character
        private static string SPattern = @"(?:\(s\)|s?)";
        private static string CupPattern = $@"(?i)(?:(?:cup{SPattern})|c){EndOfUnitPattern}";
        private static string TbspPattern = $@"(?i)(?:(?:tablespoon{SPattern})|(?:tbsp{SPattern})|(?:(?-i)T)){EndOfUnitPattern}";
        private static string TspPattern = $@"(?i)(?:(?:teaspoon{SPattern})|(?:tsp{SPattern})|(?:(?-i)t)){EndOfUnitPattern}";
        private static string OtherIngPattern = @"(?=\s\w)";

        //Pattern for measurements
        private string PATTERN = $@"(?:{RANGE_PATTERN}|{NUMBER_PATTERN})(?<unit>\s{CupPattern}|\s{TbspPattern}|\s{TspPattern}|{OtherIngPattern})";
        //text in which to find and replace measurements
        private string text;
        //amount by which to multiply measurements
        private NonNegativeFraction multiplier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">String in which to find and replace measurements</param>
        /// <param name="multiplier">Amount by which to multiply measurements</param>
        public MeasurementConverter(string text, NonNegativeFraction multiplier)
        {
            this.text = text;
            this.multiplier = multiplier;
        }

        /// <summary>
        /// The Convert method converts the measurements in the text
        /// </summary>
        /// <returns></returns>
        public string Convert()
        {
            return Regex.Replace(text, PATTERN, m => GetMeasurementReplacement(m));
        }

        /// <summary>
        /// The GetMeasurementReplacement method replaces a measurement 
        /// </summary>
        /// <param name="match">Match of measurement</param>
        /// <returns>String of converted measurement</returns>
        private string GetMeasurementReplacement(Match match)
        {
            Unit unit = GetUnitFromMatch(match);
            //if the measurement is a range (ie 1-2) convert each range endpoint separately
            if (Regex.Match(match.Value, RANGE_PATTERN).Success)
            {
                String replacement = "[" + GetMeasurementReplacement(match.Groups["number"].Captures[0].Value, unit) + "]";
                replacement += " <b>-</b> [" + GetMeasurementReplacement(match.Groups["number"].Captures[1].Value, unit) + "]";
                return replacement;
            }
            else
            {
                /* passes match.Value is valid string of amount even though may contain unit text
                as well because numeric portions will be extracted and other text ignored */
                return GetMeasurementReplacement(match.Value, unit);
            }

        }

        /// <summary>
        /// Overloaded GetMeasurementReplacement method replaces the measurement
        /// in the given text 
        /// </summary>
        /// <param name="amount">String containing measurement amount</param>
        /// <param name="unit">Unit of measurement</param>
        /// <returns></returns>
        private string GetMeasurementReplacement(String amount, Unit unit)
        {
            //gets the measurement as a fraction
            RecipeFraction fraction = GetFraction(amount);
            //perform the actual recipe conversion
            fraction *= multiplier;
            Measurement measurement = new Measurement(fraction, unit);
            //get user-friendly version of converted measurement
            ICollection<Measurement> measurements = measurement.UserFriendlyMeasurements();
            return String.Join(" + ", measurements.Select(i => i.ToHTMLFormattedString()));
        }

        /// <summary>
        /// The GetUnitFromMatch method determines the Unit of measurement from a Match
        /// </summary>
        /// <param name="match">Match of measurement</param>
        /// <returns>Unit of measurement</returns>
        private static Unit GetUnitFromMatch(Match match)
        {
            string unitText = match.Groups["unit"].Value;

            //check if matches cup pattern
            Match unitMatch = Regex.Match(unitText, CupPattern);
            if (unitMatch.Success)
            {
                return Unit.CUP;
            }

            //check if matches tablespoon pattern
            unitMatch = Regex.Match(unitText, TbspPattern);
            if (unitMatch.Success)
            {
                return Unit.TABLESPOON;
            }

            //check if matches teaspoon pattern
            unitMatch = Regex.Match(unitText, TspPattern);
            if (unitMatch.Success)
            {
                return Unit.TEASPOON;
            }

            //if doesn't match any, unit is OTHER
            else
            {
                return Unit.OTHER;
            }

        }


        /// <summary>
        /// The GetFraction method returns a RecipeFraction representing the value contained in a String
        /// </summary>
        /// <param name="str">String from which to extract fraction</param>
        /// <returns>RecipeFraction</returns>
        private static RecipeFraction GetFraction(String str)
        {
            Match match = Regex.Match(str, MIXED_FRACTION_PATTERN);
            if (match.Success)
            {
                return GetMixedFractionAsFraction(match);
            }
            match = Regex.Match(str, FRACTION_PATTERN);
            if (match.Success)
            {
                return GetFractionAsFraction(match);
            }
            match = Regex.Match(str, INTEGER_PATTERN);
            if (match.Success)
            {
                return GetIntegerAsFraction(match);
            }
            throw new ArgumentException("Provided string does not contain number.");

        }

        /// <summary>
        /// The GetMixedFractionAsFraction method gets a mixed fraction (ie 1 1/2) as a Fraction
        /// </summary>
        /// <param name="match">Match containing mixed fraction</param>
        /// <returns>Improper fraction as RecipeFraction</returns>
        private static RecipeFraction GetMixedFractionAsFraction(Match match)
        {
            /* get the fraction portion of the mixed fraction as a fraction
               Passes 1 as numeratorIndex because integer portion of mixed fraction will be the number at Captures' index 0 */
            RecipeFraction fraction = GetFractionAsFraction(match, 1);
            //add the integer portion of the fraction (this happens second because integer addition is less costly)
            fraction += (System.Convert.ToInt32(match.Groups["integer"].Captures[0].Value));
            return fraction;
        }

        /// <summary>
        /// The GetFractionAsFraction method returns a RecipeFraction representing the fraction contained in a String
        /// </summary>
        /// <param name="match">Match containing fraction</param>
        /// <param name="numeratorIndex">Index at which Captures contains numerator of fraction if fraction is not represented as Unicode fraction</param>
        /// <returns>RecipeFraction</returns>
        private static RecipeFraction GetFractionAsFraction(Match match, int numeratorIndex = 0)
        {
            GroupCollection groups = match.Groups;
            RecipeFraction fraction;
            CaptureCollection integers = groups["integer"].Captures;
            if (groups["unicode_fraction"].Success)
            {
                fraction = GetFractionFromUnicode(groups["unicode_fraction"].Value);
            }
            else
            {
                int numerator = System.Convert.ToInt32(integers[numeratorIndex].Value);
                int denominator = System.Convert.ToInt32(integers[numeratorIndex + 1].Value);
                fraction = new RecipeFraction(numerator, denominator);
            }
            return fraction;
        }

        /// <summary>
        /// The GetIntegerAsFraction method returns a RecipeFraction representing an integer contained in a Match
        /// </summary>
        /// <param name="match">Match containing integer</param>
        /// <returns>RecipeFraction representing integer</returns>
        private static RecipeFraction GetIntegerAsFraction(Match match)
        {
            int integer = System.Convert.ToInt32(match.Groups["integer"].Value);
            return new RecipeFraction(integer, 1);
        }

        /// <summary>
        /// The GetFractionFromUnicode method gets a RecipeFraction based on a Unicode value
        /// </summary>
        /// <param name="value">String of Unicode</param>
        /// <returns>RecipeFraction</returns>
        private static RecipeFraction GetFractionFromUnicode(string value)
        {

            if (value == '\u00BC'.ToString())
            {
                return new RecipeFraction(1, 4);
            }
            else if (value == '\u00BD'.ToString())
            {
                return new RecipeFraction(1, 2);
            }
            else if (value == '\u00BE'.ToString())
            {
                return new RecipeFraction(3, 4);
            }
            else if (value == '\u2150'.ToString())
            {
                return new RecipeFraction(1, 7);
            }
            else if (value == '\u2151'.ToString())
            {
                return new RecipeFraction(1, 9);
            }
            else if (value == '\u2152'.ToString())
            {
                return new RecipeFraction(1, 10);
            }
            else if (value == '\u2153'.ToString())
            {
                return new RecipeFraction(1, 3);
            }
            else if (value == '\u2154'.ToString())
            {
                return new RecipeFraction(2, 3);
            }
            else if (value == '\u2155'.ToString())
            {
                return new RecipeFraction(1, 5);
            }
            else if (value == '\u2156'.ToString())
            {
                return new RecipeFraction(2, 5);
            }
            else if (value == '\u2157'.ToString())
            {
                return new RecipeFraction(3, 5);
            }
            else if (value == '\u2158'.ToString())
            {
                return new RecipeFraction(4, 5);
            }
            else if (value == '\u2159'.ToString())
            {
                return new RecipeFraction(1, 6);
            }
            else if (value == '\u215A'.ToString())
            {
                return new RecipeFraction(5, 6);
            }
            else if (value == '\u215B'.ToString())
            {
                return new RecipeFraction(1, 8);
            }
            else if (value == '\u215C'.ToString())
            {
                return new RecipeFraction(3, 8);
            }
            else if (value == '\u215D'.ToString())
            {
                return new RecipeFraction(5, 8);
            }
            else if (value == '\u215E'.ToString())
            {
                return new RecipeFraction(7, 8);
            }
            else return null;
        }
    }
}
