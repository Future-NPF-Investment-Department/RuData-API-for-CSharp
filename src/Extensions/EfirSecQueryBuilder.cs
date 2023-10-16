
using RuDataAPI.Extensions.Ratings;

namespace RuDataAPI.Extensions
{
    public sealed class EfirSecQueryBuilder
    {
        private readonly EfirSecQueryDetails _query;
        public EfirSecQueryBuilder()
            => _query = new EfirSecQueryDetails();


        public EfirSecQueryBuilder WithDistributionDateStart(DateTime start)
        {
            _query.DistributionStart = start;
            return this;
        }

        public EfirSecQueryBuilder WithDistributionDateEnd(DateTime end)
        {
            _query.DistributionEnd = end;
            return this;
        }

        public EfirSecQueryBuilder WithMaturityDateStart(DateTime start)
        {
            _query.MaturityStart = start;
            return this;
        }

        public EfirSecQueryBuilder WithMaturityDateEnd(DateTime end)
        {
            _query.MaturityEnd = end;
            return this;
        }

        public EfirSecQueryBuilder WithRatingBig3(CreditRatingUS rating)
        {
            _query.Big3RatingLow = rating;
            _query.Big3RatingHigh = rating;
            return this;
        }

        public EfirSecQueryBuilder WithRatingBig3(CreditRatingUS low, CreditRatingUS high)
        {
            _query.Big3RatingLow = low;
            _query.Big3RatingHigh = high;
            return this;
        }

        public EfirSecQueryBuilder WithRatingRu(CreditRatingRU rating)
        {
            _query.RuRatingLow = rating;
            _query.RuRatingHigh = rating;
            return this;
        }

        public EfirSecQueryBuilder WithRatingRu(CreditRatingRU low, CreditRatingRU high)
        {
            _query.RuRatingLow = low;
            _query.RuRatingHigh = high;
            return this;
        }

        public EfirSecQueryBuilder WithSector(params string[] sectors)
        {
            _query.Sectors = sectors;
            return this;
        }

        public EfirSecQueryBuilder WithCurrency(string currecny)
        {
            _query.Currency = currecny;
            return this;
        }

        public EfirSecQueryBuilder WithIssuerCountry(string countryCode)
        {
            _query.Country = countryCode;
            return this;
        }

        public EfirSecQueryDetails Build() => _query;
    }
}
