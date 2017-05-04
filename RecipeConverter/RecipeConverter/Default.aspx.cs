using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RecipeConverterClasses;

namespace RecipeConverterApp
{
    public partial class _Default : Page
    {
        /// <summary>
        /// The SubmitButton_Click method triggers submission
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Submit();
        }

        /// <summary>
        /// The ResetButton_Click triggers reloading the page
        /// Because reseting from the server will send page back, 
        /// makes sense to reload the page to clear rather than clear all the fields and results boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ResetButton_Click(object sender, EventArgs e)
        {
            ReloadPage();
        }

        /// <summary>
        /// The ReloadPage method reloads the page
        /// </summary>
        private void ReloadPage()
        {
            Response.Redirect(Request.Url.PathAndQuery, true);
        }

        /// <summary>
        /// The Submit method submits the recipe  and displays the results if the serving sizes are valid
        /// </summary>
        protected void Submit()
        {
            int desiredServings, originalServings;
            if (TitleText.Text != "")
            {
                RecipeTitleLabel.Text = TitleText.Text;
            }
            if (Int32.TryParse(DesiredServingsText.Text, out desiredServings) && Int32.TryParse(OriginalServingsText.Text, out originalServings))
            {
                try
                {
                    ResizedVersionLabel.Text = "<i>Servings: " + desiredServings + "</i><br/>" + ConvertRecipe(IngredientsText.Text, desiredServings, originalServings);

                }
                //if any Exception is thrown when converteing the recipe, catch it and display an error message.
                catch(Exception ex)
                {
                    //display the exception message in small so user can submit the error
                    DisplayError("Error converting recipe.<small>Exception: " + ex.Message + "</small>");
                }
            }
            else
            {
                DisplayError("Invalid serving size entered.");
            }
        }

        /// <summary>
        /// The ConvertRecipe method converts a recipe using the RecipeConverter
        /// </summary>
        /// <param name="ingredients">Text of ingredients</param>
        /// <param name="desiredServings">Desired number of servings</param>
        /// <param name="originalServings">Original number of servings</param>
        /// <returns>New text</returns>
        private string ConvertRecipe(string ingredients, int desiredServings, int originalServings)
        {
            RecipeConverter converter = new RecipeConverter(ingredients, new NonNegativeFraction(desiredServings, originalServings));
            return converter.Convert();
        }
        protected void DisplayError(string message)
        {
            ResizedVersionLabel.Text = message;
        }

    }
}