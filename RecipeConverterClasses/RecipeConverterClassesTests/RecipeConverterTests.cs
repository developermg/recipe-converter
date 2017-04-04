using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeConverterClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses.Tests
{
    [TestClass()]
    public class RecipeConverterTests
    {
        
        [TestMethod()]
        public void RecipeConverter_Test()
        {
            //arrange
            string recipe = "2 cups sugar\n1 cup milk";
            NonNegativeFraction multiplier = new NonNegativeFraction(2, 1);
            //act
            RecipeConverter converter = new RecipeConverter(recipe, multiplier);
            //assert
            Assert.AreEqual(recipe, converter.OriginalRecipe);
        }
        [TestMethod()]
        public void Convert_Test()
        {
            //arrange
            string recipe = "2 cups sugar\r\n1 cup milk\r\n1 T chocolate syrup\r\n4 t coffee";
            NonNegativeFraction multiplier = new NonNegativeFraction(2, 1);
            RecipeConverter converter = new RecipeConverter(recipe, multiplier);
            string expected = "4 CUPS sugar"
                +Environment.NewLine+"2 CUPS milk" 
                + Environment.NewLine + "2 TABLESPOONS chocolate syrup"
                + Environment.NewLine + "2 1/2 TABLESPOONS + 1/2 TEASPOON coffee";
            string actual = converter.Convert();
            Assert.AreEqual(expected, actual);
        }


    }
}