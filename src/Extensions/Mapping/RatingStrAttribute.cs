
namespace RuDataAPI.Extensions.Mapping
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RatingStrAttribute : Attribute
    {
        protected const string FITCH      = "Fitch Ratings";
        protected const string MOODYS     = "Moody's";
        protected const string SNP        = "Standard & Poor's";
        protected const string AKRA       = "АКРА";
        protected const string EXPRA      = "Эксперт РА";
        protected const string NKR        = "НКР";
        protected const string NRA        = "НРА";

        private protected readonly Dictionary<string, string[]> _map = new();

        protected RatingStrAttribute() { }

        public RatingStrAttribute(string rating)         
            => Rating = rating;
        
        public string Rating { get; init; } = null!;
    }
}
