namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Security issue classifications.
    /// </summary>
    [Flags]
    public enum InstrumentFlags
    {
        None            = 0,
        Structured      = 1,
        Secured         = 2,
        Subordinated    = 4,
        Linker          = 8,
        Convertible     = 16,
        Guaranteed      = 32,
    }
}
