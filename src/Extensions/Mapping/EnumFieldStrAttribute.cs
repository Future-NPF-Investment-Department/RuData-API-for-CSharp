

namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents collection of alternative string descriptions for enum field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumFieldStrAttribute : Attribute
    {
        public EnumFieldStrAttribute(params string[] strvals) 
            => Values = new HashSet<string>(strvals);
        
        /// <summary>
        ///     Set of possible unique string values.
        /// </summary>
        public HashSet<string> Values { get; set; }
    }
}
