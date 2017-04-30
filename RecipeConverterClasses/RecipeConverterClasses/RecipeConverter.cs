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

        //getter (user modification of original recipe not allowd).
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
        /// <param name="multiplier">NonNegativeFraction by which to multiply recipe</param>
        public RecipeConverter(string recipe, NonNegativeFraction multiplier)
        {
            _originalRecipe = recipe;
            _multiplier = multiplier;
        }

        /// <summary>
        /// The Convert method converts the recipe
        /// </summary>
        /// <returns>Environment.NewLine delimited string of converted recipes</returns>
        public string Convert()
        {
            string[] separators = { "\r\n" };
            StringBuilder convertedRecipe = new StringBuilder();
            //split recipe into individual lines
            string[] recipeLines = _originalRecipe.Split(separators, StringSplitOptions.None);
            try
            {
                //convert each line in the recipe and append to StringBuilder
                foreach (string line in recipeLines)
                {
                    if (convertedRecipe.Length != 0)
                    {
                        convertedRecipe.Append("<br/>");
                    }
                    convertedRecipe.Append(ConvertLine(line));
                }
                return convertedRecipe.ToString();
            }
            //Handles exception thrown by StringBuilder when it reaches maximum capacity
            catch (ArgumentOutOfRangeException e)
            {
                return convertedRecipe.ToString() + "<br/>" + "-------RECIPE TERMINATED-------";
            }
        }

        /// <summary>
        /// The Convert method returns a line of text with all contained measurements
        /// multiplied by multiplier and in user-friendly measurements
        /// </summary>
        /// <param name="line">Line of text to convert</param>
        /// <returns>Converted line of text</returns>
        private string ConvertLine(string line)
        {
            //if blank line, blank line returned
            if (!String.IsNullOrWhiteSpace(line))
            {
                MeasurementConverter converter = new MeasurementConverter(line, _multiplier);
                line = converter.Convert();
                /* Puts a bullet point at the beginning of the line to indicate the start of a new line
                 * because a single ingredient's measurements could wrap onto more than one line */
                line = "\u2022" + " " + line;
            }
            return line;
        }
    }
}
