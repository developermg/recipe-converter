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
        private const string INTEGER_PATTERN = @"(?<integer>\d+)";
        private const string SLASH_FRACTION_PATTERN = @"(?<slash_fraction>" + INTEGER_PATTERN + @"/" + INTEGER_PATTERN + @")";
        private const string UNICODE_FRACTION_PATTERN = @"(?<unicode_fraction>[\u00BC-\u00BE]|[\u2150-\u215E])";
        private const string FRACTION_PATTERN = @"(?<fraction>" + UNICODE_FRACTION_PATTERN + @"|" + SLASH_FRACTION_PATTERN + @")";
        private const string MIXED_FRACTION_PATTERN = "(?<mixed_fraction>" + INTEGER_PATTERN + @"(?:\s*)" + FRACTION_PATTERN + ")";
        private const string RANGE_PATTERN = "(?<range>" + NUMBER_PATTERN + @"(\s*)-(\s*)" + NUMBER_PATTERN + ")";
        private const string NUMBER_PATTERN = "(?<number>" + MIXED_FRACTION_PATTERN + @"|" + FRACTION_PATTERN + @"|" + INTEGER_PATTERN + ")";

        private static string EndOfUnitPattern =@"(?i)(?=\s+|$|\))";
        private static string SPattern = @"(?:\(s\)|s?)";
        private static string CupPattern = $@"(?i)(?:(?:cup{SPattern})|c){EndOfUnitPattern}";
        private static string TbspPattern = $@"(?i)(?:(?:tablespoon{SPattern})|(?:tbsp{SPattern})|(?:(?-i)T)){EndOfUnitPattern}";
        private static string TspPattern = $@"(?i)(?:(?:teaspoon{SPattern})|(?:tsp{SPattern})|(?:(?-i)t)){EndOfUnitPattern}";
        private static string OtherIngPattern = @"(?=\s\w)";

        private string PATTERN = $@"(?:{RANGE_PATTERN}|{NUMBER_PATTERN})(?<unit>\s{CupPattern}|\s{TbspPattern}|\s{TspPattern}|{OtherIngPattern})";
        private string text;
        private NonNegativeFraction multiplier;


        public MeasurementConverter(string text, NonNegativeFraction multiplier)
        {
            this.text = text;
            this.multiplier = multiplier;
        }


        /// <summary>
        /// The Read method reads text for a Measurement 
        /// </summary>
        /// <returns>Measurement found in text or null if not found</returns>
        public string ConvertLine()
        {
            text = Regex.Replace(text, PATTERN, m => Replace(m));
            return "\u2022" + " " + text;

        }

        private string Replace(Match match)
        {
            Unit unit = GetUnitFromMatch(match);
            if (Regex.Match(match.Value, RANGE_PATTERN).Success)
            {
                String replacement = "[" + Replace(match.Groups["number"].Captures[0].Value, unit) + "]";
                replacement += " <b>-</b> [" + Replace(match.Groups["number"].Captures[1].Value, unit) + "]";
                return replacement;
            }
            else {
                return Replace(match.Value, unit);
            }
          
        }

        private static Unit GetUnitFromMatch(Match match)
        {
            string unitText = match.Groups["unit"].Value;

            Unit unit;
            Match unitMatch = Regex.Match(unitText, CupPattern);
            if (unitMatch.Success)
            {
                unit = Unit.CUP;
            }
            else
            {
                unitMatch = Regex.Match(unitText, TbspPattern);
                if (unitMatch.Success)
                {
                    unit = Unit.TABLESPOON;
                }
                else
                {
                    unitMatch = Regex.Match(unitText, TspPattern);
                    if (unitMatch.Success)
                    {
                        unit = Unit.TEASPOON;
                    }
                    else
                    {
                        unit = Unit.OTHER;
                    }
                }
            }
            return unit;
        }
        private string Replace(String matchStr, Unit unit)
        {
            
           
                RecipeFraction fraction = GetFraction(matchStr);
                fraction*=(multiplier);
                Measurement measurement = new Measurement(fraction, unit);
                ICollection<Measurement> measurements = measurement.UserFriendlyMeasurements();
                string replacement = String.Join(" + ", measurements.Select(i => i.ToHTMLFormattedString()));
                return replacement;
            
           
        }


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
        private static RecipeFraction GetFractionFromMatch(Match match)
        {
            if (match.Groups["mixed_fraction"].Success)
            {
                return GetMixedFractionAsFraction(match);
            }
            else if (match.Groups["fraction"].Success)
            {
                return GetFractionAsFraction(match);
            }
            else
            {
                return GetIntegerAsFraction(match);
            }


        }

        private static RecipeFraction GetMixedFractionAsFraction(Match match)
        {
            RecipeFraction fraction = GetFractionAsFraction(match, 1);

            fraction+=(Convert.ToInt32(match.Groups["integer"].Captures[0].Value));
            return fraction;
        }

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
                fraction = new RecipeFraction(Convert.ToInt32(integers[numeratorIndex].Value), Convert.ToInt32(integers[numeratorIndex + 1].Value));
            }
            return fraction;
        }

        private static RecipeFraction GetIntegerAsFraction(Match match)
        {
            return new RecipeFraction(Convert.ToInt32(match.Groups["integer"].Value), 1);
        }

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
