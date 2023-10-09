namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from NKR agency scale to map.
    /// </summary>
    public class NKRAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular NKR agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public NKRAttribute(params string[] ratings) : base(NKR, ratings) { }
    }
}
