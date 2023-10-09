namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular FITCH agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class FitchAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular FITCH agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public FitchAttribute(params string[] ratings) : base(FITCH, ratings) { }
    }
}
