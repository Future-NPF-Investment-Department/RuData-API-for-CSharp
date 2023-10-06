namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from Expert RA agency scale to map.
    /// </summary>
    public class ExpRAAttribute : GenericRatingAttribute
    {
        public ExpRAAttribute(params string[] ratings) : base()
            => _map.Add(EXPRA, ratings);
    }
}
