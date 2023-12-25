using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Credit Rating aggregation method.
    /// </summary>
    public enum RatingAggregationMethod
    {
        /// <summary>
        ///     All ratings are used for aggregation.
        /// </summary>
        Any,

        /// <summary>
        ///     Maximum rating is selected.
        /// </summary>
        Max,

        /// <summary>
        ///     Minimum rating is selected.
        /// </summary>
        Min,
    }
}
