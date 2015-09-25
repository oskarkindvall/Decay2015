using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Decay
{
    public class SimulationManager
    {
        //String parameters
        static string _decimalSeparator;
        const string DELIMINATOR = ";";

        //General parameters
        static DecayModel _selectedModel;
        static int _maxDecayClass;
        static int _maxTimeStep;

        //Simulated objects
        static List<ForestStand> _forestStands;

        public SimulationManager(DecayModel model, int maxDecompositionClass, int maxTimeStep)
        {
            _decimalSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            _selectedModel = model;
            _maxDecayClass = maxDecompositionClass;
            _maxTimeStep = maxTimeStep;
            _forestStands = new List<ForestStand>();
        }

        /// <summary>
        /// Simulates decay of logs existing at time step 0.
        /// </summary>
        /// <param name="inputFile">Input file with initial values.</param>
        /// <returns>True if input file is read successfully.</returns>
        public bool SetInitialValues(string inputFileName)
        {
            bool success = true;
            String rowText;
            String[] colTexts;
            Int64 rowCount = 0;

            string plotId;
            int period = 0;
            TreeSpecies species;
            String treeSizeClass;

            Double numberOfStems, volume, biomass, diameter, density, decayClass;
            CohortStatistics cohort;

            // Open the input file to read initial values.
            System.IO.Stream fileStream = File.OpenRead(inputFileName);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    rowCount++;
                    rowText = reader.ReadLine();
                    
                    if (_decimalSeparator == ",")
                    {
                        rowText = rowText.Replace(".", ",");
                    }
                    else
                    {
                        rowText = rowText.Replace(",", ".");
                    }

                    if (rowCount > 1)
                    {
                        colTexts = rowText.Split(";".ToCharArray());
                        cohort = null;

                        if (TreeSpecies.TryParse(colTexts[InitialValuesInputColIndex.Species], out species) &&
                            Double.TryParse(colTexts[InitialValuesInputColIndex.NumberOfStems], out numberOfStems) &&
                            Double.TryParse(colTexts[InitialValuesInputColIndex.Volume], out volume) &&
                            Double.TryParse(colTexts[InitialValuesInputColIndex.Biomass], out biomass) &&
                            Double.TryParse(colTexts[InitialValuesInputColIndex.Diameter], out diameter) &&
                            Double.TryParse(colTexts[InitialValuesInputColIndex.DecayClass], out decayClass))
                        {

                            plotId = colTexts[MortalityInputColIndex.Plot];
                            treeSizeClass = colTexts[MortalityInputColIndex.DiameterClass];

                            if (numberOfStems > 0)
                            {
                                density = biomass / volume;
                                cohort = new CohortStatistics(
                                    period, plotId, species, treeSizeClass, _selectedModel, _maxDecayClass, _maxTimeStep,
                                    numberOfStems, volume, biomass, density, diameter);

                                if (cohort.Iterations.Count > 0)
                                {
                                    ForestStand newStand = new ForestStand(plotId, _maxTimeStep, _maxDecayClass, _selectedModel);
                                    newStand.Cohorts.Add(cohort);
                                    _forestStands.Add(newStand);
                                }
                            }
                        }
                        else success = false;
                    }
                }
            }
            fileStream.Close();

            return success;
        }

        /// <summary>
        /// Simulates decay of trees that die each time step.
        /// </summary>
        /// <param name="inputFile">Input file with time step specific mortalities.</param>
        /// <returns>True if input file is read successfully.</returns>
        public bool SimulateDecayOfAddedLogs(string inputFileName)
        {
            bool success = true;
            String rowText;
            String[] colTexts;
            Int64 rowCount = 0;

            string plotId;
            int period = 0;
            TreeSpecies species;
            String treeSizeClass;

            Double numberOfStems, volume, biomass, diameter, density;
            CohortStatistics cohort;

            // Open the input file to read initial values.
            System.IO.Stream fileStream = File.OpenRead(inputFileName);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    rowCount++;
                    rowText = reader.ReadLine();

                    if (_decimalSeparator == ",")
                    {
                        rowText = rowText.Replace(".", ",");
                    }
                    else
                    {
                        rowText = rowText.Replace(",", ".");
                    }

                    if (rowCount > 1)
                    {
                        colTexts = rowText.Split(";".ToCharArray());
                        cohort = null;
                        if (Int32.TryParse(colTexts[MortalityInputColIndex.Period], out period) &&
                            TreeSpecies.TryParse(colTexts[MortalityInputColIndex.Species], out species) &&
                            Double.TryParse(colTexts[MortalityInputColIndex.NumberOfStems], out numberOfStems) &&
                            Double.TryParse(colTexts[MortalityInputColIndex.Volume], out volume) &&
                            Double.TryParse(colTexts[MortalityInputColIndex.Biomass], out biomass) &&
                            Double.TryParse(colTexts[MortalityInputColIndex.Diameter], out diameter))
                        {
                            if (numberOfStems > 0)
                            {
                                plotId = colTexts[MortalityInputColIndex.Plot];
                                treeSizeClass = colTexts[MortalityInputColIndex.DiameterClass];
                                density = biomass / volume;
                                cohort = new CohortStatistics(
                                    period, plotId, species, treeSizeClass,
                                    _selectedModel, _maxDecayClass, _maxTimeStep,
                                    numberOfStems, volume, biomass, density, diameter);


                                int i = _forestStands.FindIndex(delegate(ForestStand stand)
                                {
                                    return stand.Plot == plotId;
                                });

                                if (i > -1)
                                {
                                    ForestStand currentStand = _forestStands[i];

                                    currentStand.Cohorts.Add(cohort);

                                }
                                else
                                {
                                    ForestStand newStand = new ForestStand(plotId, _maxTimeStep, _maxDecayClass, _selectedModel);

                                    newStand.Cohorts.Add(cohort);

                                    _forestStands.Add(newStand);
                                }
                            }
                        }
                        else success = false;
                    }
                }
            }
            fileStream.Close();

            return success;
        }

        /// <summary>
        /// Saves simulation results to file.
        /// </summary>
        /// <param name="outputFileName">Name of output csv file.</param>
        /// <returns>True if output file was saved successfully.</returns>
        public bool SaveResults(string outputFileName)
        {
            bool success = true;

            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            StringBuilder records = new StringBuilder();
            records.Append(OutputColNames.Period);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.Plot);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.Species);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.DiameterClass);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.NumberOfStems);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.Volume);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.Biomass);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.Diameter);
            records.Append(DELIMINATOR);
            records.Append(OutputColNames.DecayClass);
            records.AppendLine();

            if (_forestStands != null)
            {
                foreach (var forestStand in _forestStands)
                {
                    forestStand.CalculateTimeSerie();
                    foreach (var timeStep in forestStand.TimeSerieOfForestStandStatisticses)
                    {
                        records.Append(timeStep.Period);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.Plot);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.Species);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.SizeClass);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.NumberOfStems);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.Volume);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.Biomass);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.AverageDiameter);
                        records.Append(DELIMINATOR);
                        records.Append(timeStep.AverageDecayClass);
                        records.AppendLine();
                    }
                }
            }

            // Append the records to a new file.
            using (StreamWriter outfile = new StreamWriter(outputFileName))
            {
                outfile.Write(records.ToString());
            }

            success = File.Exists(outputFileName);

            return success;
        }
    }
}
