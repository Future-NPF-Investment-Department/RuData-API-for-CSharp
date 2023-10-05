namespace RuDataAPI.Extensions.Ratings
{
    public class AKRAAttribute : RatingStrAttribute
    {
        public AKRAAttribute(params string[] ratings) : base()
            => _map.Add(AKRA, ratings);
    }
}
