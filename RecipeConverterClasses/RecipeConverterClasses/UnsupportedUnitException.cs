using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
[Serializable] public class UnsupportedUnitException : ApplicationException
    {
        public UnsupportedUnitException() { }
        public UnsupportedUnitException(string message) : base(message) { }
        public UnsupportedUnitException(string message, Exception inner) : base(message, inner) { }

        protected UnsupportedUnitException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
}
