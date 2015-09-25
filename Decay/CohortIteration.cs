using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Holds information of simulation results for specific iteration time step of a cohort.
    /// This class perform the calculation of decay from one time period to the next.
    /// </summary>
    public class CohortIteration
    {
        public CohortIteration(
            Int64 period,
            Int64 timeStepLength,
            Double initialNumberOfTrees,
            Double initialVolume,
            Double initialBiomass,
            Double initialDensity,
            TreeSpecies species,
            bool performCalculation,
            DecayModel model)
        {
            Period = period;
            Species = species;
            Model = model;

            if (performCalculation)
            {
                //V_t = V_0 * exp(- alpha*t)
                Volume = initialVolume * Math.Exp(-GetVolumeAlpha() * timeStepLength);
                Biomass = initialBiomass * Math.Exp(-GetBiomassAlpha() * timeStepLength);
                Density = initialDensity * Math.Exp(-GetDensityAlpha() * timeStepLength);
            }
            else
            {
                Volume = initialVolume;
                Biomass = initialBiomass;
                Density = initialDensity;
            }
            DecayClass = GetDecayClass(Density);
            NumberOfStems = initialNumberOfTrees;
        }


        /// <summary>
        /// TimeStep corresponding to 5 years.
        /// </summary>
        public Int64 Period { get; set; }

        /// <summary>
        /// The species of the stem.
        /// </summary>
        public TreeSpecies Species { get; set; }

        /// <summary>
        /// Number of stems.
        /// </summary>
        public Double NumberOfStems { get; set; }

        /// <summary>
        /// Decay class of of the cohort at current period.
        /// </summary>
        public int DecayClass { get; set; }

        /// <summary>
        /// Volume of stems.
        /// </summary>
        public Double Volume { get; set; }

        /// <summary>
        /// Density of stems.
        /// </summary>
        public Double Density { get; set; }

        /// <summary>
        /// Biomass of stems
        /// </summary>
        public Double Biomass { get; set; }

        /// <summary>
        /// Current model used for decay calculations
        /// </summary>
        public DecayModel Model { get; set; }

        /// <summary>
        /// Get alpha for Volume calculations
        /// </summary>
        /// <returns></returns>
        private Double GetVolumeAlpha()
        {
            switch (Model)
            {
                case (DecayModel.OneTimeRegression):
                    {
                        switch (Species)
                        {

                            case TreeSpecies.Aspen:
                                return 0.013; //As Birch
                            case TreeSpecies.Birch:
                                return 0.013;
                            case TreeSpecies.Spruce:
                                return 0.013;
                            case TreeSpecies.Pine:
                                return 0.010;
                            default:
                                return 0;
                        }
                    }
                default: //Decay model = Vector
                    {
                        switch (Species)
                        {
                            case TreeSpecies.Aspen:
                                return 0.025; //As Birch
                            case TreeSpecies.Birch:
                                return 0.025;
                            case TreeSpecies.Spruce:
                                return 0.032;
                            case TreeSpecies.Pine:
                                return 0.039;
                            default:
                                return 0;
                        }
                    }
            }
        }

        /// <summary>
        /// Get alpha for density calculations
        /// </summary>
        /// <returns></returns>
        private Double GetDensityAlpha()
        {
            switch (Model)
            {
                case (DecayModel.OneTimeRegression):
                    {
                        switch (Species)
                        {
                            case TreeSpecies.Aspen:
                                return 0.042; //As Birch
                            case TreeSpecies.Birch:
                                return 0.042;
                            case TreeSpecies.Spruce:
                                return 0.027;
                            case TreeSpecies.Pine:
                                return 0.024;
                            default:
                                return 0;
                        }
                    }
                default: //Decay model = Vector
                    {
                        switch (Species)
                        {
                            case TreeSpecies.Aspen:
                                return 0.083; //As Birch
                            case TreeSpecies.Birch:
                                return 0.083;
                            case TreeSpecies.Spruce:
                                return 0.037;
                            case TreeSpecies.Pine:
                                return 0.019;
                            default:
                                return 0;
                        }
                    }
            }
        }

        /// <summary>
        /// Get alpha for Biomass calculations
        /// </summary>
        /// <returns></returns>
        private Double GetBiomassAlpha()
        {
            switch (Model)
            {
                case (DecayModel.OneTimeRegression):
                    {
                        switch (Species)
                        {
                            case TreeSpecies.Aspen:
                                return 0.046; //As Birch
                            case TreeSpecies.Birch:
                                return 0.046;
                            case TreeSpecies.Spruce:
                                return 0.033;
                            case TreeSpecies.Pine:
                                return 0.035;
                            default:
                                return 0;
                        }
                    }
                default: //Dacay model = Vector
                    {
                        switch (Species)
                        {
                            case TreeSpecies.Aspen:
                                return 0.108; //As Birch
                            case TreeSpecies.Birch:
                                return 0.108;
                            case TreeSpecies.Spruce:
                                return 0.067;
                            case TreeSpecies.Pine:
                                return 0.050;
                            default:
                                return 0;
                        }
                    }
            }
        }

        /// <summary>
        /// The method that estimate the decay class for a given wood density
        /// </summary>
        /// <param name="density"></param>
        /// <returns></returns>
        private int GetDecayClass(Double density)
        {
            // Conversion between tree density and RT decomposition classes 0-4
            // We will receive tree-specific parameters values for tree density OR data on Biomass and Volume
            // In mail från Tomas Lämås:
            // Beräkning av nedbrytningsklass utifrån densitet:
            // 1.  För en stam med en viss densitet, gå in i tabellen nedan (jfr Tabell 1 i attached) 
            //     och välj den klass som har högre densitet. 
            //     Utom om densiteten ligger utanför högsta resp. lägsta värde i tabellen, välj då högsta resp. lägsta densitet. 
            // 2.  Ta den nedbrytningsklass 0 - 9 som motsvarar den valda densiteten
            // 3.  Dela nedbrytningsklassen med 2
            // 4.  Avrunda nedåt så erhålls klass 0 - 4

            Double decayClass = 0;
            switch (Species)
            {
                case TreeSpecies.Aspen: //beräknas som björk
                    {
                        if (density >= 0.4745)
                        {
                            decayClass = 0;
                        }
                        else if (density >= 0.41955)
                        {
                            decayClass = 1;
                        }
                        else if (density >= 0.3646)
                        {
                            decayClass = 2;
                        }
                        else if (density >= 0.31725)
                        {
                            decayClass = 3;
                        }
                        else if (density >= 0.2699)
                        {
                            decayClass = 4;
                        }
                        else if (density >= 0.2315)
                        {
                            decayClass = 5;
                        }
                        else if (density >= 0.1931)
                        {
                            decayClass = 6;
                        }
                        else if (density >= 0.1573)
                        {
                            decayClass = 7;
                        }
                        else if (density >= 0.1215)
                        {
                            decayClass = 8;
                        }
                        else if (density >= 0.09)
                        {
                            decayClass = 9;
                        }
                        else
                        {
                            decayClass = 9;
                        }
                    }
                    break;
                case TreeSpecies.Birch:
                    {
                        if (density >= 0.4745)
                        {
                            decayClass = 0;
                        }
                        else if (density >= 0.41955)
                        {
                            decayClass = 1;
                        }
                        else if (density >= 0.3646)
                        {
                            decayClass = 2;
                        }
                        else if (density >= 0.31725)
                        {
                            decayClass = 3;
                        }
                        else if (density >= 0.2699)
                        {
                            decayClass = 4;
                        }
                        else if (density >= 0.2315)
                        {
                            decayClass = 5;
                        }
                        else if (density >= 0.1931)
                        {
                            decayClass = 6;
                        }
                        else if (density >= 0.1573)
                        {
                            decayClass = 7;
                        }
                        else if (density >= 0.1215)
                        {
                            decayClass = 8;
                        }
                        else if (density >= 0.09)
                        {
                            decayClass = 9;
                        }
                        else
                        {
                            decayClass = 9;
                        }
                    }
                    break;
                case TreeSpecies.Spruce:
                    {
                        if (density >= 0.3060)
                        {
                            decayClass = 0;
                        }
                        else if (density >= 0.2987)
                        {
                            decayClass = 1;
                        }
                        else if (density >= 0.2915)
                        {
                            decayClass = 2;
                        }
                        else if (density >= 0.26635)
                        {
                            decayClass = 3;
                        }
                        else if (density >= 0.2412)
                        {
                            decayClass = 4;
                        }
                        else if (density >= 0.208)
                        {
                            decayClass = 5;
                        }
                        else if (density >= 0.1748)
                        {
                            decayClass = 6;
                        }
                        else if (density >= 0.15295)
                        {
                            decayClass = 7;
                        }
                        else if (density >= 0.1311)
                        {
                            decayClass = 8;
                        }
                        else if (density >= 0.11)
                        {
                            decayClass = 9;
                        }
                        else
                        {
                            decayClass = 9;
                        }
                    }
                    break;
                case TreeSpecies.Pine:
                    {
                        if (density >= 0.3360)
                        {
                            decayClass = 0;
                        }
                        else if (density >= 0.3244)
                        {
                            decayClass = 1;
                        }
                        else if (density >= 0.3128)
                        {
                            decayClass = 2;
                        }
                        else if (density >= 0.28245)
                        {
                            decayClass = 3;
                        }
                        else if (density >= 0.2521)
                        {
                            decayClass = 4;
                        }
                        else if (density >= 0.23735)
                        {
                            decayClass = 5;
                        }
                        else if (density >= 0.2226)
                        {
                            decayClass = 6;
                        }
                        else if (density >= 0.19015)
                        {
                            decayClass = 7;
                        }
                        else if (density >= 0.1577)
                        {
                            decayClass = 8;
                        }
                        else if (density >= 0.13)
                        {
                            decayClass = 9;
                        }
                        else
                        {
                            decayClass = 9;
                        }
                    }
                    break;
                default:
                    {
                        decayClass = 9;
                    }
                    break;
            }
            return (Int32)Math.Floor(decayClass / 2);
        }
    }
}
