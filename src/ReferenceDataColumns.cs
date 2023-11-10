namespace RuDataAPI
{
    /// <summary>
    ///     Column names bitmask for EFIR FintoolReferenceData method response.     
    /// </summary>
    [Flags]
    public enum RefDataCols : int
    {
        ALL                 = 0x7FFFFFFF,
        ALLCODES            = 0x1F,

        FINTOOLID           = 1 << 0, 
        ISINCODE            = 1 << 1, 
        MOEX_CODE           = 1 << 2,
        ISSUERINN           = 1 << 3,
        BORROWERINN         = 1 << 4,
        NICKNAME            = 1 << 5,
        ISSUERNAME_NRD      = 1 << 6,
        FINTOOLTYPE         = 1 << 7,
        STATUS              = 1 << 8,
        FACEVALUE           = 1 << 9,
        FACEFTNAME          = 1 << 10,
        SUMMARKETVAL        = 1 << 11,
        COUPONTYPE          = 1 << 12,
        COUPONRATE          = 1 << 13,
        FIRSTCOUPONDATE     = 1 << 14,
        COUPONTYPENAME_NRD  = 1 << 15,
        FLOATRATENAME       = 1 << 16,
        NUMCOUPONS          = 1 << 17,
        BASIS               = 1 << 18,    
        ISSUERSECTOR        = 1 << 19,
        ISSUERCOUNTRY       = 1 << 20,
        ENDMTYDATE          = 1 << 21,
        BEGDISTDATE         = 1 << 22,
        ENDDISTDATE         = 1 << 23,
        ISSUBORDINATED      = 1 << 24,
        BONDSTRUCTURALPAR   = 1 << 25,
        SECURITIZATION      = 1 << 26,
        HAVEINDEXEDFV       = 1 << 27,
        ISCONVERTIBLE       = 1 << 28,
        ISGUARANTEED        = 1 << 29,
        GUARANTVAL          = 1 << 30          
    }
}
