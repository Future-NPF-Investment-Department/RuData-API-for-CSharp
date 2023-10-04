namespace RuDataAPI.Extensions.Mapping
{
    public class NKRAttribute : RatingAttribute
    {
        public NKRAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
