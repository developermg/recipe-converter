﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeConverterClasses
{
    /// <summary>
    /// The USVolumeMeasurement class holds measurements for which the unit is US volume unit-size 
    /// Cups, tablespoons and teaspoons supported
    /// </summary>
    public class USVolumeMeasurement : Measurement
    {
        public USVolumeMeasurement(RecipeFraction fraction, Unit unit) : base(fraction, unit)
        {
            //must be a US volume unit
            if (!(unit == Unit.CUP || unit == Unit.TABLESPOON || unit == Unit.TEASPOON))
            {
                throw new UnsupportedUnitException("Non-volume unit provided for USVolumeMeasurement");
            }
        }

        /// <summary>
        /// The UserFriendlyMeasurements method returns a collection of measurements 
        /// that is the user-friendly equivelant of the current USVolumeMeasurement
        /// </summary>
        /// <returns></returns>
        public override ICollection<Measurement> UserFriendlyMeasurements()
        {
                RecipeFraction teaspoons = GetAsTeaspoons();
                return TeaspoonsToUserFriendlyMeasurements(teaspoons);
        }

        /// <summary>
        /// The GetAsTeaspoons method gets the equivelant of the USVolumeMeasurement in teaspoons
        /// </summary>
        /// <returns>RecipeFraction representing number of teaspoons that is the equivelant of this USVolumeMeasurement</returns>
        private RecipeFraction GetAsTeaspoons()
        {
            switch (UnitSize)
            {
                case Unit.CUP:
                    return GetCupsAsTeaspoons(Amount);

                case Unit.TABLESPOON:
                    return GetTablespoonsAsTeaspoons(Amount);

                case Unit.TEASPOON:
                    return Amount.Copy();

                default:
                    throw new UnsupportedUnitException("Only CUP, TABLESPOON and TEASPOON can be converted to teaspoons.");

            }
        }

        /// <summary>
        /// The GetCupsAsTeaspoons method returns the equivelant of 
        /// a RecipeFraction of cups as a RecipeFraction of teaspoons
        /// </summary>
        /// <param name="fraction">RecipeFraction representing number of cups</param>
        /// <returns>RecipeFraction representing number of teaspoons</returns>
        private static RecipeFraction GetCupsAsTeaspoons(RecipeFraction fraction)
        {
            /*don't have to copy RecipeFraction because *= returns new RecipeFraction 
            and will therefore not be able to change fraction (it was not passed by value using ref/out)*/
            fraction *= (RecipeConstants.TBSP_PER_CUP);

            return GetTablespoonsAsTeaspoons(fraction);
        }

        /// <summary>
        /// The GetTablespoonsAsTeaspoons method the equivelant of 
        /// a RecipeFraction of tablespoons as a RecipeFraction of teaspoons 
        /// </summary>
        /// <param name="fraction">RecipeFraction representing number of tablespoons</param>
        /// <returns>RecipeFraction representing number of teaspoons</returns>
        private static RecipeFraction GetTablespoonsAsTeaspoons(RecipeFraction fraction)
        {
            return fraction * (RecipeConstants.TSP_PER_TBSP);
        }

        /// <summary>
        /// The TeaspoonsToUserFriendlyMeasurements gets a collection of user friendly measurements
        /// from RecipeFraction of teaspoons 
        /// </summary>
        /// <param name="teaspoons">RecipeFraction containing number of teaspoons in the recipe</param>
        /// <returns>Collection of user friendly measurements</returns>
        private static ICollection<Measurement> TeaspoonsToUserFriendlyMeasurements(RecipeFraction teaspoons)
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
                    /*can cast as USVolumeMeasurement because was created in this class. 
                    List is of type Measurement to be usable across multiple types of measurements.
                    Casting required to access Amount with its current access modifier and making public is undesirable*/
                    Fraction qCups = ((USVolumeMeasurement)convertedMeasurements.ElementAt(0)).Amount;
                    Fraction tCups = ((USVolumeMeasurement)tCupMeasurements.ElementAt(0)).Amount;
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
            OptimizeMeasurements(convertedMeasurements);
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
            AddThirdCupsToCollection(ref teaspoons, measurements);
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
            AddQuarterCupsToCollection(ref teaspoons, measurements);
            if (teaspoons.EqualsValue(0))
            {
                return measurements;
            }
            AddHalfTablespoonsToCollection(ref teaspoons, measurements);
            if (teaspoons.EqualsValue(0))
            {
                return measurements;
            }
            AddTeaspoonsToCollection(ref teaspoons, measurements);
            return measurements;
        }

        /// <summary>
        /// The AddThirdCupsToCollection method adds as many third cup USVolumeMeasurements as can be formed from given number of teaspoons to a Collection 
        /// The third cups are added as a single Measurement with a Fraction reflecting the number of third cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add third cups Measurement</param>
        private static void AddThirdCupsToCollection(ref RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {
            int thirdCups = GetThirdCups(teaspoons);
            if (thirdCups > 0)
            {
                RecipeFraction cups = new RecipeFraction(thirdCups, 3);
                measurements.Add(new USVolumeMeasurement(cups, Unit.CUP));
                teaspoons -= (thirdCups * RecipeConstants.TSP_PER_THIRD_CUP);
            }

        }

        /// <summary>
        /// The AddQuarterCupsToCollection method adds as many quarter cup USVolumeMeasurements as can be formed from given number of teaspoons to a Collection 
        /// The quarter cups are added as a single measurement with a fraction reflecting the number of quarter cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add quarter cups Measurement</param>
        private static void AddQuarterCupsToCollection(ref RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {
            int quarterCups = GetQuarterCups(teaspoons);
            if (quarterCups > 0)
            {
                RecipeFraction cups = new RecipeFraction(quarterCups, 4);
                measurements.Add(new USVolumeMeasurement(cups, Unit.CUP));
                teaspoons -= (quarterCups * RecipeConstants.TSP_PER_QUARTER_CUP);
            }

        }

        /// <summary>
        /// The AddHalfTablespoonsToCollection method adds as many half tablespoon USVolumeMeasurements 
        /// as can be formed from given number of teaspoons to a Collection 
        /// The half tablespoons are added as a single measurement with a fraction reflecting the number of quarter cups
        /// </summary>
        /// <param name="teaspoons">Fraction representing number of teaspoons</param>
        /// <param name="measurements">Measurements Collection to which to add half tablespoons USVolumeMeasurement</param>
        private static void AddHalfTablespoonsToCollection(ref RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {
            int halfTbsp = GetHalfTablespoons(teaspoons);
            if (halfTbsp > 0)
            {
                RecipeFraction tbsp = new RecipeFraction(halfTbsp, 2);
                measurements.Add(new USVolumeMeasurement(tbsp, Unit.TABLESPOON));
                teaspoons -= (new Fraction((halfTbsp * RecipeConstants.TSP_PER_TBSP), 2));
            }

        }

        /// <summary>
        /// The AddTeaspoonsToCollection adds measurement of teaspoons to a Collection
        /// </summary>
        /// <param name="teaspoons">Number of teaspoons to add to Collection</param>
        /// <param name="measurements">Collection to which to add teaspoons Measurement</param>
        private static void AddTeaspoonsToCollection(ref RecipeFraction teaspoons, ICollection<Measurement> measurements)
        {

            if (teaspoons.CompareTo(0) > 0)
            {
                measurements.Add(new USVolumeMeasurement(teaspoons, Unit.TEASPOON));
            }

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
        /// The OptimizeMeasurements method modifies a collection of common measurements to make it easier for kitchen use
        /// </summary>
        /// <param name="measurements">ICollection of Measurements</param>
        private static void OptimizeMeasurements(ICollection<Measurement> measurements)
        {
            //if only 1/2 a tablespoon and has teaspoons, better to take out the tablespoons and turn have 1 1/2 more teaspoons
            int size = measurements.Count;
            if (size > 1)
            {
                //can cast as USVolumeMeasurement because was created in this class. List is of type Measurement to be usable across multiple types of measurements
                USVolumeMeasurement secondToLast = (USVolumeMeasurement)measurements.ElementAt(size - 2);
                /* check if second-to-last element is tablespoon 
                 (if it is, the last one is teaspoons because it is the only unit less than tablespoons) */
                if (secondToLast.UnitSize == Unit.TABLESPOON)
                {
                    if (secondToLast.Amount.Equals(new Fraction(1, 2))) //if tablespoons is 1/2 (don't have to use EqualsValue because it has been simplified)
                    {
                        //can cast as USVolumeMeasurement because was created in this class. List is of type Measurement to be usable across multiple types of measurements
                        USVolumeMeasurement teaspoons = (USVolumeMeasurement)measurements.ElementAt(size - 1);
                        teaspoons.Amount += new Fraction(RecipeConstants.TSP_PER_TBSP, 2);
                        teaspoons.Amount.Simplify();

                        measurements.Remove(secondToLast);
                    }

                }
            }
        }

        /// <summary>
        /// The ToString method returns a string representation of the USVolumeMeasurement
        /// </summary>
        /// <returns>String representation of the USVolumeMeasurement</returns>
        public override string ToString()
        {
            //add S to end of unit if more than 1
            string unitString = Amount.CompareTo(1) > 0 ? UnitSize.ToString().ToLower() + "s" : UnitSize.ToString().ToLower();
            return Amount.ToString() + " " + unitString;
        }

        /// <summary>
        /// The ToHTMLFormattedString method returns a string that includes HTML tags to better display fractions
        /// </summary>
        /// <returns>String containing HTML-formatted representation of USVolumeMeasurement </returns>
        public override string ToHTMLFormattedString()
        {
            string unitString = Amount.CompareTo(1) > 0 ? UnitSize.ToString().ToLower() + "s" : UnitSize.ToString().ToLower();
            return Amount.ToHTMLFormattedString() + " " + unitString;
        }

        /// <summary>
        /// The Copy method returns a deep copy of the Measurement
        /// </summary>
        /// <returns>Copy of Measurement object</returns>
        public override Measurement Copy()
        {
            return new USVolumeMeasurement(Amount.Copy(), UnitSize);
        }

    }

}