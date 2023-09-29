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
            CouponStart = fields.BeginPeriod;
            CouponEnd = fields.EndPeriod;
            EventDate = fields.EventDate ?? default;
            CouponLength = (int)fields.CouponPeriod!.Value;
            Rate = (double?)fields.Value ?? 0.0;
            Payment = (double?)fields.Pay1Bond ?? 0.0;
            PaymentType = Enum.Parse<EventType>(fields.EventType);
        }


        /// <summary>
        ///     Start date of coupon period.
        /// </summary>
        public DateTime? CouponStart { get; set; }   
        
        /// <summary>
        ///     End date of coupon period.
        /// </summary>
        public DateTime? CouponEnd { get; set; }

        /// <summary>
        ///     Date of event.
        /// </summary>
        public DateTime EventDate { get; set; }
        
        /// <summary>
        ///     Length of coupon period in days.
        /// </summary>        
        public int CouponLength { get; set; }

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
