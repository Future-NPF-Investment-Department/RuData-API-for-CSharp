
using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents object to which credit rating action applied.
    /// </summary>
    public enum CreditRatingTarget
    {
        /// <summary>
        ///     Credit rating action applied to issuer.
        /// </summary>
        [EnumFieldStr("Компания")]
        Issuer,

        /// <summary>
        ///     Credit rating action applied to issuer's security.
        /// </summary>
        [EnumFieldStr("Инструмент")]
        Security
    }
}
