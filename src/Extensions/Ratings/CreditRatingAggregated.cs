namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents issuer's aggregated credit rating.
    /// </summary>
    public class CreditRatingAggregated
    {
        public static readonly CreditRatingAggregated Default = new();

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
        ///     Provides stylized rating description.
        /// </summary>
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

        /// <summary>
        ///     Returns stylized rating in BIG3 scale.
        /// </summary>
        public string ToShortStringBig3() 
            => RatingBig3.ToRatingString();

        /// <summary>
        ///     Returns stylized rating in RU scale.
        /// </summary>
        public string ToShortStringRu() 
            => RatingRu.ToRatingString();        
        
        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is lower than <see cref="CreditRatingUS"/> value. Otherwise false.
        /// </summary>
        public static bool operator <(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 < value;

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is greater than <see cref="CreditRatingUS"/> value. Otherwise false.
        /// </summary>
        public static bool operator >(CreditRatingAggregated rating1, CreditRatingUS value)
            => rating1.RatingBig3 > value;

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is lower than <see cref="CreditRatingRU"/> value. Otherwise false.
        /// </summary>
        public static bool operator <(CreditRatingAggregated rating1, CreditRatingRU value)
            => rating1.RatingRu < value;

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is greater than <see cref="CreditRatingRU"/> value. Otherwise false.
        /// </summary>
        public static bool operator >(CreditRatingAggregated rating1, CreditRatingRU value)
            => rating1.RatingRu > value;
    }
}
