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
        internal static TEnum MapToEnum<TEnum>(string strval) where TEnum : struct, Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attr = field.GetCustomAttribute<EnumFieldStrAttribute>();
                if (attr is not null && attr.Value == strval)
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
            throw new Exception($"Cannot parse '{rating}' credit rating (RU) of {agency}.");
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
        ///     Obtains default of probility implied by specified aggregated rating.
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
            return default;
        }
    }
}