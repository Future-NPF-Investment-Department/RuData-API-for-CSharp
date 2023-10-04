namespace RuDataAPI.Extensions.Mapping
{
    public class AKRAAttribute : RatingAttribute
    {
        public AKRAAttribute(params string[] ratings) : base()
            => _map.Add(AKRA, ratings);
    }
}
