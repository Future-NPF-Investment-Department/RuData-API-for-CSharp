using Efir.DataHub.Models.Models.Bond; 

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents coupon period of a bond.
    /// </summary>
    public class CouponPeriod
    {
        public CouponPeriod(CouponsFields fields)
        {
            CouponId            = fields.id_coupon;
            CouponStart         = fields.begin_period!.Value;
            CouponEnd           = fields.end_period!.Value;
            CouponLength        = fields.coupon_period!.Value;
            CouponRate          = (double?)fields.coupon_rate ?? default;
            Spread              = (double?)fields.rate_spread_pct ?? default;
            CouponPayment       = (double?)fields.pay_per_bond ?? default;
        }

        /// <summary>
        ///     Coupon ID.
        /// </summary>
        public long CouponId { get; set; }

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
        public long CouponLength { get; set; }

        /// <summary>
        ///     Annual interest rate established for the coupon period.
        /// </summary>
        public double CouponRate { get; set; }

        /// <summary>
        ///     Difference between Coupon rate and reference floating rate.
        /// </summary>
        public double Spread { get; set; }

        /// <summary>
        ///     Payment per bond for this coupon period in units of bond's notional currency.
        /// </summary>
        public double CouponPayment { get; set; }
    }
}
