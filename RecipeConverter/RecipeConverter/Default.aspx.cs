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

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Submit();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            ResetWholePage();
        }

        private void ResetWholePage()
        {
            Response.Redirect(Request.Url.PathAndQuery, true);
        }
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
                    ResizedVersionLabel.Text = "<i>Servings: " + desiredServings + "</i><br/>" + ConvertRecipe(IngredientsText.Text, desiredServings, originalServings); //.Replace("\r\n", "<br/>");

                }
                catch(Exception ex)
                {
                    DisplayError("Error converting recipe.");
                }
            }
            else
            {
                DisplayError("Invalid serving size entered.");
            }
        }

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