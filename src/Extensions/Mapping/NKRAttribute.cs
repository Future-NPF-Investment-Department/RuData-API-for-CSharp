namespace RuDataAPI.Extensions.Mapping
{
    public class NKRAttribute : RatingStrAttribute
    {
        public NKRAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
