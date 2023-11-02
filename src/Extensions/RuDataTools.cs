using Efir.DataHub.Models.Models.Bond;
using Efir.DataHub.Models.Models.Rating;
using RuDataAPI.Extensions.Mapping;
using RuDataAPI.Extensions.Ratings;
using System.Reflection;
using System.Text.RegularExpressions;

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
        internal static TEnum MapToEnum<TEnum>(string strval) where TEnum : struct, Enum
        {
            if (strval is "") return default;

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
        ///     Converts <see cref="RatingsHistoryFields"/> to <see cref="CreditRating"/> with most recent raitings. If ISIN code is specified returns last ratings fot it.
        /// </summary>
        /// <returns>Array of <see cref="CreditRating"/></returns>
        internal static CreditRating[] GetLastRaitings(this RatingsHistoryFields[] rawData, string? isin = null)
        {
            var filtered = isin is not null && rawData.Any(d => d.isin == isin) 
                ? rawData.Where(d => d.isin == isin) 
                : rawData.Where(d => d.rating_object_type == "Компания");

            return filtered
                .Select(f => f.ToCreditRaiting())
                .GroupBy(r => r.Agency)
                .Select(g => g.MaxBy(r => r.Date)!)
                .ToArray();
        }

        /// <summary>
        ///     Aggregates raitings from array of <see cref="CreditRating"/> objects.
        /// </summary>
        /// <returns><see cref="CreditRatingAggregated"/> object.</returns>
        internal static CreditRatingAggregated AggregateRatings(this CreditRating[] ratings)
        {
            const CreditRatingScale NATIONAL = CreditRatingScale.National;

            var filtered = ratings.Any(r => r.Object is CreditRatingTarget.Issuer)
                ? ratings.Where(r => r.Object is CreditRatingTarget.Issuer)
                : ratings;

            CreditRatingUS usr = filtered.Any() ? filtered
                .Select(r => ParseRatingUS(r!.Agency, r!.Value))
                .Max() : default;

            CreditRatingRU rur = filtered.Any(r => r.Scale is NATIONAL) ? filtered
                .Where(r => r.Scale is NATIONAL)
                .Select(r => ParseRatingRU(r!.Agency, r!.Value))
                .Max() : default;

            double pd = usr is not CreditRatingUS.NR
                ? GetDefaultProbality(usr)
                : GetDefaultProbality(rur);

            return new CreditRatingAggregated
            {
                RatingBig3 = usr,
                RatingRu = rur,
                Ratings = ratings,
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
                throw new EfirFieldNullValueException($"Intrument flow has undefined operation type. ISIN: {fields.ISINcode}; FLOWID: {fields.EventID}");

            var flow = new InstrumentFlow()
            {
                Isin            = fields.ISINcode ?? string.Empty,
                StartDate       = fields.BeginEventPer ?? default,
                EndDate         = fields.EventDate ?? default,
                PeriodLength    = fields.EventDate is not null ? (int)fields.EventPeriod! : 0,
                Rate            = fields.Value is not null ? (double)fields.Value : .0,
                Payment         = fields.Pay1Bond is not null ? (double)fields.Pay1Bond : .0,
                PaymentType     = MapToEnum<FlowType>(fields.TypeOperation)
            };
            return flow;
        }

        /// <summary>
        ///     Converts <see cref="RatingsHistoryFields"/> object to <see cref="CreditRating"/> object.
        /// </summary>
        internal static CreditRating ToCreditRaiting(this RatingsHistoryFields fields)
        {
            if (fields.last is null)
                throw new EfirFieldNullValueException($"Raiting value is null. INN: {fields.inn}; Agency: {fields.rating_agency}");

            var rating = new CreditRating
            {
                Value = fields.last,
                Agency = fields.rating_agency,
                Date = fields.last_dt ?? default,
                PreviousValue = fields.prev,
                IssuerName = fields.short_name_org,
                Isin = fields.isin,
                PressRelease = fields.press_release,
                Inn = fields.inn,
                Object = MapToEnum<CreditRatingTarget>(fields.rating_object_type),
                Scale = MapToEnum<CreditRatingScale>(fields.scale_type),
                Currency = MapToEnum<CreditRatingCurrency>(fields.scale_cur),
                Action = MapToEnum<CreditRatingAction>(fields.change),
                Outlook = MapToEnum<CreditRatingOutlook>(fields.forecast ?? string.Empty)
            };
            return rating;
        }

        /// <summary>
        ///     Defines if string is ISIN-code.
        /// </summary>
        /// <param name="code">Code string.</param>
        /// <returns>True if string is ISIN code. Otherwise false.</returns>
        internal static bool IsIsinCode(string code)
        {
            /* ISIN code (according to ISO 6166) consists of:
             *  - 2 alphabetic characters which represents code for the issuing country (ex.: US, XS, RU)
             *  - 9 alpha-numeric characters which identifies the security
             *  - 1 numerical check digit */

            string isinPattern = "([A-Z]{2})([A-Z0-9]{9})([0-9]{1})$";
            return Regex.IsMatch(code, isinPattern);
        }

        /// <summary>
        ///     Adds bussiness days to specified date.
        /// </summary>
        /// <param name="date">Initial date.</param>
        /// <param name="days">Number of days to add. Could be negative.</param>
        /// <param name="holidays">collection of holiday dates.</param>
        internal static DateTime DateAdd(DateTime date, int days, IEnumerable<DateTime> holidays)
        {
            if (days == 0) return date;
            int dt = days < 0 ? -1 : 1;
            int period = Math.Abs(days);
            while (period > 0)            
            {
                date += new TimeSpan(dt, 0, 0, 0);
                if (holidays.Contains(date)) continue;                
                period--;
            }
            return date;
        }
    }
}