namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular Akra agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class AKRAAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular Akra agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public AKRAAttribute(params string[] ratings) : base(AKRA, ratings) { }            
    }
}
