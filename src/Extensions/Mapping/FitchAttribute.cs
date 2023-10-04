namespace RuDataAPI.Extensions.Mapping
{
    public class FitchAttribute : RatingAttribute
    {
        public FitchAttribute(params string[] ratings) : base()
            => _map.Add(FITCH, ratings);        
    }
}
