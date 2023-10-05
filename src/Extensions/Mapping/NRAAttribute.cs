namespace RuDataAPI.Extensions.Mapping
{
    public class NRAAttribute : RatingStrAttribute
    {
        public NRAAttribute(params string[] ratings) : base()
            => _map.Add(NRA, ratings);
    }
}
