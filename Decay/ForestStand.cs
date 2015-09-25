using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Holds all information from input file and all calculated statistics on decay over time
    /// </summary>
    public class ForestStand
    {
        public ForestStand(
            String plot,
            int maxTimeStep,
            int maxDecayClass,
            DecayModel model)
        {
            Plot = plot;
            MaxTimeStep = maxTimeStep;
            MaxDecayClass = maxDecayClass;
            Cohorts = new List<CohortStatistics>();
            TimeSerieOfForestStandStatisticses = new List<ForestStandStatistics>();
        }

        /// <summary>
        /// The Id of the forest stand.
        /// </summary>
        public String Plot { get; set; }

        /// <summary>
        /// The number of iterations
        /// </summary>
        public int MaxTimeStep { get; set; }

        /// <summary>
        /// Maximum decay class
        /// </summary>
        public int MaxDecayClass { get; set; }

        /// <summary>
        /// List of all cohorts of stems.
        /// </summary>
        public List<CohortStatistics> Cohorts { get; set; }

        /// <summary>
        /// List of time step specific summary statistics
        /// </summary>
        public List<ForestStandStatistics> TimeSerieOfForestStandStatisticses { get; set; }



        

        /// <summary>
        /// This method calculates the different sets o times series that are specific for Stem diameter class and Tree species.
        /// </summary>
        public void CalculateTimeSerie()
        {
            int index = 0;
            foreach (var cohort in Cohorts)
            {
                foreach (var iteration in cohort.Iterations)
                {
                    index = cohort.Iterations.IndexOf(iteration);
                    
                    if (iteration.DecayClass <= MaxDecayClass && iteration.Period <= MaxTimeStep)
                    {
                        int i = TimeSerieOfForestStandStatisticses.FindIndex(delegate(ForestStandStatistics timeStep)
                        {
                            return (timeStep.Period == iteration.Period &&
                                    timeStep.SizeClass == cohort.SizeClass &&
                                    timeStep.Species == cohort.Species);
                        });


                        if (i > -1)
                        {
                            ForestStandStatistics existingTimeStep = TimeSerieOfForestStandStatisticses[i];

                            existingTimeStep.UpdateVolumeStatistics(cohort.Iterations[index].Volume);
                            existingTimeStep.UpdateDecayClassStatistics(cohort.Iterations[index].DecayClass);
                            existingTimeStep.UpdateDiameterStatistics(cohort.DiameterAtBrestHeightCreated);
                            existingTimeStep.UpdateNumberOfStemStatistics(cohort.Iterations[index].NumberOfStems);
                            existingTimeStep.UpdateBiomassStatistics(cohort.Iterations[index].Biomass);
                        }
                        else
                        {
                            ForestStandStatistics newTimeStep = new ForestStandStatistics(
                                iteration.Period,
                                Plot,
                                cohort.Species,
                                cohort.SizeClass);

                            newTimeStep.UpdateVolumeStatistics(cohort.Iterations[index].Volume);
                            newTimeStep.UpdateDecayClassStatistics(cohort.Iterations[index].DecayClass);
                            newTimeStep.UpdateDiameterStatistics(cohort.DiameterAtBrestHeightCreated);
                            newTimeStep.UpdateNumberOfStemStatistics(cohort.Iterations[index].NumberOfStems);
                            newTimeStep.UpdateBiomassStatistics(cohort.Iterations[index].Biomass);

                            TimeSerieOfForestStandStatisticses.Add(newTimeStep);
                        }
                    }
                }
            }
        }
    }
}
