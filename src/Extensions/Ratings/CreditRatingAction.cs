using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents credit rating information for particular issuer or security.
    /// </summary>
    public class CreditRatingAction
    {
        public static readonly CreditRatingAction Default = new();

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
        public RatingAction Action { get; set; } 

        /// <summary>
        ///     Credit rating outlook (forecast).
        /// </summary>
        public CreditRatingOutlook Outlook { get; set; }

        /// <summary>
        ///     Particular credit rating.
        /// </summary>
        public string Value { get; set; } = "NR";

        /// <summary>
        ///     Previous credit rating.
        /// </summary>
        public string PreviousValue { get; set; } = string.Empty;

        /// <summary>
        ///     Rating agency name.
        /// </summary>
        public RatingAgency Agency { get; set; }

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
        ///     Provides extended stylized rating description.
        /// </summary>
        public override string ToString()
        {
            const string ACNT = "\x1b[38;5;210m";       // color accent 
            const string RST = "\x1b[0m";               // normal color accent
            const string UNDSCR = "\x1b[4m";            // underscore accent

            string head = $"{UNDSCR}RATING for {IssuerName} (INN: {ACNT}{Inn}{RST})\n";
            string rating = $"{ACNT}{Value}{RST} from {Agency} ({Date.ToShortDateString()}, {Action})\n";
            string scale = $"Scale: {Scale} in {Currency} currency\n";
            string action = $"Outlook: {Outlook}\n";
            string release = $"Press release: {PressRelease}\n";
            return head + rating + scale + action + release;
        }

        /// <summary>
        ///     Provides short stylized rating description.
        /// </summary>
        public string ToShortString()
        {
            const string ACNT = "\x1b[38;5;210m";              // color accent 
            const string RST = "\x1b[0m";                      // normal color accent
            return $"{RuDataTools.GetEnumPrintStrig(Agency)}:\t {ACNT}{(Value is "Снят" ? "NR" : Value)}{RST} ({Action} {Date.ToShortDateString()} | {Outlook})";
        }
    }
}
