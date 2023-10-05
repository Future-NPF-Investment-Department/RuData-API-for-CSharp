namespace RuDataAPI.Extensions.Ratings
{
    public class NKRAttribute : RatingStrAttribute
    {
        public NKRAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
