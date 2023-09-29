using Efir.DataHub.Models.Models.Bond;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents coupon period of a bond.
    /// </summary>
    public class SecurityEvent
    {
        public SecurityEvent(TimeTableV2Fields fields)
        {
            StartDate = fields.BeginPeriod;
            EndDate = fields.EndPeriod;
            //EventDate = fields.EventDate ?? default;
            PeriodLength = (int?)fields.EventPeriod ?? 0;
            Rate = (double?)fields.Value ?? 0.0;
            Payment = (double?)fields.Pay1Bond ?? 0.0;
            PaymentType = Enum.Parse<EventType>(fields.EventType);
        }


        /// <summary>
        ///     Event period start date. For CPN events - coupon period start date. For MTY - bond distribution date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Event period end date. For CPN events - coupon period end date. For MTY - bond maturity date.
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        ///     Length of coupon period (for CPN events) or length of bond lifetime (for MTY events) in days.
        /// </summary>        
        public int PeriodLength { get; set; }

        /// <summary>
        ///     Annual interest rate established for the coupon period.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        ///     Payment per bond for this coupon period in units of bond's notional currency.
        /// </summary>
        public double Payment { get; set; }

        /// <summary>
        ///     Payment type (CPN, CALL, MTY, CONV, DIV)
        /// </summary>
        public EventType PaymentType { get; set; }
    }
}
