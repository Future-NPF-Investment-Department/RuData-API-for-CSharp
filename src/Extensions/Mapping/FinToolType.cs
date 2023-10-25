namespace RuDataAPI.Extensions.Mapping
{
    public enum FinToolType
    {
        None,

        /// <summary>
        ///     Акции.
        /// </summary>
        [EnumFieldStr("Акция")]
        Stock,

        /// <summary>
        ///     Определенный выпуск акций.
        /// </summary>
        [EnumFieldStr("Выпуск акции")]
        StockIssue,

        /// <summary>
        ///     Депозитарные расписки.
        /// </summary>        
        [EnumFieldStr("Депозитарная расписка")] 
        DR,

        /// <summary>
        ///     ИСУ.
        /// </summary>
        [EnumFieldStr("Ипотечный сертификат")] 
        MortgageNote,

        /// <summary>
        ///     Облигация.
        /// </summary>
        [EnumFieldStr("Облигация")] 
        Bond,

        /// <summary>
        ///     Фонд (ПИФ, ETF и пр.)
        /// </summary>
        [EnumFieldStr("Фонд")] 
        Fund
    }
}
