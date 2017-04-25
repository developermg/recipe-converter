using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// Constants available to all RecipeConverter classes
    /// </summary>
    public class RecipeConstants
    {
        public const int TBSP_PER_CUP = 16; //Number of tablespoons per cup
        public const int TSP_PER_TBSP = 3; //Number of teaspoons per tablespoon
        public const int TSP_PER_CUP = TSP_PER_TBSP * TBSP_PER_CUP; //Number of teaspoons per cup. This value is a constant to provide easier access to this value
        public const int TSP_PER_THIRD_CUP = TSP_PER_CUP / 3; //Number of teaspoons per third of a cup. This value is a constant to provide easier access to this value
        public const int TSP_PER_QUARTER_CUP = TSP_PER_CUP / 4; //Number of teaspoons per quarter cup. This value is a constant to provide easier access to this value


    }
}
