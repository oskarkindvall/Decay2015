using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Class that holds statistics for a stand specific cohort of decomposing trees.
    /// </summary>
    public class CohortStatistics
    {
        private const int TimeStepLength = 5;

        public CohortStatistics(
            int periodCreated,
            String plot,
            TreeSpecies species,
            String sizeClass,
            DecayModel model,
            int maxDecayClass,
            int maxTimeStep,
            Double numberOfStemsCreated,
            Double volumeCreated,
            Double biomassCreated,
            Double densityCreated,
            Double diameterCreated
            )
        {
            CohortIteration iteration;

            PeriodCreated = periodCreated;
            Plot = plot;
            Species = species;
            SizeClass = sizeClass;
            NumberOfStemsCreated = numberOfStemsCreated;
            VolumeCreated = volumeCreated;
            BiomassCreated = biomassCreated;
            DiameterAtBrestHeightCreated = diameterCreated;

            int decayClass = 0;
            int period = periodCreated;
            Double numberOfStems = NumberOfStemsCreated;
            Double volume = volumeCreated;
            Double biomass = biomassCreated;
            Double density = densityCreated;
            Iterations = new List<CohortIteration>();
            iteration = new CohortIteration(period++, TimeStepLength, numberOfStems, volume, biomass, density, Species, false, model);
            Iterations.Add(iteration);
            while (decayClass < maxDecayClass && period < (periodCreated + maxTimeStep)) //Iterates the decay of the cohort of all dead tree stems until reaching max decay class.
            {
                iteration = new CohortIteration(period++, TimeStepLength, numberOfStems, volume, biomass, density, Species, true, model);
                Iterations.Add(iteration);
                decayClass = iteration.DecayClass;
                volume = iteration.Volume;
                biomass = iteration.Biomass;
                density = iteration.Density;
            }
        }

        /// <summary>
        /// TimeStep corresponding to 5 years.
        /// </summary>
        public Int64 PeriodCreated { get; set; }

        /// <summary>
        /// The Id of the forest stand.
        /// </summary>
        public String Plot { get; set; }

        /// <summary>
        /// The species of the stem.
        /// </summary>
        public TreeSpecies Species { get; set; }

        /// <summary>
        /// The dimention of the stem.
        /// </summary>
        public String SizeClass { get; set; }

        /// <summary>
        /// Maximum decay class. When logs enter the decay class greater than this value the logs are omitted.
        /// </summary>
        public int MaxDecayClass { get; set; }
        
        /// <summary>
        /// Max number of time steps simulated for this cohort.
        /// </summary>
        public int MaxTimeStep { get; set; }

        /// <summary>
        /// Number of stems added current period.
        /// </summary>
        public Double NumberOfStemsCreated { get; set; }

        /// <summary>
        /// Volume of the stems added current period.
        /// </summary>
        public Double VolumeCreated { get; set; }

        /// <summary>
        /// Biomass of the stems added current period.
        /// </summary>
        public Double BiomassCreated { get; set; }

        /// <summary>
        /// Diameter of the stems added current period.
        /// </summary>
        public Double DiameterAtBrestHeightCreated { get; set; }

        /// <summary>
        /// The time step specific iteration results of this cohort.
        /// </summary>
        public List<CohortIteration> Iterations { get; set; }
    }
}
