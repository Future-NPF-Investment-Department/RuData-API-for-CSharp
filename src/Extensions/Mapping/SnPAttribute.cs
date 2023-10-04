namespace RuDataAPI.Extensions.Mapping
{
    public class SnPAttribute : RatingAttribute
    {
        public SnPAttribute(params string[] ratings) : base()
            => _map.Add(SNP, ratings);
    }
}
