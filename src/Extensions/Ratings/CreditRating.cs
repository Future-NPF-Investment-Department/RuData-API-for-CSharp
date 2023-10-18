//#pragma warning disable IDE0017 // Simplify object initialization

using Efir.DataHub.Models.Models.Rating;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents credit rating information for particular issuer or security.
    /// </summary>
    public class CreditRating
    {
        public static readonly CreditRating Default = new();

        /// <summary>
        ///     Date of credit rating action.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Object to which credit rating action applied.
        /// </summary>`
        public CreditRatingTarget Object { get; set; }

        /// <summary>
        ///     Credit rating scale type.
        /// </summary>
        public CreditRatingScale Scale { get; set; }

        /// <summary>
        ///     Credit rating scale currency.
        /// </summary>
        public CreditRatingCurrency Currency { get; set; }

        /// <summary>
        ///     Credit rating action type.
        /// </summary>
        public CreditRatingAction Action { get; set; } 

        /// <summary>
        ///     Credit rating outlook (forecast).
        /// </summary>
        public CreditRatingOutlook Outlook { get; set; }

        /// <summary>
        ///     Particular credit rating.
        /// </summary>
        public string Value { get; set; } = null!;

        /// <summary>
        ///     Previous credit rating.
        /// </summary>
        public string PreviousValue { get; set; } = string.Empty;

        /// <summary>
        ///     Rating agency name.
        /// </summary>
        public string Agency { get; set; } = null!;

        /// <summary>
        ///     Probability of default (PD) that corresponds to <see cref="AggregatedBig3"/> rating.
        /// </summary>
        public double DefaultProbability { get; set; }

        /// <summary>
        ///     Name of issuer that is subject for credit rating action.
        /// </summary>
        public string IssuerName { get; set; } = string.Empty;

        /// <summary>
        ///     Issuer's INN code.
        /// </summary>
        public string Inn { get; set; } = null!;

        /// <summary>
        ///     Issuer's ISIN that is subject for credit rating action.
        /// </summary>
        public string Isin { get; set; } = string.Empty;

        /// <summary>
        ///     Reference to agency press-release regarding this credit rating action.
        /// </summary>
        public string PressRelease { get; set; } = null!;

        /// <summary>
        ///     Creates new rating
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static CreditRating ConvertFromEfirRatingsFields(RatingsHistoryFields fields)
        {
            return new()
            {
                Value = fields.last,
                Agency = fields.rating_agency,
                Date = fields.last_dt ?? default,
                PreviousValue = fields.prev,
                IssuerName = fields.short_name_org,
                Isin = fields.isin,
                PressRelease = fields.press_release,
                Inn = fields.inn,
                Object = RuDataTools.MapToEnum<CreditRatingTarget>(fields.rating_object_type),
                Scale = RuDataTools.MapToEnum<CreditRatingScale>(fields.scale_type),
                Currency = RuDataTools.MapToEnum<CreditRatingCurrency>(fields.scale_cur),
                Action = RuDataTools.MapToEnum<CreditRatingAction>(fields.change),
                Outlook = RuDataTools.MapToEnum<CreditRatingOutlook>(fields.forecast)
            };
        }

        /// <summary>
        ///     Provides extended stylized rating description.
        /// </summary>
        public override string ToString()
        {
            string head = $"RATING for {IssuerName} (ISIN: {Isin})\n";
            string rating = $"{Value} from {Agency} ({Date.ToShortDateString()}, {Action})\n";
            string scale = $"Scale: {Scale} in {Currency} currency\n";
            string action = $"Outlook: {Outlook}\n";
            string release = $"Press release: {PressRelease}\n";
            string pd = $"Probability of default: {DefaultProbability:0.00%}\n";
            return head + rating + scale + action + release + pd;
        }

        /// <summary>
        ///     Provides short stylized rating description.
        /// </summary>
        public string ToShortString()
            => $"{Date.ToShortDateString()} {(Value is "Снят" ? "NR" : Value)} by {Agency} ({Action})";        
    }
}
