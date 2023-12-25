namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents rating agency.
    /// </summary>
    public enum RatingAgency
    {
        /// <summary>
        ///     Unknown rating agency.
        /// </summary>
        [EnumFieldStr(PrintString = "Unknown RA")]
        UNDEFINED,

        /// <summary>
        ///     Moody’s Investors Service (MIS) international rating agency.
        /// </summary>
        [EnumFieldStr("Moody's", PrintString = "Moodys")]
        MOODYS,

        /// <summary>
        ///     Fitch Ratings Inc. international rating agency.
        /// </summary>
        [EnumFieldStr("Fitch Ratings", PrintString = "FITCH")]
        FITCH,

        /// <summary>
        ///     S&P Global Ratings international rating agency.
        /// </summary>
        [EnumFieldStr("Standard & Poor's", PrintString = "S&P")]
        SNP,

        /// <summary>
        ///     AKRA russian national rating agency.
        /// </summary>
        [EnumFieldStr("АКРА", PrintString = "AKRA")]
        AKRA,

        // <summary>
        ///     Expert RA russian national rating agency.
        /// </summary>
        [EnumFieldStr("Эксперт РА", PrintString = "RaEx")]
        RAEX,

        // <summary>
        ///     NKR russian national rating agency.
        /// </summary>
        [EnumFieldStr("НКР", PrintString = "NKR")]
        NKR,

        // <summary>
        ///     NRA russian national rating agency.
        /// </summary>
        [EnumFieldStr("НРА", PrintString = "NRA")]
        NRA
    }
}
