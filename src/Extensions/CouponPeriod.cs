using Efir.DataHub.Models.Models.Bond;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents coupon period of a bond.
    /// </summary>
    public class CouponPeriod
    {
        public CouponPeriod(TimeTableV2Fields fields)
        {
            CouponStart = fields.BeginPeriod!.Value;
            CouponEnd = fields.EndPeriod!.Value;
            CouponLength = (int)fields.CouponPeriod!.Value;
            Rate = (double?)fields.Value ?? 0.0;
            Payment = (double?)fields.Pay1Bond ?? 0.0;
            PaymentType = Enum.Parse<PaymentType>(fields.EventType);
        }


        /// <summary>
        ///     Start date of coupon period.
        /// </summary>
        public DateTime CouponStart { get; set; }   
        
        /// <summary>
        ///     End date of coupon period.
        /// </summary>
        public DateTime CouponEnd { get; set; }
        
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
        public PaymentType PaymentType { get; set; }
    }
}
