namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents financial intruments classification.
    /// </summary>
    public enum InstrumentType
    {
        UNDEFINED,

        /// <summary>
        ///     Акции.
        /// </summary>
        [EnumFieldStr("Акция", "Выпуск акции")]
        STOCK,

        /// <summary>
        ///     Депозитарные расписки.
        /// </summary>        
        [EnumFieldStr("Депозитарная расписка")] 
        RECEIPT,

        /// <summary>
        ///     ИСУ.
        /// </summary>
        [EnumFieldStr("Ипотечный сертификат")] 
        MPC,

        /// <summary>
        ///     Облигация.
        /// </summary>
        [EnumFieldStr("Облигация")] 
        BOND,

        /// <summary>
        ///     Фонд (ПИФ, ETF и пр.)
        /// </summary>
        [EnumFieldStr("Фонд")] 
        FUND
    }
}
