namespace RuDataAPI.Extensions.Mapping
{
    public enum FinToolType
    {
        /// <summary>
        ///     Акции.
        /// </summary>
        Stock,
        /// <summary>
        ///     Определенный выпуск акций.
        /// </summary>
        StockIssue,
        /// <summary>
        ///     Депозитарные расписки.
        /// </summary>
        DR,
        /// <summary>
        ///     ИСУ.
        /// </summary>
        MortgageNote,
        /// <summary>
        ///     Облигация.
        /// </summary>
        Bond,
        /// <summary>
        ///     Фонд (ПИФ, ETF и пр.)
        /// </summary>
        Fund
    }
}
