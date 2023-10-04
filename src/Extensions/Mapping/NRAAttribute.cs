namespace RuDataAPI.Extensions.Mapping
{
    public class NRAAttribute : RatingAttribute
    {
        public NRAAttribute(params string[] ratings) : base()
            => _map.Add(NRA, ratings);
    }
}
