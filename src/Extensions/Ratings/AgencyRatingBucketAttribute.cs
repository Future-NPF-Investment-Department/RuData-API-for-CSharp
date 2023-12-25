using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents abstract rating bucket for rating agency.
    ///     All ratings from this bucket should imply equivalent level of credit risk.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class AgencyRatingBucketAttribute : Attribute
    {
        // rating agencies names constants
        protected const RatingAgency FITCH      = RatingAgency.FITCH;              
        protected const RatingAgency MOODYS     = RatingAgency.MOODYS;      
        protected const RatingAgency SNP        = RatingAgency.SNP;            
        protected const RatingAgency AKRA       = RatingAgency.AKRA;          
        protected const RatingAgency EXPRA      = RatingAgency.RAEX;         
        protected const RatingAgency NKR        = RatingAgency.NKR;            
        protected const RatingAgency NRA        = RatingAgency.NRA;            

        private protected readonly RatingAgency _agency;
        private protected readonly string[] _bucket;

        /// <summary>
        ///     Represents particular rating bucket for specified rating agency.
        ///      All ratings from this bucket should imply equivalent level of credit risk.
        /// </summary>
        /// <param name="agency">Rating agency.</param>
        /// <param name="ratingsBucket">Set of ratings that determines this particular bucket.</param>
        public AgencyRatingBucketAttribute(RatingAgency agency, string[] ratingsBucket)
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
        internal bool ContainsAgencyRating(RatingAgency agency, string rating)
            => _agency == agency && _bucket.Contains(rating);
    }
}
