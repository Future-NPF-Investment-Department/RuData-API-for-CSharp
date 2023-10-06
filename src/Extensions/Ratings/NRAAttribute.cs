namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from NRA agency scale to map.
    /// </summary>
    public class NRAAttribute : GenericRatingAttribute
    {
        public NRAAttribute(params string[] ratings) : base()
            => _map.Add(NRA, ratings);
    }
}
