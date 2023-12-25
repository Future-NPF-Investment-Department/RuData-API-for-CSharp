namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents collection of alternative string descriptions for enum field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumFieldStrAttribute : Attribute
    {
        private HashSet<string> _values;

        /// <summary>
        ///     Creates string-enum mapping with predefined string values to be mapped.
        /// </summary>        
        public EnumFieldStrAttribute(params string[] strvals) 
            => _values = new HashSet<string>(strvals);
        
        /// <summary>
        ///     Values for mapping.
        /// </summary>
        public HashSet<string> Values => _values;

        /// <summary>
        ///     Value used for printing output.
        /// </summary>
        public string PrintString { get; init; } = string.Empty;
    }
}
