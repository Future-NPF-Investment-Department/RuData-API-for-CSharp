namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular NRA agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class NRAAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular NRA agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public NRAAttribute(params string[] ratings) : base(NRA, ratings) { }
    }
}
