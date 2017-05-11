<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="RecipeConverterApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> the Recipe Converter</h2>
    <a href="#background">Background</a>
    &#8226;
    <a href="#usage">Usage</a>
    &#8226;
    <a href="#scope">Scope</a>
    &#8226;
    <a href="#compatibility">Compatiblity</a>
    &#8226;
    <a href="#results">Results</a>
    <br />
    <h4 class="text-primary"><span id="background" class="anchor">Background</span></h4>
    <p>Online recipe hunting is extremely popular, creating a market for online cooking tools. One such potentially useful tool is a recipe converter that allows a user to multiply or divide a recipe. The current online market has two categories of recipe converters. One variety allows the user to paste in blocks of text, but unbeknownst to the unsuspecting user, only one measurement may be entered per line, and the number portion of the measurement must be the first non-space character on the line. A line that reads “About 1 cup milk” will be ignored. Additionally, it only performs simple multiplication or division on the measurement. Dividing a recipe that requires “1 cup sugar” by twelve yields either rounds up to “1/8 cup sugar,” which is inaccurate, or returns the decimal version, “.08 cup sugar,” which is not a practical measurement. The second variety of recipe converter will return “1 tablespoon + 1 teaspoon,” a user-friendly measurement, when dividing “1 cup” by twelve, but it requires each ingredient entered in a separate textbox, with dropdowns for the measurement size. For this honors thesis project, the aim was to combine the strengths of each of these recipe converters. The recipe converter allows users to enter the recipe as one block of text, allowing them to paste in a recipe, and returns kitchen-friendly measurements. </p>

    <h4 class="text-primary"><span id="usage" class="anchor">Usage</span></h4>
    <p>
        The tool has four fields for user input:
•	Recipe Title: Optional entry. If user enters a recipe title, the title will be displayed above the converted version of the recipe.
•	Original Servings: Required field. User must enter a whole number between 1 and 1000. 
•	Desired Servings: Required field. User must enter a whole number between 1 and 1000. Desired serving size value will be displayed above the converted version of the recipe.
•	Ingredients: Required field. Ingredients need not be on separate lines, as the tool can recognize multiple measurements per line. However, if multiple ingredients are written on the same line they will be returned on the same line.
The clear button clears the four input fields and the returned recipe. The submit button generates the converted recipe. 
    </p>

    <h4 class="text-primary"><span id="scope" class="anchor">Scope</span></h4>
    <p>
        The application can recognize variations of the same measurements. For the numeric portion of the measurement, integers, decimals, fractions, either divided by a slash or as single character such as ½, and mixed fractions, combinations of an integer and fraction, are accepted. Although a decimal number may be provided, when converted it will be returned as a fraction. 
        <br />
        <br />
        Ranges, such as “1½-2,” are handled as well. Each part of the range will be converted separately and recombined with a bolded hyphen. To avoid confusion caused by returning multiple measurements per endpoint of the range, such as “1 tablespoon + 1 teaspoon – 3 tablespoons + ½ teaspoon,” range endpoints are separated by brackets, as in “[1 tablespoon + 1 teaspoon] - [3 tablespoons + ½ teaspoon]. Brackets are also used when the original measurement included a “+,” such as “1+ tsp.” If the user-friendly version of the measurement yields multiple measurements, such as “1 tablespoon + ½ teaspoon,” they will be surrounded by brackets, followed by a bolded plus sign. For this example, “[1 tablespoon + ½ teaspoon]+” would be displayed. If the converted measurement only consists of one measurement, the plus sign will not be bolded and will follow the measurement name, with no brackets, for example, “1 tablespoon+.”
        <br />
        <br />
        The application will recognize a unit of measurement without a space preceding it. Therefore, “½tsp” would be an acceptable measurement. However, a symbol, aside from the standard keyboard plus symbol, following a number causes the number not to be recognized as a measurement amount. For example, “5-ounce” will not be recognized as a measurement. This prevents issues such as converting “1 5-ounce bag” to “½  2½-ounce bag,” which quarters the measurement when halving is desired. However, this problem would occur if the original recipe called for “1 5 ounce bag,” using an informal form without the hyphen, or “1-ounce baking chocolate,” in which “1” is the measurement amount. 
        <br />
        <br />
        Cups, tablespoons and teaspoons may be specified as units of measurement in shortened or regular forms. Any other unit of measurement, including an ingredient name as a measurement, such as “eggs,” will yield a converted number, but the unit of measurement will not be adjusted. Included in this category are measurements in which a word, such as “heaping” or “scant,” precedes a recognized unit name. 
        <br />
        <br />
        An example of handling an unsupported measurement is “8 ounces” when doubled, which will yield “16 ounces,” rather than “1 pound” or “1 dry cup.” Similarly, “3 eggs” when divided by eight will yield “3⁄8 eggs.” Although this is inconvenient for any user who is not using an egg substitute, the application returns it in that form because there is no way to both return an accurate and familiar measurement to the user. The application maintains accuracy; therefore, the user can expect measurements such as “1⁄32 teaspoon” when dividing a recipe. As teaspoons are the smallest volume-based unit of measurement, there is no other accurate way to present the measurement. Rounding either up or down could affect the recipe and is therefore not performed. 
        <br />
        <br />
        The assumption that words following numbers are ingredient names or unsupported measurements can lead to unexpected results. For example, if an ingredient list contains a line that reads, “½ cup slivered almonds, toasted in the oven for about 5 minutes,” the “5,” in addition to the “½,” will be multiplied or divided. 
        <br />
        <br />
        If no unit size is entered following the number, the number will not be converted. For example, if a recipe calls for “onions, 1-2,” no conversion occurs to the measurement. 
        <br />
        <br />
        The scope of the recognized text is such that the calculations were performed properly on sample recipes from various blogs and recipe sites, including “Pinch of Yum,” “Overtime Cook,” “Joy of Kosher” and Allrecipes.com. 
    </p>

    <h4 class="text-primary"><span id="compatibility" class="anchor">Compatibility</span></h4>
    <p>The recipe converter has been tested on and found compatible with recent versions of standard browsers: Microsoft Edge version 38, Internet Explorer 11, Chrome version 58, Mozilla Firefox version 53, and Safari version 10.</p>

    <h4 class="text-primary"><span id="results" class="anchor">Results</span></h4>
    <p>The recipe title, if entered, and the servings size are displayed above the converted recipe. Because an ingredient line, or even multiple units of measurement for the same ingredient, can span multiple lines, a bullet point is displayed at the beginning of each new line.</p>

</asp:Content>
