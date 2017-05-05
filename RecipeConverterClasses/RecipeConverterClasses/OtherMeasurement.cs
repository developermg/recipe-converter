using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{
    /// <summary>
    /// The OtherMeasurement class extends the Measurement class for Measurements that do not fit into one of the Unit options
    /// </summary>
    class OtherMeasurement : Measurement
    {
        public OtherMeasurement(RecipeFraction fraction) : base(fraction, Unit.OTHER)
        {
        }

        /// <summary>
        /// The Copy method returns a deep copy of the OtherMeasurement
        /// </summary>
        /// <returns>Copy of Measurement object</returns>
        public override Measurement Copy()
        {
            return new OtherMeasurement(Amount.Copy());
        }

        /// <summary>
        /// The ToHTMLFormattedString method returns a string that includes HTML tags to better display fractions
        /// </summary>
        /// <returns>String containing HTML-formatted representation of OtherMeasurement </returns>
        public override string ToHTMLFormattedString()
        {
            return Amount.ToHTMLFormattedString();
        }


        /// <summary>
        /// The ToString method returns a string representation of the OtherMeasurement
        /// </summary>
        /// <returns>String representation of the OtherMeasurement</returns>
        public override string ToString()
        {
            return Amount.ToString();
        }

        /// <summary>
        /// Because the measurement/ingredient type of an OtherMeasurement is unknown, the UserFriendlyMeaurements method 
        /// simply returns this measurement in an ICollection to conform to Measurement requirements
        /// </summary>
        /// <returns>ICollection containing this measurement</returns>
        public override ICollection<Measurement> UserFriendlyMeasurements()
        {
            /* Amount does not need to be simplified here becuase it can be changed if compared to anything else. 
            It is simplified when displayed (in the ToString and ToHTMLFormattedString) */
            ICollection<Measurement> convertedMeasurements = new List<Measurement>();
            convertedMeasurements.Add(Copy());
            return convertedMeasurements;
        }
    }
}
