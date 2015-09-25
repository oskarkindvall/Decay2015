using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Column indexes for input csv files with mortality data.
    /// </summary>
    public struct MortalityInputColIndex
    {
        public const int Period = 0;
        public const int Plot = 1;
        public const int Species = 2;
        public const int DiameterClass = 3;
        public const int NumberOfStems = 4;
        public const int Volume = 5;
        public const int Biomass = 6;
        public const int Diameter = 7;
    }

    /// <summary>
    /// Column indexes for input csv files with initial values.
    /// </summary>
    public struct InitialValuesInputColIndex
    {
        public const int Period = 0;
        public const int Plot = 1;
        public const int Species = 2;
        public const int DiameterClass = 3;
        public const int NumberOfStems = 4;
        public const int Volume = 5;
        public const int Biomass = 6;
        public const int Diameter = 7;
        public const int DecayClass = 8;
    }

    /// <summary>
    /// Column headers for the output csv file.
    /// </summary>
    public struct OutputColNames
    {
        //Combinations
        public const string Period = "Period";
        public const string Plot = "Plot";
        public const string Species = "Tree Species";
        public const string DiameterClass = "Diameter class";

        //Results for all trees
        public const string NumberOfStems = "Number of stems";
        public const string Volume = "Volume";
        public const string Biomass = "Biomass";
        public const string Diameter = "Average DBH";
        public const string DecayClass = "Average decay class";
    }
}
