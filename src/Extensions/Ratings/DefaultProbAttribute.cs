namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Represents probability of default for particular rating.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DefaultProbAttribute : Attribute
    {
        public DefaultProbAttribute(double pd)
            => Pd = pd;

        /// <summary>
        ///     Probability of default.
        /// </summary>
        public double Pd { get; init; }
    }
}
