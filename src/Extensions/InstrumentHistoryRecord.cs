namespace RuDataAPI.Extensions
{
    public class InstrumentHistoryRecord
    {
        /// <summary>
        ///     ISIN-code.
        /// </summary>
        public string Isin { get; set; } = string.Empty;

        /// <summary>
        ///     Trade date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Open price for particular trade date.
        /// </summary>
        public double Open { get; set; }    

        /// <summary>
        ///     Minimal price for particulart trade date.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        ///     Maximal price for particulart trade date.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        ///     Close price for particulart trade date.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        ///     Yield for particulart trade date.
        /// </summary>
        public double Yield { get; set; }

        /// <summary>
        ///     Aggregated volume traded during trade day.
        /// </summary>
        public double VolumeTraded { get; set; }

        /// <summary>
        ///     Aggregated amount traded during trade day.
        /// </summary>
        public long AmountTraded { get; set; }

        /// <summary>
        ///     Number of deals during trade day.
        /// </summary>
        public long NumberOfDeals { get; set; }

        /// <summary>
        ///     Face value for the last trade of the day.
        /// </summary>
        public double FaceValue { get; set; }

        /// <summary>
        ///     Bonds accrued coupon interest.
        /// </summary>
        public double AccruedInterest { get; set; }

        /// <summary>
        ///     Market capitalization (issue size * price).
        /// </summary>
        public double Capitalization { get; set; }

        /// <summary>
        ///     Exhange name.
        /// </summary>
        public string ExchangeName { get; set; } = string.Empty;
    }
}
