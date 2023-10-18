using RuDataAPI.Extensions.Ratings;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents parameters that are used by EFIR server for securities search.
    /// </summary>
    public class EfirSecQueryDetails
    {
        /// <summary>
        ///     Min issue's distribution date.
        /// </summary>
        public DateTime? DistributionStart { get; set; }

        /// <summary>
        ///     Max issue's distribution date.
        /// </summary>
        public DateTime? DistributionEnd { get; set; }

        /// <summary>
        ///     Min issue's maturity date.
        /// </summary>
        public DateTime? MaturityStart { get; set; }

        /// <summary>
        ///     Max Issue's maturity date.
        /// </summary>
        public DateTime? MaturityEnd { get; set; }

        /// <summary>
        ///     International rating lower limit.
        /// </summary>
        public CreditRatingUS? Big3RatingLow { get; set; }

        /// <summary>
        ///     International rating upper limit.
        /// </summary>
        public CreditRatingUS? Big3RatingHigh { get; set; }

        /// <summary>
        ///     National rating lower limit.
        /// </summary>
        public CreditRatingRU? RuRatingLow { get; set; }

        /// <summary>
        ///     National rating upper limit.
        /// </summary>
        public CreditRatingRU? RuRatingHigh { get; set; }

        /// <summary>
        ///     Issuer's sector.
        /// </summary>
        public string[]? Sectors { get; set; }

        /// <summary>
        ///     Issue currency.
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        ///     Issuer country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        ///     Generates query string used to filter out securities that satisfy existing parameters of <see cref="EfirSecQueryDetails"/>.
        /// </summary>
        /// <returns>Qurey string in SQL WHERE expression style.</returns>
        public override string ToString()
        {
            string diststart = DistributionStart is not null
                ? $"begdistdate >= '{DistributionStart:dd.MM.yyyy}'"
                : string.Empty;

            string distend = DistributionEnd is not null
                ? $"begdistdate <= '{DistributionEnd:dd.MM.yyyy}'"
                : string.Empty;

            string matstart = MaturityStart is not null
                ? $"endmtydate >= '{MaturityStart:dd.MM.yyyy}'"
                : string.Empty;

            string matend = MaturityEnd is not null
                ? $"endmtydate <= '{MaturityEnd:dd.MM.yyyy}'"
                : string.Empty;

            string cur = Currency is not null
                ? $"faceftname = '{Currency}'"
                : string.Empty;

            string country = Country is not null
                ? $"issuercountry = '{Country}'"
                : string.Empty;

            string sectors = string.Empty;
            if (Sectors is not null)
                if (Sectors.Length is 1) sectors = $"issuersector = '{Sectors[0]}'";
                else sectors = $"issuersector in ('{string.Join("', '", Sectors)}')";

            string status = "status = 'В обращении'";
            string coupontype = "coupontype in ('Постоянный', 'Переменный', 'Фиксированный')";
            string sectype = "SecurityType = 'Корп'";

            string[] clauses = new string[] { sectype, country, status, diststart,
                coupontype, distend, matstart, matend, sectors, cur };

            return string.Join(" AND ", clauses.Where(str => !string.IsNullOrEmpty(str)));
        }
    }
}
