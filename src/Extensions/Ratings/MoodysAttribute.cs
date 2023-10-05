namespace RuDataAPI.Extensions.Ratings
{
    public class MoodysAttribute : RatingStrAttribute
    {
        public MoodysAttribute(params string[] ratings) : base()
            => _map.Add(MOODYS, ratings);
    }
}
