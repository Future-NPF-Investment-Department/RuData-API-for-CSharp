namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular Expert RA agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class ExpRAAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular Exper RA agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public ExpRAAttribute(params string[] ratings) : base(EXPRA, ratings) { }
    }
}
