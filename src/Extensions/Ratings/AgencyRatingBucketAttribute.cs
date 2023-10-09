namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents abstract rating bucket for rating agency.
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class AgencyRatingBucketAttribute : Attribute
    {
        // these constants are taken from EFIR raings agencies classification.
        protected const string FITCH = "Fitch Ratings";
        protected const string MOODYS = "Moody's";
        protected const string SNP = "Standard & Poor's";
        protected const string AKRA = "АКРА";
        protected const string EXPRA = "Эксперт РА";
        protected const string NKR = "НКР";
        protected const string NRA = "НРА";

        private protected readonly string _agency;
        private protected readonly string[] _bucket;

        /// <summary>
        ///     Represents particular rating bucket for specified rating agency.
        ///      All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="agency">Rating agency.</param>
        /// <param name="ratingsBucket">Set of ratings that determines this particular bucket.</param>
        public AgencyRatingBucketAttribute(string agency, string[] ratingsBucket)
        {
            _agency = agency;
            _bucket = ratingsBucket;
        }

        /// <summary>
        ///     Defiens if specified agency-rating pair belongs to this bucket.
        /// </summary>
        /// <param name="agency">Rating agency.</param>
        /// <param name="rating">Credit rating value.</param>
        /// <returns>True if specified agency rating belongs to this bucket, otherwise false.</returns>
        internal bool ContainsAgencyRating(string agency, string rating)
            => _agency == agency && rating.Contains(agency);
    }
}
