
using RuDataAPI.Extensions.Ratings;

namespace RuDataAPI.Extensions
{
    public class EfirSecQueryDetails
    {
        public DateTime? DistributionStart { get; set; }
        public DateTime? DistributionEnd { get; set; }
        public DateTime? MaturityStart { get; set; }
        public DateTime? MaturityEnd { get; set; }
        public CreditRatingUS? RatingLow { get; set; }
        public CreditRatingUS? RatingHigh { get; set; }       
        public string[]? Sectors { get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }    

    }
}
