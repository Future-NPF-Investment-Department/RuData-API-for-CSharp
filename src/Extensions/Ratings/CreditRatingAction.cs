using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents a rating action carried out by a rating agency in respect of emitent or security credit rating.
    /// </summary>
    public enum CreditRatingAction
    {
        [EnumFieldStr("Установлен")]
        Assigned,

        [EnumFieldStr("Повышен")]
        Upgraded,

        [EnumFieldStr("Понижен")]
        Downgraded,

        [EnumFieldStr("Подтвержден")]
        Affirmed,

        [EnumFieldStr("снят")]
        Withdrawed,

        [EnumFieldStr("Не изменился")]
        Unchanged,

        [EnumFieldStr("Приостановлен")]
        Stopped
    }
}
