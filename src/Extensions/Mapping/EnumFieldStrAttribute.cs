

namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents alternative string description for enum field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumFieldStrAttribute : Attribute
    {
        public EnumFieldStrAttribute(string stringVal) 
            => Value = stringVal;
        
        public string Value { get; set; }
    }
}
