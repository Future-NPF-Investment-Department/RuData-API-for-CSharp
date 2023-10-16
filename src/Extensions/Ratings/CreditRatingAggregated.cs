#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals(object o)

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents issuer's aggregated credit rating.
    /// </summary>
    public class CreditRatingAggregated
    {
        /// <summary>
        ///     Name of issuer that is subject for credit rating action.
        /// </summary>
        public string Issuer { get; init; } = "Unknown issuer";

        /// <summary>
        ///     International aggregated credit rating value. Based on BIG3 (FITCH, SnP, Moodys) rating scale.
        /// </summary>
        public CreditRatingUS RatingBig3 { get; init; }

        /// <summary>
        ///     National (RU) aggregated credit rating value.
        /// </summary>
        public CreditRatingRU RatingRu { get; init; }

        /// <summary>
        ///     Agencies ratings used to 
        /// </summary>
        public IReadOnlyList<CreditRating>? Ratings { get; init; }

        /// <summary>
        ///     Probability of default (PD) that corresponds to <see cref="AggregatedBig3"/> rating.
        /// </summary>
        public double DefaultProbability { get; init; }

        /// <summary>
        ///     Aggregates ratings 
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        public static CreditRatingAggregated AggregateFrom(CreditRating[] ratings)
        {
            if (ratings.Length is 0)
                return new CreditRatingAggregated();
            return  RuDataTools.CreateAggregatedRating(ratings);
        }

        /// <summary>
        ///     Provides stylized rating description.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string head = $"Aggregated rating for '{Issuer}':\n";
            string values = $"National scale: {RatingRu.ToRatingString()}\nInternational scale: {RatingBig3.ToRatingString()}\n";
            string pd = $"Probability of default: {DefaultProbability:0.00%}\n";
            string lastRatingsHead = string.Empty;
            string rawratings = string.Empty;

            if (Ratings is not null)
            {
                lastRatingsHead = $"\nMost recent ratings:\n";
                foreach(var r in Ratings)
                    rawratings += r.ToShortString() + '\n';
            }
            return head + values + pd + lastRatingsHead + rawratings;
        }

        public static bool operator <=(CreditRatingAggregated rating1, CreditRatingAggregated rating2)
            => rating1.RatingBig3 <= rating2.RatingBig3;

        public static bool operator >=(CreditRatingAggregated rating1, CreditRatingAggregated rating2)
            => rating1.RatingBig3 >= rating2.RatingBig3;

        public static bool operator <=(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 <= value;

        public static bool operator >=(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 >= value;

        public static bool operator <(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 < value;

        public static bool operator >(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 > value;

        public static bool operator <(CreditRatingAggregated rating1, CreditRatingRU value)
            => rating1.RatingRu < value;

        public static bool operator >(CreditRatingAggregated rating1, CreditRatingRU value)
            => rating1.RatingRu > value;

        public static bool operator ==(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 == value;

        public static bool operator !=(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 != value;
    }
}
