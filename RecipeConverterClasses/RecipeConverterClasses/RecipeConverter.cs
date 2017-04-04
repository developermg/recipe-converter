using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// The RecipeConverter class converts a text recipe to new proportions
    /// </summary>
    public class RecipeConverter
    {

        private string _originalRecipe;
        private NonNegativeFraction _multiplier;

        public string OriginalRecipe
        {
            get
            {
                return _originalRecipe;
            }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="recipe">string of recipe text</param>
        public RecipeConverter(string recipe, NonNegativeFraction multiplier)
        {
            _originalRecipe = recipe;
            _multiplier = multiplier;
        }


        /// <summary>
        /// The multiplyBy method multiplies the recipe
        /// </summary>
        /// <param name="numerator">Numerator of fraction by which to multiply the recipe (or new serving size)</param>
        /// <param name="denominator">Denominator of fraction by which to multiply the recipe (or original serving size)</param>
        /// <returns>Environment.NewLine delimited string of converted recipes</returns>
        public string Convert()
        {
            StringBuilder convertedRecipe = new StringBuilder();
            string[] separators = { "\r\n" };
            string[] recipeLines = _originalRecipe.Split(separators, StringSplitOptions.None);
            try
            {
                //convert each line in the recipe and append to StringBuilder
                foreach (string line in recipeLines)
                {
                    if (convertedRecipe.Length != 0)
                    {
                        convertedRecipe.Append(Environment.NewLine);
                    }
                    convertedRecipe.Append(ConvertLine(line));
                    

                }
                return convertedRecipe.ToString();
            }
            //Handles exception thrown by StringBuilder when it reaches maximum capacity.
            catch (ArgumentOutOfRangeException e)
            {
                return convertedRecipe.ToString() + Environment.NewLine + "-------RECIPE TERMINATED-------";
            }
        }

        private string ConvertLine(string line)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {

                //use regex reader to return string of measurement and add to convertedRecipe
                MeasurementConverter reader = new MeasurementConverter(line, _multiplier);
                /* Measurement originalMeasurement = reader.Read();
                 if (originalMeasurement != null)
                 {

                     originalMeasurement.MultiplyBy(_multiplier);
                     Match match = reader.Match;
                     ICollection<Measurement> convertedMeasurement = originalMeasurement.UserFriendlyMeasurements();
                     string replacement = String.Join(" + ", convertedMeasurement.Select(i => i.ToHTMLFormattedString()));
                     line=line.Replace(match.Value, replacement);

                 }*/
                line= reader.ConvertLine();
            }
            return line;
        }
    }
}
