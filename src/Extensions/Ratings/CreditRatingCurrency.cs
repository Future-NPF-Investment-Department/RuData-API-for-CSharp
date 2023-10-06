using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Cretid rating scale currency used by rating agency.
    /// </summary>
    public enum CreditRatingCurrency
    {
        [EnumFieldStr("Локальная")]
        Local,

        [EnumFieldStr("Иностранная")]
        Foreign
    }
}
