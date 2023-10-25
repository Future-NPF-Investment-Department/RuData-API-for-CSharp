namespace RuDataAPI.Extensions.Mapping
{
    public enum CouponBasis
    {
        /// <summary>
        ///     Not applicable.
        /// </summary>
        [EnumFieldStr("Не применимо")]
        None,

        /// <summary>
        ///     Купонные платежи всегда одинаковые.
        /// </summary>
        [EnumFieldStr("act/act")]
        ACTACT,

        /// <summary>
        ///     (Кол-во дней попавших на високосный год) / 366 + (Кол-во дней приходящихся на не високосный год) / 365.
        /// </summary>
        [EnumFieldStr("act/act ISDA")]
        ACTACTISDA,

        /// <summary>
        ///     Фактическое количество дней в расчетном периоде (без поправок) делится на 360.
        /// </summary>
        [EnumFieldStr("act/360")]
        ACT360,

        /// <summary>
        ///     Фактическое количество дней в расчетном периоде (без поправок) делится на 365.
        /// </summary>
        [EnumFieldStr("act/365")]
        ACT365,

        /// <summary>
        ///     366, если дата окончания периода попадает в високосный год, иначе делим на 365.
        /// </summary>
        [EnumFieldStr("act/365L")]
        ACT365L,

        /// <summary>
        ///     Фактическое количество дней в расчетном периоде (без поправок) делится на 366.
        /// </summary>
        [EnumFieldStr("act/366")]
        ACT366,

        /// <summary>
        ///     Вычитается 1, если 29 февраля попадает в период. Базис расчета НКД = 365.
        /// </summary>
        [EnumFieldStr("NL/365")]
        NL365,

        /// <summary>
        ///     Определяется как (Y2 - Y1) * 360 + (M2 - M1) * 30 + (D2 - D1). 
        /// </summary>
        /// <remarks>
        ///     <para>Если D1 = 31, то D1 = 30</para> 
        ///     <para>Если D2 = 31, то D2 = 30</para>
        /// </remarks>
        [EnumFieldStr("30E/360")]
        _30E360,

        /// <summary>
        ///     Определяется как (Y2 - Y1) * 360 + (M2 - M1) * 30 + (D2 - D1).    
        /// </summary>
        /// <remarks>
        ///     <para>Если D1 = 31, то D1 = 30.</para> 
        ///     <para>Если D2 = 31 и D1 > 29, то D2 = 30</para>
        /// </remarks>
        [EnumFieldStr("30/360")]
        _30360,

        /// <summary>
        ///     Если D1=31, то D1=30; если D2=31, то D2=30; если D1-посл. день фев., то D1=30, если D2 - посл. Д
        /// </summary>
        [EnumFieldStr("30/360 German")]
        _30360German
    }
}
