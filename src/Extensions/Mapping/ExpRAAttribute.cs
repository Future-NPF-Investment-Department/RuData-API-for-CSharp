namespace RuDataAPI.Extensions.Mapping
{
    public class ExpRAAttribute : RatingAttribute
    {
        public ExpRAAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
