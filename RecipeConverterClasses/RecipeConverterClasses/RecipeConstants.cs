using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    public class RecipeConstants
    {
        public const int TBSP_PER_CUP= 16;
        public const int TSP_PER_TBSP = 3;
        public const int TSP_PER_CUP = TSP_PER_TBSP * TBSP_PER_CUP;
        public const int TSP_PER_QUARTER_CUP = TSP_PER_CUP / 4;
        public const int TSP_PER_THIRD_CUP = TSP_PER_CUP / 3;
        

    }
}
