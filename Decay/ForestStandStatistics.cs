using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Class that holds information about the forest parameters at one stand in a particular time step.
    /// </summary>
    public class ForestStandStatistics
    {

        private List<Double> _volumeValues;
        private List<Double> _decayClassValues;
        private List<Double> _diameterValues;

        private Double _biomass;
        private List<Double> _numberOfStemsValues;

        public ForestStandStatistics(
            Int64 period,
            String plot,
            TreeSpecies species,
            String sizeClass
            )
        {
            Period = period;
            Plot = plot;
            Species = species;
            SizeClass = sizeClass;

            _volumeValues = new List<double>();
            _decayClassValues = new List<double>();
            _diameterValues = new List<double>();
            _numberOfStemsValues = new List<double>();
            _biomass = 0;
        }

        /// <summary>
        /// TimeStep corresponding to 5 years.
        /// </summary>
        public Int64 Period { get; set; }

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

        

        public void UpdateDecayClassStatistics(Double value)
        {
            if (value > -1)
            {
                _decayClassValues.Add(value);
            }
        }

        public void UpdateVolumeStatistics(Double value)
        {
            if (value > -1)
            {
                _volumeValues.Add(value);
            }
        }

        public void UpdateDiameterStatistics(Double value)
        {
            if (value > -1)
            {
                _diameterValues.Add(value);
            }
        }

        public void UpdateNumberOfStemStatistics(Double value)
        {
            if (value > -1)
            {
                _numberOfStemsValues.Add(value);
            }
        }

        public void UpdateBiomassStatistics(Double value)
        {
            _biomass = _biomass + value;
        }

        /// <summary>
        /// Average Decay class of the stems existing during the current period weighted by number of stems.
        /// </summary>
        public Double AverageDecayClass
        {
            get
            {
                Double sum = 0;
                if (_decayClassValues.Count > 0 && NumberOfStems > 0)
                {
                    for (int i = 0; i < _decayClassValues.Count; i++)
                    {
                        sum = sum + (_decayClassValues[i] * _numberOfStemsValues[i]);
                    }
                    return sum / NumberOfStems;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Average diameter of the stems existing during the current period weighted by number of stems.
        /// </summary>
        public Double AverageDiameter
        {
            get
            {
                Double sum = 0;
                if (_diameterValues.Count > 0 && NumberOfStems > 0)
                {
                    for (int i = 0; i < _diameterValues.Count; i++)
                    {
                        sum = sum + (_diameterValues[i] * _numberOfStemsValues[i]);
                    }
                    return sum / NumberOfStems;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Number of stems existing during current period.
        /// </summary>
        public Double NumberOfStems
        {
            get
            {
                Double sum = 0;
                foreach (var item in _numberOfStemsValues)
                {
                    sum = sum + item;
                }
                return sum;
            }
        }

        /// <summary>
        /// Volume of the stems existing during current period.
        /// </summary>
        public Double Volume
        {
            get
            {
                Double sum = 0;
                foreach (var item in _volumeValues)
                {
                    sum = sum + item;
                }
                return sum;
            }
        }

        /// <summary>
        /// Biomass of the stems existing current period.
        /// </summary>
        public Double Biomass
        {
            get
            {
                return _biomass;
            }
        }
    }
}
