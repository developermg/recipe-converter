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
        private const string INTEGER_PATTERN = @"(\d+)";
        private const string SLASH_FRACTION_PATTERN = @"(?:" + INTEGER_PATTERN + @"/" + INTEGER_PATTERN + @")";
        private const string UNICODE_FRACTION_PATTERN = @"([\u00BC-\u00BE]|[\u2150-\u215E])";
        private const string FRACTION_PATTERN = @"(?:" + UNICODE_FRACTION_PATTERN + @"|" + SLASH_FRACTION_PATTERN + @")";
        private const string NUMBER_PATTERN= "(?:" + FRACTION_PATTERN + @"|(?:" + INTEGER_PATTERN + @"(?:\s*)" + FRACTION_PATTERN + @")|" + INTEGER_PATTERN + ")";

        private static string EndOfUnitPattern = @"(?i)(?:\s+|%)";
        private static string SPattern=@"(?:\(s\)|s?)";
        private static string CupPattern = $@"(?i)(?:(?:cup{SPattern})|c){EndOfUnitPattern}";
        private static string TbspPattern = $@"(?i)(?:(?:tablespoon{SPattern})|(tbsp{SPattern})|(?:(?-i)T)){EndOfUnitPattern}"; 
        private static string TspPattern = $@"(?i)(?:(?:teaspoon{SPattern})|(?:tsp{SPattern})|(?:(?-i)t)){EndOfUnitPattern}"; 
        private static string OtherIngPattern = $@"(?:(?!(?:TABLESPOON|TEASPOON|CUP){SPattern})(?<word>\w))";

        private string PATTERN = $@"{NUMBER_PATTERN}\s(?<unit>{CupPattern}|{TbspPattern}|{TspPattern}|{OtherIngPattern})";
        // private const string OTHER_ING_PATTERN = NUMBER_PATTERN_STRING + @"\w+");
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
            //text=Regex.Replace(text, CUP_PATTERN, m => Replace(m, Unit.CUP));
            //text=Regex.Replace(text, TBSP_PATTERN, m => Replace(m, Unit.TABLESPOON));
            //text=Regex.Replace(text, TSP_PATTERN, m => Replace(m, Unit.TEASPOON));
            //text = Regex.Replace(text, OTHER_ING_PATTERN, m => ReplaceOther(m, Unit.OTHER));
            return "\u2022" + " "+ text;

        }

        private string Replace(Match match)
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
            string replacement = Replace(match, unit)+" ";
            if (unit == Unit.OTHER)
            {
                replacement+= match.Groups["word"];
            }
            return replacement;  
        }
        private string ReplaceOther(Match match, Unit unit)
        {
            return Replace(match, unit) + " " +match.Groups["word"];
        }
        private string Replace(Match match, Unit unit)
        {
            RecipeFraction fraction = GetFractionFromMatch(match);
            fraction.MultiplyBy(multiplier);
            Measurement measurement = new Measurement(fraction, unit);
            ICollection<Measurement> measurements = measurement.UserFriendlyMeasurements();
            string replacement = String.Join(" + ", measurements.Select(i => i.ToHTMLFormattedString()));
            return replacement;
        }
        private static RecipeFraction GetFractionFromMatch(Match match)
        {
            int justIntGroup = 8;
            int justFractionUnicodeGroup = 1;
            int mixedInt = 4;
            int mixedUnicodeFraction = 5;
            int justFractionNum = 2;
            int justFractionDen = 3;
            int mixedFractionNum = 6;
            int mixedFractionDen = 7;
            RecipeFraction fraction;
            //check if just an integer
            if (match.Groups[justIntGroup].Value != "")
            {
                fraction = new RecipeFraction(Convert.ToInt32(match.Groups[justIntGroup].Value), 1);
            }
            //otherwise if it is just a unicode fraction
            else if (match.Groups[justFractionUnicodeGroup].Value != "")
            {
                fraction = GetFractionFromUnicode(match.Groups[justFractionUnicodeGroup].Value);
            }
            //otherwise if just manual fraction
            else if (match.Groups[justFractionNum].Value != "")
            {
                fraction = new RecipeFraction(Convert.ToInt32(match.Groups[justFractionNum].Value), Convert.ToInt32(match.Groups[justFractionDen].Value));
            }
            //otherwise it is a mixed group
            else
            {
                if (match.Groups[mixedUnicodeFraction].Value != "")
                {
                    fraction = GetFractionFromUnicode(match.Groups[mixedUnicodeFraction].Value);
                }
                else
                {
                    fraction = new RecipeFraction(Convert.ToInt32(match.Groups[mixedFractionNum].Value), Convert.ToInt32(match.Groups[mixedFractionDen].Value));
                }

                fraction.Add(Convert.ToInt32(match.Groups[mixedInt].Value));
            }
            return fraction;
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
