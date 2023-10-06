namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from Akra agency scale to map.
    /// </summary>
    public class AKRAAttribute : GenericRatingAttribute
    {
        public AKRAAttribute(params string[] ratings) : base()
            => _map.Add(AKRA, ratings);
    }
}
