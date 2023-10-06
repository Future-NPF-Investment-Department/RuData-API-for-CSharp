namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from FITCH agency scale to map.
    /// </summary>
    public class FitchAttribute : GenericRatingAttribute
    {
        public FitchAttribute(params string[] ratings) : base()
            => _map.Add(FITCH, ratings);
    }
}
