namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from NKR agency scale to map.
    /// </summary>
    public class NKRAttribute : GenericRatingAttribute
    {
        public NKRAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
