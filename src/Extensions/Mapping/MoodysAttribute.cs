namespace RuDataAPI.Extensions.Mapping
{
    public class MoodysAttribute : RatingStrAttribute
    {
        public MoodysAttribute(params string[] ratings) : base()
            => _map.Add(MOODYS, ratings);
    }
}
