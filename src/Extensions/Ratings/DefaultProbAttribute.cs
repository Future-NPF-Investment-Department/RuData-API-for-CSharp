namespace RuDataAPI.Extensions.Ratings
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DefaultProbAttribute : Attribute
    {
        public DefaultProbAttribute(double pd)
            => Pd = pd;

        public double Pd { get; init; }
    }
}
