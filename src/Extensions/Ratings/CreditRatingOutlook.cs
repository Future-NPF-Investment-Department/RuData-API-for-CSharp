using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents rating oultook (forecast) of emitent or security.
    /// </summary>
    public enum CreditRatingOutlook
    {
        [EnumFieldStr(null)]
        None,

        [EnumFieldStr("Позитивный")]
        Positive,

        [EnumFieldStr("Негативный")]
        Negative,

        [EnumFieldStr("Стабильный")]
        Stable,

        [EnumFieldStr("Неопределенный")]
        Uncertain,

        [EnumFieldStr("на пересмотре")]
        Revising,

        [EnumFieldStr("Развивающийся")]
        Evolving,
    }
}
