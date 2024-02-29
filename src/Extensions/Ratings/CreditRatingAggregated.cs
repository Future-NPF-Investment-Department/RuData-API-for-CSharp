
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
        public string Issuer { get; internal set; } = "Unknown issuer";

        /// <summary>
        ///     International aggregated credit rating value. Based on BIG3 (FITCH, SnP, Moodys) rating scale.
        /// </summary>
        public CreditRatingUS RatingBig3 { get; internal set; }

        /// <summary>
        ///     National (RU) aggregated credit rating value.
        /// </summary>
        public CreditRatingRU RatingRu { get; internal set; }

        /// <summary>
        ///     Agencies ratings used to 
        /// </summary>
        public IEnumerable<CreditRatingAction>? Ratings { get; internal set; }        

        /// <summary>
        ///     Probability of default (PD) that corresponds to <see cref="AggregatedBig3"/> rating.
        /// </summary>
        public double DefaultProbability { get; internal set; } = double.NaN;

        /// <summary>
        ///     Provides stylized rating description.
        /// </summary>
        public override string ToString()
        {
            string head = $"Aggregated rating for '{Issuer}':\n";
            string values = $"National scale: {Rating.ConvertToString(RatingRu)}\nInternational scale: {Rating.ConvertToString(RatingBig3)}\n";
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
        {
            var bitRatings = GetRatingBits(RatingBig3);
            var strings = bitRatings.Select(br => br.ToString()).Where(s => s is not "NR");
            return string.Join(", ", strings);
        }              

        /// <summary>
        ///     Returns stylized rating in RU scale.
        /// </summary>
        public string ToShortStringRu()
        {
            var bitRatings = GetRatingBits(RatingRu);
            var strings = bitRatings.Select(br => br.ToString()).Where(s => s is not "NR");
            return string.Join(", ", strings);
        }

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is lower than <see cref="CreditRatingUS"/> value. Otherwise false.
        /// </summary>
        public static bool operator <(CreditRatingAggregated rating1, CreditRatingUS value)
            => GetRatingBits(rating1.RatingBig3).Max() < value;

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is greater than <see cref="CreditRatingUS"/> value. Otherwise false.
        /// </summary>
        public static bool operator >(CreditRatingAggregated rating1, CreditRatingUS value)
        {
            var allvals = GetRatingBits(rating1.RatingBig3);
            var min = allvals.Where(r => r != CreditRatingUS.NR).Any()
                ? allvals.Where(r => r != CreditRatingUS.NR).Min()
                : default;
            return min > value;
        }

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is lower than <see cref="CreditRatingRU"/> value. Otherwise false.
        /// </summary>
        public static bool operator <(CreditRatingAggregated rating1, CreditRatingRU value)
            => GetRatingBits(rating1.RatingRu).Max() < value;

        /// <summary>
        ///     Returns true if <see cref="CreditRatingAggregated"/> value is greater than <see cref="CreditRatingRU"/> value. Otherwise false.
        /// </summary>
        public static bool operator >(CreditRatingAggregated rating1, CreditRatingRU value)
        {
            var allvals = GetRatingBits(rating1.RatingRu);
            var min = allvals.Where(r => r != CreditRatingRU.NR).Any()
                ? allvals.Where(r => r != CreditRatingRU.NR).Min()
                : default;
            return min > value;
        }        

        /// <summary>
        ///     Gets collection of bit flags from <see cref="CreditRatingUS"/> or <see cref="CreditRatingRU"/> enumerations.
        /// </summary>
        private static IEnumerable<TRating> GetRatingBits<TRating>(TRating bitmask) where TRating : struct, Enum
        {
            var vals = Enum.GetValues<TRating>();
            foreach (var val in vals) 
                if (bitmask.HasFlag(val)) 
                    yield return val;
        }
    }
}
