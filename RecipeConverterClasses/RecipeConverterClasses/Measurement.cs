using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeConverterClasses
{

    public class Measurement
    {
        private RecipeFraction _amount;
        private Unit _unitSize;

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
        internal RecipeFraction Amount
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

        internal Unit UnitSize
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
        /// The ToString method returns a string representation of the Measurement
        /// </summary>
        /// <returns>String representation of the Measurement</returns>
        public override string ToString()
        {
            if (UnitSize == Unit.OTHER)
            {
                return Amount.ToString();

            }
            else
            {
                string unitString = Amount.CompareTo(1) > 0 ? UnitSize.ToString() + "S" : UnitSize.ToString();
                return Amount.ToString() + " " + unitString;
            }

        }

        public string ToHTMLFormattedString()
        {
            if (UnitSize == Unit.OTHER)
            {
                return Amount.ToHTMLFormattedString();
            }
            else
            {
                string unitString = Amount.CompareTo(1) > 0 ? UnitSize.ToString() + "S" : UnitSize.ToString();
                return Amount.ToHTMLFormattedString() + " " + unitString;
            }

        }


        /// <summary>
        /// The Copy method returns a deep copy of the Measurement
        /// </summary>
        /// <returns>Copy of Measurement object</returns>
        public Measurement Copy()
        {
            return new Measurement(Amount.Copy(), UnitSize);
        }
        //NEW: NEED COMMENTS EDITED OR ADDED

        public bool Equals(Measurement measurement)
        {
            return Amount.Equals(measurement.Amount) && UnitSize.Equals(measurement.UnitSize);
        }
        public void MultiplyBy(Fraction multiplier)
        {
            Amount*=(multiplier);
        }

        public void DivideBy(Fraction divisor)
        {
            Amount/=(divisor);
        }

        public ICollection<Measurement> UserFriendlyMeasurements()
        {

            if (UnitSize == Unit.OTHER)
            {
                ICollection<Measurement> convertedMeasurements = new List<Measurement>();
                convertedMeasurements.Add(Copy());
                return convertedMeasurements;
            }
            else
            {
                RecipeFraction teaspoons = GetFractionAsTeaspoons();
                return GetUserFriendlyMeasurements(teaspoons);
            }

        }

        /// <summary>
        /// The ConvertNewMeasurementToTeaspoons method converts the new measurement to teaspoons
        /// </summary>
        private RecipeFraction GetFractionAsTeaspoons()
        {
            switch (UnitSize)
            {
                case Unit.CUP:
                    return GetCupsFractionAsTeaspoons();

                case Unit.TABLESPOON:
                    return GetTablespoonsFractionAsTeaspoons();

                case Unit.TEASPOON:
                    return Amount.Copy();

                default:
                    throw new Exception();

            }
        }

        /// <summary>
        /// The ConvertCupToTeaspoon method converts the new Measurement from cups to teaspoons
        /// </summary>
        private RecipeFraction GetCupsFractionAsTeaspoons()
        {
            RecipeFraction fraction = Amount.Copy();
            fraction*=(RecipeConstants.TBSP_PER_CUP);
            fraction*=(RecipeConstants.TSP_PER_TBSP);
            return fraction;
        }

        /// <summary>
        /// The ConvertTablespoonToTeaspoon method converts the new Measurement from tablespoons to teaspoons
        /// </summary>
        private RecipeFraction GetTablespoonsFractionAsTeaspoons()
        {
            RecipeFraction fraction = Amount.Copy();
            fraction*=(RecipeConstants.TSP_PER_TBSP);
            return fraction;
        }

        /// <summary>
        /// The GetUserFriendlyMeasurements gets a collection of user friendly measurements
        /// based on the number of teaspoons in a recipe
        /// </summary>
        /// <param name="teaspoons">RecipeFraction containing number of teaspoons in the recipe</param>
        /// <returns>Collection of user friendly measurements</returns>
        private static ICollection<Measurement> GetUserFriendlyMeasurements(RecipeFraction teaspoons)
        {
            //Get simplified measurements, using as many quarter cups as possible 
            ICollection<Measurement> convertedMeasurements = GetMeasurementsFromQuarterCup(teaspoons.Copy());
            int wholeTeaspoons = teaspoons.WholePart();

            /**
             * If could get a third of a cup from converted recipe, get measurements 
             * using third cups and then quarter cups (if applicable).
             * Then compare two Collections of Measurments to see if Collection with third cups is more user-friendly
             */
            if (wholeTeaspoons >= RecipeConstants.TSP_PER_THIRD_CUP)
            {
                ICollection<Measurement> tCupMeasurements = GetMeasurementsFromThirdCup(teaspoons);
                //If same number of Measurements are returned, use Collection that starts with greater Measurement
                if (convertedMeasurements.Count == tCupMeasurements.Count)
                {
                    Fraction qCups = convertedMeasurements.ElementAt(0).Amount;
                    Fraction tCups = tCupMeasurements.ElementAt(0).Amount;
                    if (tCups.CompareTo(qCups) > 0)
                    {
                        convertedMeasurements = tCupMeasurements;
                    }

                }
                //If the Collection with third cups is shorter than collection with quarter cups, use collection with third cups
                else if (tCupMeasurements.Count < convertedMeasurements.Count)
                {
                    convertedMeasurements = tCupMeasurements;
                }
            }
            return convertedMeasurements;
        }

        /// <summary>
        /// The GetMeasurementsFromThirdCup method creates a Collection of the greatest possible Measurements 
        /// the sum of which will equal the given number of teaspoons
        /// Starts with number of third cups and then quarter cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <returns>Collection of Measurements equal to number of teaspoons</returns>
        private static ICollection<Measurement> GetMeasurementsFromThirdCup(RecipeFraction teaspoons)
        {
            ICollection<Measurement> measurements = new List<Measurement>();
            AddThirdCupsToCollection(teaspoons, measurements);
            //once have maximum third cups, use GetMeasurementsFromQuarterCups to get next greatest Measurements
            measurements = measurements.Concat(GetMeasurementsFromQuarterCup(teaspoons)).ToList();


            return measurements;
        }

        /// <summary>
        /// The GetMeasurementsFromQuarterCup method gets creates a Collection of the greatest possible Measurements 
        /// the sum of which will equal the given number of teaspoons
        /// Uses quarter cup as greatest possible measurement
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons from which to get Measurements</param>
        /// <returns>Collection of Measurements equal to the teaspoon Fraction</returns>
        private static ICollection<Measurement> GetMeasurementsFromQuarterCup(RecipeFraction teaspoons)
        {
            ICollection<Measurement> measurements = new List<Measurement>();
            if (teaspoons.EqualsValue(0))
            {
                return measurements;
            }
            AddQuarterCupsToCollection(teaspoons, measurements);
            if (teaspoons.EqualsValue(0))
            {
                return measurements;
            }
            AddHalfTablespoonsToCollection(teaspoons, measurements);
            if (teaspoons.EqualsValue(0))
            {
                return measurements;
            }
            AddTeaspoonsToCollection(teaspoons, measurements);
            return measurements;
        }


        /// <summary>
        /// The AddThirdCupsToCollection method adds as many third cup Measurements as can be formed from given number of teaspoons to a Collection 
        /// The third cups are added as a single Measurement with a Fraction reflecting the number of third cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add third cups Measurement</param>
        private static void AddThirdCupsToCollection(RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {
            int thirdCups = GetThirdCups(teaspoons);
            if (thirdCups > 0)
            {
                RecipeFraction cups = new RecipeFraction(thirdCups, 3);
                measurements.Add(new Measurement(cups, Unit.CUP));
                teaspoons-=(thirdCups * RecipeConstants.TSP_PER_THIRD_CUP);
            }

        }

        /// <summary>
        /// The AddQuarterCupsToCollection method adds as many quarter cup Measurements as can be formed from given number of teaspoons to a Collection 
        /// The quarter cups are added as a single measurement with a fraction reflecting the number of quarter cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add quarter cups Measurement</param>
        private static void AddQuarterCupsToCollection(RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {
            int quarterCups = GetQuarterCups(teaspoons);
            if (quarterCups > 0)
            {
                RecipeFraction cups = new RecipeFraction(quarterCups, 4);
                measurements.Add(new Measurement(cups, Unit.CUP));
                teaspoons-=(quarterCups * RecipeConstants.TSP_PER_QUARTER_CUP);
            }

        }


        /// <summary>
        /// The AddHalfTablespoonsToCollection method adds as many half tablespoon Measurements 
        /// as can be formed from given number of teaspoons to a Collection 
        /// The half tablespoons are added as a single measurement with a fraction reflecting the number of quarter cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add half tablespoons Measurement</param>
        private static void AddHalfTablespoonsToCollection(Fraction teaspoons, ICollection<Measurement> measurements)
        {
            int halfTbsp = GetHalfTablespoons(teaspoons);
            if (halfTbsp > 0)
            {
                RecipeFraction tbsp = new RecipeFraction(halfTbsp, 2);
                measurements.Add(new Measurement(tbsp, Unit.TABLESPOON));
                teaspoons-=(new Fraction((halfTbsp * RecipeConstants.TSP_PER_TBSP), 2));
            }

        }

        /// <summary>
        /// The AddTeaspoonsToCollection adds measurement of teaspoons to a Collection
        /// </summary>
        /// <param name="teaspoons">Number of teaspoons to add to Collection</param>
        /// <param name="measurements">Collection to which to add teaspoons Measurement</param>
        private static void AddTeaspoonsToCollection(RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {

            if (teaspoons.CompareTo(0) > 0)
            {
                measurements.Add(new Measurement(teaspoons, Unit.TEASPOON));
            }

        }

        /// <summary>
        /// The GetHalfTablespoons method gets the greatest number of half tablespoons
        /// that can be formed from given number of teaspoons
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <returns>Greatest number of half tablespoons that can be formed</returns>
        private static int GetHalfTablespoons(Fraction teaspoons)
        {
            //equivelant of teaspoons/number_of_teaspoons_per_half_teaspoon, but need Fraction's div
            //method because they are both fractions
            return Fraction.Div(teaspoons.Copy(), new Fraction(3, 2));
        }

        /// <summary>
        /// The GetQuarterCups method gets the greatest number of quarter cups 
        /// that can be formed from given number of teaspoons
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <returns>Greatest number of quarter cups that can be formed</returns>
        private static int GetQuarterCups(Fraction teaspoons)
        {

            return teaspoons.WholePart() / RecipeConstants.TSP_PER_QUARTER_CUP;
        }

        /// <summary>
        /// The GetThirdCups method gets the greatest number of third cups 
        /// that can be formed from given number of teaspoons
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <returns>Greatest number of third cups that can be formed</returns>
        private static int GetThirdCups(Fraction teaspoons)
        {
            return teaspoons.WholePart() / RecipeConstants.TSP_PER_THIRD_CUP;
        }

    }
}
