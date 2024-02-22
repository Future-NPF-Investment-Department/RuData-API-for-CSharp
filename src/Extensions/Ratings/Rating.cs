using RuDataAPI.Extensions.Mapping;
using System.Reflection;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Provides tools for credit ratings manipulations.
    /// </summary>
    public static class Rating
    {
        /// <summary>
        ///     Parses string to generic BIG-3 credit rating.
        /// </summary>
        /// <param name="rating">Generic rating string representation.</param>
        /// <returns><see cref="CreditRatingRU"/> value that represents credit rating in generic BIG-3 credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        public static CreditRatingUS ParseRatingUS(string rating)
        {
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                if (attr is not null && attr.Name == rating)
                    return Enum.Parse<CreditRatingUS>(field.Name);
            }
            throw new Exception($"Cannot recognize BIG-3 rating '{rating}'.");
        }

        /// <summary>
        ///     Parses string pair of agency and its rating to generic BIG3 credit rating scale.
        /// </summary>
        /// <param name="agency">Rating agency name.</param>
        /// <param name="rating">Rating agency's rating.</param>
        /// <returns><see cref="CreditRatingUS"/> value that represents credit rating from generic RU credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        public static CreditRatingUS ParseRatingUS(string rating, RatingAgency agency)
        {
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attrs = field.GetCustomAttributes<AgencyRatingBucketAttribute>();
                foreach (var attr in attrs)
                    if (attr.ContainsAgencyRating(agency, rating))
                        return Enum.Parse<CreditRatingUS>(field.Name);
            }
            throw new Exception($"Cannot parse '{rating}' credit rating (US) of {agency}.");
        }

        /// <summary>
        ///     Parses string to generic RU credit rating.
        /// </summary>
        /// <param name="rating">Generic rating string representation.</param>
        /// <returns><see cref="CreditRatingRU"/> value that represents credit rating in generic RU credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        public static CreditRatingRU ParseRatingRU(string rating)
        {
            Type enumType = typeof(CreditRatingRU);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                if (attr is not null && attr.Name == rating)
                    return Enum.Parse<CreditRatingRU>(field.Name);
            }
            throw new Exception($"Cannot recognize RU rating '{rating}'.");
        }

        /// <summary>
        ///     Parses string pair of agency and its rating to generic RU credit rating scale.
        /// </summary>
        /// <param name="agency">Rating agency name.</param>
        /// <param name="rating">Rating agency's rating.</param>
        /// <returns><see cref="CreditRatingRU"/> value that represents credit rating from generic RU credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        public static CreditRatingRU ParseRatingRU(string rating, RatingAgency agency)
        {
            Type enumType = typeof(CreditRatingRU);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attrs = field.GetCustomAttributes<AgencyRatingBucketAttribute>();
                foreach (var attr in attrs)
                    if (attr.ContainsAgencyRating(agency, rating))
                        return Enum.Parse<CreditRatingRU>(field.Name);
            }
            return CreditRatingRU.NR;
        }

        /// <summary>
        ///     Returns string representation of <see cref="CreditRatingUS"/> value.
        /// </summary>
        public static string ConvertToString(CreditRatingUS rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                if (attr is not null) return attr.Name;
            }
            throw new Exception($"Cannot parse '{rating}' (Big 3) to appropriate string.");
        }

        /// <summary>
        ///     Returns string representation of <see cref="CreditRatingRU"/> value.
        /// </summary>
        public static string ConvertToString(CreditRatingRU rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                if (attr is not null) return attr.Name;
            }
            throw new Exception($"Cannot parse '{rating}' (RU) to appropriate string.");
        }

        /// <summary>
        ///     Extracts rating collection for specified value of all possible scales.
        /// </summary>
        public static string[] ExtractUnderlyingRatings(CreditRatingUS rating)
        {
            Type enumType = typeof(CreditRatingUS);
            FieldInfo? field = enumType.GetField(rating.ToString());
            var attrs = field!.GetCustomAttributes<AgencyRatingBucketAttribute>();
            List<string[]> buckets = new();
            foreach (var attr in attrs)
                buckets.Add(attr.GetBucket());
            return buckets.SelectMany(a => a).ToArray();
        }

        /// <summary>
        ///     Extracts rating collection for specified value of all possible scales.
        /// </summary>
        public static string[] ExtractUnderlyingRatings(CreditRatingRU rating)
        {
            Type enumType = typeof(CreditRatingRU);
            FieldInfo? field = enumType.GetField(rating.ToString());
            var attrs = field!.GetCustomAttributes<AgencyRatingBucketAttribute>();
            List<string[]> buckets = new();
            foreach (var attr in attrs)
                buckets.Add(attr.GetBucket());
            return buckets.SelectMany(a => a).ToArray();
        }

        /// <summary>
        ///     Get all ratings in scale beween specified rating levels.
        /// </summary>
        /// <param name="low">Low rating bound. If null then low bound is not specified.</param>
        /// <param name="high">High rating bound. If null then high bound is not specified.</param>
        /// <returns>Collection of <see cref="CreditRatingUS"/></returns>
        public static CreditRatingUS[] GetRatingRange(CreditRatingUS? low, CreditRatingUS? high)
        {
            var range = Enum.GetValues<CreditRatingUS>().AsEnumerable();
            if (high < low) return Array.Empty<CreditRatingUS>();

            range = low is not null
                ? range.Where(crus => crus >= low)
                : range;

            range = high is not null
                ? range.Where(crus => crus <= high)
                : range;

            return range.ToArray();
        }

        /// <summary>
        ///     Get all ratings in scale beween specified rating levels.
        /// </summary>
        /// <param name="low">Low rating bound. If null then low bound is not specified.</param>
        /// <param name="high">High rating bound. If null then high bound is not specified.</param>
        /// <returns>Collection of <see cref="CreditRatingRU"/></returns>
        public static CreditRatingRU[] GetRatingRange(CreditRatingRU? low, CreditRatingRU? high)
        {
            var range = Enum.GetValues<CreditRatingRU>().AsEnumerable();
            if (high < low) return Array.Empty<CreditRatingRU>();

            range = low is not null
                ? range.Where(crus => crus >= low)
                : range;

            range = high is not null
                ? range.Where(crus => crus <= high)
                : range;

            return range.ToArray();
        }

        /// <summary>
        ///     Obtains default of probility implied by specified aggregated rating in Big3 scale.
        /// </summary>
        /// <param name="rating">Rating aggregated.</param>
        /// <returns>Probability of default.</returns>
        public static double GetDefaultProbality(CreditRatingUS rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            double pd = default;
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                pd = (attr is not null) ? attr.PD : double.NaN;
            }
            return pd;
        }

        /// <summary>
        ///     Obtains default of probility implied by specified aggregated rating in Big3 scale.
        /// </summary>
        /// <param name="rating">Rating aggregated.</param>
        /// <returns>Probability of default.</returns>
        public static double GetDefaultProbality(CreditRatingRU rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingRU);
            FieldInfo[] fields = enumType.GetFields();
            double pd = default;
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                pd = (attr is not null) ? attr.PD : double.NaN;
            }
            return pd;
        }
    }
}
