namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Coupon period.
    /// </summary>
    public enum CouponPeriodType
    {
        [EnumFieldStr("Не применимо")]
        None,

        [EnumFieldStr("Ежедневный")]
        Daily,

        [EnumFieldStr("Еженедельный")]
        Weekly,

        [EnumFieldStr("Ежемесячный")]
        Monthly,

        [EnumFieldStr("Квартальный")]
        Quarterly,

        [EnumFieldStr("Полугодовой")]
        Semiannual,

        [EnumFieldStr("Ежегодный")]
        Annual,

        [EnumFieldStr("Раз в два года")]
        OncePerTwoYears,

        [EnumFieldStr("Иной")]
        Nonstandard
    }
}
