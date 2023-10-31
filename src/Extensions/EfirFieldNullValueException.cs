namespace RuDataAPI.Extensions
{
    public sealed class EfirFieldNullValueException : NullReferenceException
    {
#pragma warning disable IDE1006 // Naming Styles
        private const string MSGHEAD = "EFIR FIELD NULL VALUE ERROR: ";
#pragma warning restore IDE1006 // Naming Styles
        public EfirFieldNullValueException(string message) : base(MSGHEAD + message) 
        {
        }  
    }
}
