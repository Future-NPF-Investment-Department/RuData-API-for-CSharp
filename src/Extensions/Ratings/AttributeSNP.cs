namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular SnP agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class SnPAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular SnP agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public SnPAttribute(params string[] ratings) : base(SNP, ratings) { }
    }
}
