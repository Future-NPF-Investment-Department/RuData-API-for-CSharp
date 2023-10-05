namespace RuDataAPI.Extensions.Ratings
{
    public class ExpRAAttribute : RatingStrAttribute
    {
        public ExpRAAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
