namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents possible ratings from Moody's agency scale to map.
    /// </summary>
    public class MoodysAttribute : GenericRatingAttribute
    {
        public MoodysAttribute(params string[] ratings) : base()
            => _map.Add(MOODYS, ratings);
    }
}
