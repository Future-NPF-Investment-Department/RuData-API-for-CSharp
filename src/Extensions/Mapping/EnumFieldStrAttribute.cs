

namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents collection of alternative string descriptions for enum field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumFieldStrAttribute : Attribute
    {
        public EnumFieldStrAttribute(params string[] stringVal) 
            => Values = stringVal;
        
        public string[] Values { get; set; }
    }
}
