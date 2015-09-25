using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    /// <summary>
    /// Enum representing the different tree species handled by this program.
    /// </summary>
    public enum TreeSpecies
    {
        Birch,
        Spruce,
        Aspen,
        Pine
    }

    /// <summary>
    /// Enum representing the alternative decay models handled by this program.
    /// </summary>
    public enum DecayModel
    {
        OneTimeRegression,
        Vector
    }
}
