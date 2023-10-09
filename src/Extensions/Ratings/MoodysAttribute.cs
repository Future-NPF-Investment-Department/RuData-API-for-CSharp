namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents particular Moody's agency ratings bucket. 
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    public class MoodysAttribute : AgencyRatingBucketAttribute
    {
        /// <summary>
        ///     Represents particular Moody's agency ratings bucket. 
        ///     All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="ratings">Set of ratings that determines this particular bucket.</param>
        public MoodysAttribute(params string[] ratings) : base(MOODYS, ratings) { }
    }
}
