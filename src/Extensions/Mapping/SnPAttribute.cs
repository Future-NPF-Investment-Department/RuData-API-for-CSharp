namespace RuDataAPI.Extensions.Mapping
{
    public class SnPAttribute : RatingStrAttribute
    {
        public SnPAttribute(params string[] ratings) : base()
            => _map.Add(SNP, ratings);
    }
}
