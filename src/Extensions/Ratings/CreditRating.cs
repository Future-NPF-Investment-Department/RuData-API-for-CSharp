#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals(object o)


using Efir.DataHub.Models.Models.Rating;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents credit rating information for particular issuer or security.
    /// </summary>
    public class CreditRating
    {
        public static CreditRating Default = new CreditRating();

        /// <summary>
        ///     Date of credit rating action.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Object to which credit rating action applied.
        /// </summary>`
        public CreditRatingTarget Object { get; set; }

        /// <summary>
        ///     Credit rating scale type.
        /// </summary>
        public CreditRatingScale Scale { get; set; }

        /// <summary>
        ///     Credit rating scale currency.
        /// </summary>
        public CreditRatingCurrency Currency { get; set; }

        /// <summary>
        ///     Credit rating action type.
        /// </summary>
        public CreditRatingAction Action { get; set; } 

        /// <summary>
        ///     International aggregated credit rating value. Based on BIG3 (FITCH, SnP, Moodys) rating scale.
        /// </summary>
        public CreditRatingUS AggregatedBig3 { get; set; }

        /// <summary>
        ///     National (RU) aggregated credit rating value.
        /// </summary>
        public CreditRatingRU AggregatedRu { get; set; }

        /// <summary>
        ///     Credit rating outlook (forecast).
        /// </summary>
        public CreditRatingOutlook Outlook { get; set; }

        /// <summary>
        ///     Particular credit rating.
        /// </summary>
        public string Value { get; set; } = null!;

        /// <summary>
        ///     Previous credit rating.
        /// </summary>
        public string? PreviousValue { get; set; }

        /// <summary>
        ///     Rating agency name.
        /// </summary>
        public string Agency { get; set; } = null!;

        /// <summary>
        ///     Probability of default (PD) that corresponds to <see cref="AggregatedBig3"/> rating.
        /// </summary>
        public double DefaultProbability { get; set; }


        public static CreditRating New(RatingsHistoryFields fields)
        {
            var rating = Default;
            rating.Value = fields.last;
            rating.Agency = fields.rating_agency;
            rating.Date = fields.last_dt ?? throw new NullReferenceException($"Rating action date is null in EFIR database. Rating: {rating.Value}, Agency: {rating.Agency}");

            return rating;
        }

        public static bool operator <=(CreditRating rating1, CreditRating rating2)
            => rating1.AggregatedBig3 <= rating2.AggregatedBig3;

        public static bool operator >=(CreditRating rating1, CreditRating rating2)
            => rating1.AggregatedBig3 >= rating2.AggregatedBig3;

        public static bool operator <=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 <= value;

        public static bool operator >=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 >= value;

        public static bool operator ==(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 == value;

        public static bool operator !=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 != value;
    }
}
