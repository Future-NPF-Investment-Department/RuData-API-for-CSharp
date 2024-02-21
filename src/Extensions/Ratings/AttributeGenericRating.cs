
namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents generic credit rating naming.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GenericRatingAttribute : Attribute
    {
        private readonly string _name;

        public GenericRatingAttribute(string ratingName)        
            => _name = ratingName;        

        /// <summary>
        ///     Generic credit rating name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        ///     Probability of default implied by this credit rating.
        /// </summary>
        public double PD { get; set; }
    }
}
