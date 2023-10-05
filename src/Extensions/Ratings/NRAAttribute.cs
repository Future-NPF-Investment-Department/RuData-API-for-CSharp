namespace RuDataAPI.Extensions.Ratings
{
    public class NRAAttribute : RatingStrAttribute
    {
        public NRAAttribute(params string[] ratings) : base()
            => _map.Add(NRA, ratings);
    }
}
