using Efir.DataHub.Models.Models.Info;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents security fetched form Efir Server.
    /// </summary>
    public class EfirSecurity
    {
        public EfirSecurity(FintoolReferenceDataFields finref) 
        {
            SecurityId = finref.fintoolid;
            ShortName = finref.nickname;
            FullName = finref.fullname;
            Isin = finref.isincode;
            IssuerName= finref.issuername_nrd;
            PlacementDate = finref.enddistdate;
            MaturityDate = finref.endmtydate;
            Currency = finref.faceftname;
            Notional = (double?)finref.facevalue;
            CouponReferenceRateName = finref.floatratename;
            CouponType = finref.coupontype;
            CouponPeriodType = finref.coupontypename_nrd;
            FirstCouponStartDate = finref.firstcoupondate;
            Status = finref.status;
            AssetClass = finref.fintooltype;
            IssueVolume = (double?)finref.summarketval;
            IssueSector = finref.issuersector;
        }

        /// <summary>
        ///     Efir security ID.
        /// </summary>
        public long? SecurityId { get; set; }
        
        /// <summary>
        ///     Security short name.
        /// </summary>
        public string? ShortName { get;set; }
        
        /// <summary>
        ///     Security full name.
        /// </summary>
        public string? FullName { get; set; }
        
        /// <summary>
        ///     Security ISIN code.
        /// </summary>
        public string? Isin { get; set; }
        
        /// <summary>
        ///     Security issuer's name.
        /// </summary>
        public string? IssuerName { get; set; }
        
        /// <summary>
        ///     Security placement date. Date of issuance.
        /// </summary>
        public DateTime? PlacementDate { get; set; }
        
        /// <summary>
        ///     Security maturity date. Only for bonds.
        /// </summary>
        public DateTime? MaturityDate { get; set; }
        
        /// <summary>
        ///     Security currency
        /// </summary>
        public string? Currency { get; set; }
        
        /// <summary>
        ///     Security notional.
        /// </summary>
        public double? Notional { get; set; }
        
        /// <summary>
        ///     Floating reference rate used in coupon rate.
        /// </summary>
        public string? CouponReferenceRateName { get; set; }
        
        /// <summary>
        ///     Type of coupon (fixed, floating, etc.).
        /// </summary>
        public string? CouponType { get; set; }
        
        /// <summary>
        ///     Coupon period length type (annual, semiannual, etc.).
        /// </summary>
        public string? CouponPeriodType { get; set; } 
        
        /// <summary>
        ///     List of <see cref="CouponPeriod"/> data.
        /// </summary>
        public List<CouponPeriod>? CouponSchedule { get; set; }
        
        /// <summary>
        ///     Start date of first Coupon period
        /// </summary>
        public DateTime? FirstCouponStartDate { get; set; }       
        
        /// <summary>
        ///     Security status.
        /// </summary>
        public string? Status { get; set; }
        
        /// <summary>
        ///     Security asset class (equity, bond, etc.).
        /// </summary>
        public string? AssetClass { get; set; }
        
        /// <summary>
        ///     Issuance volume in units of curency.
        /// </summary>
        public double? IssueVolume { get; set; }
        
        /// <summary>
        ///     Security issuer's sector (gov, fin, etc.).
        /// </summary>
        public string? IssueSector { get; set; }
    }
}
