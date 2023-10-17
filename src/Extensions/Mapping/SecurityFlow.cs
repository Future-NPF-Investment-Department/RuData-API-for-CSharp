namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Security flow type.
    /// </summary>
    public enum SecurityFlow
    {
        /// <summary>
        ///     Coupon payment.
        /// </summary>
        [EnumFieldStr("Дисконт", "Плавающий", "Постоянный", "Фиксированный", "Переменный", "Прочий", "Ипотечный")]
        CPN,

        /// <summary>
        ///     Call-offer payment.
        /// </summary>
        [EnumFieldStr("C", "CA", "CM")]
        CALL,

        /// <summary>
        ///     Put-offer payment.
        /// </summary>
        [EnumFieldStr("O", "P")]
        PUT,

        /// <summary>
        ///     Notional payment.
        /// </summary>
        [EnumFieldStr("OM")] 
        MTY,

        /// <summary>
        ///     Notional amortization.
        /// </summary>
        [EnumFieldStr("OA")]
        AMRT,

        /// <summary>
        ///     Conversion payment.
        /// </summary>
        [EnumFieldStr("CC", "CP", "PC", "PP", "SB")]
        CONV,

        /// <summary>
        ///     Dividend payment.
        /// </summary>
        [EnumFieldStr("EM", "AM")]
        DIV,
    }
}
