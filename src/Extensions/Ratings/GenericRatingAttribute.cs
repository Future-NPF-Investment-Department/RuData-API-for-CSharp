namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents generic rating to which ratings from different rating agencies can be reduced.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GenericRatingAttribute : Attribute
    {
        // these constants are taken from EFIR raings agencies classification.
        protected const string FITCH = "Fitch Ratings";
        protected const string MOODYS = "Moody's";
        protected const string SNP = "Standard & Poor's";
        protected const string AKRA = "АКРА";
        protected const string EXPRA = "Эксперт РА";
        protected const string NKR = "НКР";
        protected const string NRA = "НРА";

        private protected readonly Dictionary<string, string[]> _map = new();

        protected GenericRatingAttribute() { }

        public GenericRatingAttribute(string rating)
            => Rating = rating;

        /// <summary>
        ///     Generic rating value.
        /// </summary>
        public string Rating { get; init; } = null!;
    }
}
