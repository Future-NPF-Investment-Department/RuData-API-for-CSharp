namespace RuDataAPI.Extensions
{
    public class InstrumentHistoryRecord
    {
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
        ///     Aggregated volume traded during trade day.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        ///     Face value for the last trade of the day.
        /// </summary>
        public double FaceValue { get; set; }

        /// <summary>
        ///     Bonds accrued coupon interest.
        /// </summary>
        public double AccruedInterest { get; set; }

        /// <summary>
        ///     ISIN-code.
        /// </summary>
        public string Isin { get; set; } = string.Empty;

        /// <summary>
        ///     Exhange name.
        /// </summary>
        public string ExchangeName { get; set; } = string.Empty;
    }
}
