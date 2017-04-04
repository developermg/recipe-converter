//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace RecipeConverterClasses
//{
//    public class MeasurementConverter : ReadsForType<Measurement>
//    {
//        //private const string CUP_PATTERN = @"\p{Nd} (?i)(cup((\(s\))|s?)) \w";
//        private const string INTEGER_PATTERN = @"(\d+)";
//        private const string SLASH_FRACTION_PATTERN = @"(?:"+INTEGER_PATTERN + @"/" + INTEGER_PATTERN +@")";
//        private const string UNICODE_FRACTION_PATTERN = @"([\u00BC-\u00BE]|[\u2150-\u215E])";
//        private const string FRACTION_PATTERN = @"(?:"+UNICODE_FRACTION_PATTERN + @"|" + SLASH_FRACTION_PATTERN+@")";
//        private const string NUMBER_PATTERN_STRING = "(?:"+FRACTION_PATTERN + @"|(?:" + INTEGER_PATTERN + @"(?:\s*)" + FRACTION_PATTERN + @")|" +INTEGER_PATTERN + ")";


//        private const string CUP_PATTERN = NUMBER_PATTERN_STRING + @" (?i)(cup(\(s\)|s?)|c)";// \S+";
//        private const string TBSP_PATTERN = NUMBER_PATTERN_STRING + @" (?i)(tablespoon((\(s\))|s?)|tbsp((\(s\))|s?)|(?-i)T)"; //word after
//        private const string TSP_PATTERN = NUMBER_PATTERN_STRING + @" (?i)(teaspoon(?:(?:\(?:s\))|s?)|tsp|(?-i)t)"; //word after
//        private const string OTHER_ING_PATTERN = NUMBER_PATTERN_STRING + @" \w";
//        private string text;
//        private Match _match;

//        public Match Match
//        {
//            get
//            {
//                return _match;
//            }
//        }

//        public MeasurementConverter(string text)
//        {
//            this.text = text;
//        }

//        /// <summary>
//        /// The Read method reads text for a Measurement 
//        /// </summary>
//        /// <returns>Measurement found in text or null if not found</returns>
//        public Measurement Read()
//        {
//            //each if has a return statement to avoid nesting the ifs
//            /*each if needs a different fraction, even though they are all the same now,
//            because only dealing with whole numbers here, but intend to add fractions*/
//            _match = Regex.Match(text, CUP_PATTERN);
//            if (_match.Success)
//            {
//                RecipeFraction fraction = GetFractionFromMatch(_match);
//                return new Measurement(fraction, Unit.CUP);
//            }
//            _match = Regex.Match(text, TBSP_PATTERN);
//            if (_match.Success)
//            {
//                RecipeFraction fraction=GetFractionFromMatch(_match);
                
//                return new Measurement(fraction, Unit.TABLESPOON);
//            }
//            _match = Regex.Match(text, TSP_PATTERN);
//            if (_match.Success)
//            {
//                RecipeFraction fraction = GetFractionFromMatch(_match);
//                return new Measurement(fraction, Unit.TEASPOON);
//            }
//            _match = Regex.Match(text, OTHER_ING_PATTERN);
//            if (_match.Success)
//            {
//                RecipeFraction fraction = GetFractionFromMatch(_match);
//                return new Measurement(fraction, Unit.OTHER);
//            }
//            else
//            {
//                _match = null;
//                return null;
//            }

            
//        }

//        private static RecipeFraction GetFractionFromMatch(Match match)
//        {
//            int justIntGroup = 8;
//            int justFractionUnicodeGroup = 1;
//            int mixedInt = 4;
//            int mixedUnicodeFraction =5;
//            int justFractionNum = 2;
//            int justFractionDen = 3;
//            int mixedFractionNum = 6;
//            int mixedFractionDen = 7;
//            RecipeFraction fraction;
//            //check if just an integer
//            if (match.Groups[justIntGroup].Value != "")
//            {
//                fraction = new RecipeFraction(Convert.ToInt32(match.Groups[justIntGroup].Value), 1);
//            }
//            //otherwise if it is just a unicode fraction
//            else if (match.Groups[justFractionUnicodeGroup].Value != "")
//            {
//                fraction = GetFractionFromUnicode(match.Groups[justFractionUnicodeGroup].Value);
//            }
//            //otherwise if just manual fraction
//            else if (match.Groups[justFractionNum].Value != "")
//            {
//                fraction = new RecipeFraction(Convert.ToInt32(match.Groups[justFractionNum].Value), Convert.ToInt32(match.Groups[justFractionDen].Value));
//            }
//            //otherwise it is a mixed group
//            else
//            {
//                if (match.Groups[mixedUnicodeFraction].Value != "")
//                {
//                    fraction = GetFractionFromUnicode(match.Groups[mixedUnicodeFraction].Value);
//                }
//                else
//                {
//                    fraction = new RecipeFraction(Convert.ToInt32(match.Groups[mixedFractionNum].Value), Convert.ToInt32(match.Groups[mixedFractionDen].Value));
//                }
                
//                fraction.Add(Convert.ToInt32(match.Groups[mixedInt].Value));
//            }
//            return fraction;
//        }

//        private static RecipeFraction GetFractionFromUnicode(string value)
//        {

//            if (value == '\u00BC'.ToString())
//            {
//                return new RecipeFraction(1, 4);
//            }
//            else if (value == '\u00BD'.ToString())
//            {
//                return new RecipeFraction(1, 2);
//            }
//            else if (value == '\u00BE'.ToString())
//            {
//                return new RecipeFraction(3, 4);
//            }
//            else if (value == '\u2150'.ToString())
//            {
//                return new RecipeFraction(1, 7);
//            }
//            else if (value == '\u2151'.ToString())
//            {
//                return new RecipeFraction(1, 9);
//            }
//            else if (value == '\u2152'.ToString())
//            {
//                return new RecipeFraction(1, 10);
//            }
//            else if (value == '\u2153'.ToString())
//            {
//                return new RecipeFraction(1, 3);
//            }
//            else if (value == '\u2154'.ToString())
//            {
//                return new RecipeFraction(2, 3);
//            }
//            else if (value == '\u2155'.ToString())
//            {
//                return new RecipeFraction(1, 5);
//            }
//            else if (value == '\u2156'.ToString())
//            {
//                return new RecipeFraction(2, 5);
//            }
//            else if (value == '\u2157'.ToString())
//            {
//                return new RecipeFraction(3, 5);
//            }
//            else if (value == '\u2158'.ToString())
//            {
//                return new RecipeFraction(4, 5);
//            }
//            else if (value == '\u2159'.ToString())
//            {
//                return new RecipeFraction(1, 6);
//            }
//            else if (value == '\u215A'.ToString())
//            {
//                return new RecipeFraction(5, 6);
//            }
//            else if (value == '\u215B'.ToString())
//            {
//                return new RecipeFraction(1, 8);
//            }
//            else if (value == '\u215C'.ToString())
//            {
//                return new RecipeFraction(3, 8);
//            }
//            else if (value == '\u215D'.ToString())
//            {
//                return new RecipeFraction(5, 8);
//            }
//            else if (value == '\u215E'.ToString())
//            {
//                return new RecipeFraction(7, 8);
//            }
//            else return null;
//        }
//    }
//}
