using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// The ReadsForType interface reads text to get the specified type
    /// </summary>
    /// <typeparam name="T">Type to find in text</typeparam>
    public interface ReadsForType<T>
    {
        T Read();
    }
}
