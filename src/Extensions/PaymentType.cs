namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Payment type.
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        ///     Coupon payment.
        /// </summary>
        CPN, 

        /// <summary>
        ///     Offer payment.
        /// </summary>
        CALL, 

        /// <summary>
        ///     Notional payment.
        /// </summary>
        MTY, 

        /// <summary>
        ///     Conversion payment.
        /// </summary>
        CONV, 

        /// <summary>
        ///     Dividend payment.
        /// </summary>
        DIV
    }
}
