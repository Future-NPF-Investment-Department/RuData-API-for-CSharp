

namespace RuDataAPI.Extensions.Ratings
{
    public class CreditRatingAggregated
    {
        public CreditRatingUS RatingBig3 { get; set; }
        public CreditRatingRU RatingRu { get; set; }
        public CreditRatingScale Scale { get; set; }
        public CreditRatingCurrency Currency { get; set; }
        public IReadOnlyList<CreditRating> Ratings { get; set; }
        
    }
}
