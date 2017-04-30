using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{

    public abstract class Measurement
    {
        private RecipeFraction _amount; //numeric part of measurement
        private Unit _unitSize;         //unit of measurement

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numerator">Numerator for fraction</param>
        /// <param name="denominator">Denominator for fraction</param>
        /// <param name="unit">Unit of measure</param>
        public Measurement(RecipeFraction fraction, Unit unit)
        {
            _amount = fraction;
            this._unitSize = unit;
        }
        //Getters and setters:
        protected internal RecipeFraction Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }
        protected internal Unit UnitSize
        {
            get
            {
                return _unitSize;
            }
            set
            {
                _unitSize = value;
            }
        }

        /// <summary>
        /// The UserFriendlyMeasurements method returns a collection of measurements 
        /// that is the user-friendly equivelant of the current measurement
        /// </summary>
        /// <returns>Equivelant of measurement in user-friendly measurements</returns>
        public abstract ICollection<Measurement> UserFriendlyMeasurements();


        /// <summary>
        /// The ToString method returns a string representation of the Measurement
        /// </summary>
        /// <returns>String representation of the Measurement</returns>
        public abstract override string ToString();


        /// <summary>
        /// The ToHTMLFormattedString method returns a string that includes HTML tags to better display fractions
        /// </summary>
        /// <returns>String containing HTML-formatted representation of Measurement </returns>
        public abstract string ToHTMLFormattedString();


        /// <summary>
        /// The Copy method returns a deep copy of the Measurement
        /// </summary>
        /// <returns>Copy of Measurement object</returns>
        public abstract Measurement Copy();



      
    }
}
