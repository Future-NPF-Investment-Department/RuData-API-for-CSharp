#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable IDE0017 // Simplify object initialization

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
        ///     International aggregated credit rating value. Based on BIG3 (FITCH, SnP, Moodys) rating scale.
        /// </summary>
        public CreditRatingUS AggregatedBig3 { get; set; }

        /// <summary>
        ///     National (RU) aggregated credit rating value.
        /// </summary>
        public CreditRatingRU AggregatedRu { get; set; }

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
        public static CreditRating New(RatingsHistoryFields fields)
        {
            CreditRating rating = new();
            rating.Value = fields.last;
            rating.Agency = fields.rating_agency;
            rating.Date = fields.last_dt ?? default;
            rating.PreviousValue = fields.prev;
            rating.IssuerName = fields.short_name_org;
            rating.Isin = fields.isin;
            rating.PressRelease = fields.press_release;
            rating.Object = RuDataTools.MapToEnum<CreditRatingTarget>(fields.rating_object_type);
            rating.Scale = RuDataTools.MapToEnum<CreditRatingScale>(fields.scale_type);
            rating.Currency = RuDataTools.MapToEnum<CreditRatingCurrency>(fields.scale_cur);
            rating.Action = RuDataTools.MapToEnum<CreditRatingAction>(fields.change);
            rating.Outlook = RuDataTools.MapToEnum<CreditRatingOutlook>(fields.forecast);
            rating.AggregatedBig3 = RuDataTools.ParseRatingUS(rating.Agency, rating.Value);
            rating.AggregatedRu = RuDataTools.ParseRatingRU(rating.Agency, rating.Value);
            rating.DefaultProbability = RuDataTools.GetDefaultProbality(rating.AggregatedBig3);
            return rating;
        }

        public override string ToString()
        {
            string head = $"RATING for {IssuerName} ({Isin})\n";
            string rating = $"{Value} from {Agency} ({Date.ToShortDateString()}, {Action})\n";
            string scale = $"Scale: {Scale} in {Currency} currency\n";
            string action = $"Outlook: {Outlook}\n";
            string aggrUS = $"Aggregated Big3: {AggregatedBig3.ToRatingString()}\n";
            string aggrRU = $"Aggregated RU: {AggregatedRu.ToRatingString()}\n";
            string release = $"Press release: {PressRelease}\n";
            string pd = $"Probability of default: {DefaultProbability:0.00%}\n";
            return head + rating + scale + action + aggrUS + aggrRU + release + pd;
        }

        public string ToShortStringUS()
            => $"{AggregatedBig3.ToRatingString()} from {Agency} as of {Date.ToShortDateString()} ({Action})";
        

        public string ToShortStringRU()
            => $"{AggregatedRu.ToRatingString()} from {Agency} as of {Date.ToShortDateString()} ({Action})";        


        public static bool operator <=(CreditRating rating1, CreditRating rating2)
            => rating1.AggregatedBig3 <= rating2.AggregatedBig3;

        public static bool operator >=(CreditRating rating1, CreditRating rating2)
            => rating1.AggregatedBig3 >= rating2.AggregatedBig3;

        public static bool operator <=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 <= value;

        public static bool operator >=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 >= value;

        public static bool operator ==(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 == value;

        public static bool operator !=(CreditRating rating1, CreditRatingUS value)
            => rating1.AggregatedBig3 != value;
    }
}
