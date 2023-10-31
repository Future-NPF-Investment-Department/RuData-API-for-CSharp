using Efir.DataHub.Models.Models.Bond;
using RuDataAPI.Extensions.Mapping;
using RuDataAPI.Extensions.Ratings;
using System.Reflection;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Auxiliary tools.
    /// </summary>
    internal static class RuDataTools
    {

        /// <summary>
        ///     Maps string value to specified enum field using attributes specified for this field.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="strval">value to parse.</param>
        /// <returns>Field of specified enum.</returns>
        internal static TEnum MapToEnum<TEnum>(string? strval) where TEnum : struct, Enum
        {
            if (strval is null) return default;

            Type enumType = typeof(TEnum);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attr = field.GetCustomAttribute<EnumFieldStrAttribute>();
                if (attr is not null && attr.Values.Contains(strval))
                    return Enum.Parse<TEnum>(field.Name);
            }
            throw new Exception($"Cannot map '{strval}' to {enumType.Name}.");
        }

        /// <summary>
        ///     Parses string pair of agency and its rating to generic RU credit rating scale.
        /// </summary>
        /// <param name="agency">Rating agency name.</param>
        /// <param name="rating">Rating agency's rating.</param>
        /// <returns><see cref="CreditRatingRU"/> value that represents credit rating from generic RU credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        internal static CreditRatingRU ParseRatingRU(string agency, string rating)
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
        ///     Parses string pair of agency and its rating to generic BIG3 credit rating scale.
        /// </summary>
        /// <param name="agency">Rating agency name.</param>
        /// <param name="rating">Rating agency's rating.</param>
        /// <returns><see cref="CreditRatingUS"/> value that represents credit rating from generic RU credit scale.</returns>
        /// <exception cref="Exception">is thrown if parse operation is unavailable.</exception>
        internal static CreditRatingUS ParseRatingUS(string agency, string rating)
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
        ///     Obtains default of probility implied by specified aggregated rating in Big3 scale.
        /// </summary>
        /// <param name="rating">Rating aggregated.</param>
        /// <returns>Probability of default.</returns>
        internal static double GetDefaultProbality(CreditRatingUS rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingUS);
            FieldInfo[] fields = enumType.GetFields();
            double pd = default;
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                pd = (attr is not null) ? attr.PD : default;
            }
            return pd;
        }

        /// <summary>
        ///     Obtains default of probility implied by specified aggregated rating in Big3 scale.
        /// </summary>
        /// <param name="rating">Rating aggregated.</param>
        /// <returns>Probability of default.</returns>
        internal static double GetDefaultProbality(CreditRatingRU rating)
        {
            string fieldName = rating.ToString();
            Type enumType = typeof(CreditRatingRU);
            FieldInfo[] fields = enumType.GetFields();
            double pd = default;
            foreach (FieldInfo field in fields)
            {
                if (field.Name != fieldName) continue;
                var attr = field.GetCustomAttribute<GenericRatingAttribute>();
                pd = (attr is not null) ? attr.PD : default;
            }
            return pd;
        }

        /// <summary>
        ///     Aggregates raitings.
        /// </summary>
        /// <returns><see cref="CreditRatingAggregated"/> object.</returns>
        internal static CreditRatingAggregated CreateAggregatedRating(CreditRating[] ratings)
        {
            const CreditRatingScale NATIONAL = CreditRatingScale.National;

            //string issuer = ratings[0].IssuerName;

            var filtered = ratings.Any(r => r.Object == CreditRatingTarget.Issuer)
                ? ratings.Where(r => r.Object == CreditRatingTarget.Issuer)
                : ratings;

            var lastRatings = filtered
                .GroupBy(r => r.Agency)
                .Select(g => g.MaxBy(r => r.Date)!)
                .ToList();

            CreditRatingUS usr = filtered.Any() ? filtered
                .GroupBy(r => r.Agency)
                .Select(g => g.MaxBy(r => r.Date))
                .Select(r => ParseRatingUS(r!.Agency, r!.Value))
                .Max() : default;

            CreditRatingRU rur = filtered.Any(r => r.Scale is NATIONAL) ? filtered
                .Where(r => r.Scale is NATIONAL)
                .GroupBy(r => r.Agency)
                .Select(g => g.MaxBy(r => r.Date))
                .Select(r => ParseRatingRU(r!.Agency, r!.Value))
                .Max() : default;

            double pd = usr is not CreditRatingUS.NR
                ? GetDefaultProbality(usr)
                : GetDefaultProbality(rur);

            return new CreditRatingAggregated
            {
                RatingBig3 = usr,
                RatingRu = rur,
                Ratings = lastRatings,
                DefaultProbability = pd,
            };
        }

        /// <summary>
        ///     Returns string representation of <see cref="CreditRatingUS"/> value.
        /// </summary>
        internal static string ToRatingString(this CreditRatingUS rating)
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
        internal static string ToRatingString(this CreditRatingRU rating)
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
        ///     Converts <see cref="TimeTableV2Fields"/> object to <see cref="InstrumentFlow"/> object.
        /// </summary>
        internal static InstrumentFlow ToFlow(this TimeTableV2Fields fields)
        {
            if (fields.TypeOperation is null)
                throw new Exception($"Intrument flow error: undefined operation type. ISIN: {fields.ISINcode}; FLOWID: {fields.EventID}");

            var flow = new InstrumentFlow()
            {
                Isin            = fields.ISINcode ?? string.Empty,
                StartDate       = fields.BeginEventPer ?? default,
                EndDate         = fields.EventDate ?? default,
                PeriodLength    = fields.EventDate is not null ? (int)fields.EventPeriod! : 0,
                Rate            = fields.Value is not null ? (double)fields.Value : .0,
                Payment         = fields.Pay1Bond is not null ? (double)fields.Pay1Bond : .0,
                PaymentType     = MapToEnum<SecurityFlow>(fields.TypeOperation)
            };
            return flow;
        }
    }
}