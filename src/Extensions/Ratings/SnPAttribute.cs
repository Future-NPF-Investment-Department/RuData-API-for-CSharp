namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from SnP agency scale to map.
    /// </summary>
    public class SnPAttribute : GenericRatingAttribute
    {
        public SnPAttribute(params string[] ratings) : base()
            => _map.Add(SNP, ratings);
    }
}
