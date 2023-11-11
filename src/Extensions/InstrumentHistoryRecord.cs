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
        ///     Yield for the last trade of the day.
        /// </summary>
        public double Yield { get; set; }
    }
}
