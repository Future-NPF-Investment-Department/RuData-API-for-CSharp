#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals(object o)


namespace RuDataAPI.Extensions.Ratings
{
    public class CreditRating
    {
        public string Value { get; set; } = null!;
        public RatingUS ValueAggregated { get; set; }
        public CreditRatingScale Scale { get; set; }
        public CreditRatingCurrency Currency { get; set; }
        public DateTime Date { get; set; }
        public string Agency { get; set; } = null!;
        public string Action { get; set; } = null!;
        public double DefaultProbability { get; set; }


        public static bool operator <=(CreditRating rating1, RatingUS value)
            => rating1.ValueAggregated <= value;

        public static bool operator >=(CreditRating rating1, RatingUS value)
            => rating1.ValueAggregated >= value;

        public static bool operator ==(CreditRating rating1, RatingUS value)
            => rating1.ValueAggregated == value;

        public static bool operator !=(CreditRating rating1, RatingUS value)
            => rating1.ValueAggregated != value;
    }
}
