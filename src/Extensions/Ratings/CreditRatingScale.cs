using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Credit rating scale type. National rating scale used by national rating agencies,
    ///     international rating scalse used by foreign rating agencies.
    /// </summary>
    public enum CreditRatingScale
    {
        [EnumFieldStr("Национальная")]
        National,

        [EnumFieldStr("Международная")]
        International
    }
}
