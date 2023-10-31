using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents instrument particular flow.
    /// </summary>
    public class InstrumentFlow
    {
        /// <summary>
        ///     Security ISIN-code.
        /// </summary>
        public string Isin { get; init; } = string.Empty;

        /// <summary>
        ///     Event period start date. For CPN events - coupon period start date. For MTY - bond distribution date.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        ///     Event period end date. For CPN events - coupon period end date. For MTY - bond maturity date.
        /// </summary>
        public DateTime EndDate { get; init; }
        
        /// <summary>
        ///     Length of coupon period (for CPN events) or length of bond lifetime (for MTY events) in days.
        /// </summary>        
        public int PeriodLength { get; init; }

        /// <summary>
        ///     Annual interest rate established for the coupon period.
        /// </summary>
        public double Rate { get; init; }

        /// <summary>
        ///     Payment per bond for this coupon period in units of bond's notional currency.
        /// </summary>
        public double Payment { get; init; }

        /// <summary>
        ///     Payment type (CPN, CALL, MTY, CONV, DIV)
        /// </summary>
        public FlowType PaymentType { get; init; }
    }
}
