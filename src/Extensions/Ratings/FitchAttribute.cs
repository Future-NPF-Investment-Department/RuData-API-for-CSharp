namespace RuDataAPI.Extensions.Ratings
{
    public class FitchAttribute : RatingStrAttribute
    {
        public FitchAttribute(params string[] ratings) : base()
            => _map.Add(FITCH, ratings);
    }
}
