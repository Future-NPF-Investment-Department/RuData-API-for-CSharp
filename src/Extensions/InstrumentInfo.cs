using Efir.DataHub.Models.Models.Info;
using RuDataAPI.Extensions.Ratings;
using RuDataAPI.Extensions.Mapping;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents financial instrument / security information.
    /// </summary>
    public class InstrumentInfo
    {
        public InstrumentInfo() { }   

        /// <summary>
        ///     Interfax security ID.
        /// </summary>
        public long IfaxSecId { get; set; }

        /// <summary>
        ///     MOEX instrument ID.
        /// </summary>
        public string MoexSecId { get; set; } = string.Empty;

        /// <summary>
        ///     Security short name.
        /// </summary>
        public string Name { get; set; } = string.Empty;        
        
        /// <summary>
        ///     Security ISIN code.
        /// </summary>
        public string Isin { get; set; } = string.Empty;
        
        /// <summary>
        ///     Security issuer's name.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        ///     Real borrower's name.
        /// </summary>
        public string Borrower { get; set; } = string.Empty;

        /// <summary>
        ///     Security issuer's INN code.
        /// </summary>
        public string IssuerInn { get; set; } = string.Empty;

        /// <summary>
        ///     Security real borrower's INN code.
        /// </summary>
        public string BorrowerInn { get; set; } = string.Empty;

        /// <summary>
        ///     Security placement date. Date of issuance.
        /// </summary>
        public DateTime PlacementDate { get; set; }
        
        /// <summary>
        ///     Security maturity date. Only for bonds.
        /// </summary>
        public DateTime MaturityDate { get; set; }
        
        /// <summary>
        ///     Security currency
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        ///     Current notional of security.
        /// </summary>
        public double CurrentFaceValue { get; set; }

        /// <summary>
        ///     Initial notional of security.
        /// </summary>
        public double InitialFaceValue { get; set; }

        /// <summary>
        ///     Floating reference rate used in coupon rate.
        /// </summary>
        public string CouponReference { get; set; } = string.Empty;
        
        /// <summary>
        ///     Type of coupon payment (fixed, floating, etc.).
        /// </summary>
        public CouponType CouponType { get; set; }
        
        /// <summary>
        ///     Coupon period length type (annual, semiannual, etc.).
        /// </summary>
        public CouponPeriodType CouponLength { get; set; } 

        /// <summary>
        ///     Coupon basis.
        /// </summary>
        public CouponBasis Basis { get; set; }
        
        /// <summary>
        ///     Security status.
        /// </summary>
        public InstrumentStatus Status { get; set; }
        
        /// <summary>
        ///     Security asset class (equity, bond, etc.).
        /// </summary>
        public InstrumentType AssetClass { get; set; }
        
        /// <summary>
        ///     Issuance volume in units of curency.
        /// </summary>
        public double MarketVolume { get; set; }

        /// <summary>
        ///     Guaranteed sum given default.
        /// </summary>
        public double GuaranteedValue { get; set; }
        
        /// <summary>
        ///     Security issuer's sector (gov, fin, etc.).
        /// </summary>
        public IssuerSector IssuerSector { get; set; }

        /// <summary>
        ///     Security borrower's sector (gov, fin, etc.).
        /// </summary>
        public IssuerSector BorrowerSector { get; set; }

        /// <summary>
        ///     Financial intrument classification.
        /// </summary>
        public InstrumentFlags Flags { get; set; }

        /// <summary>
        ///     Flows data.
        /// </summary>
        public IEnumerable<InstrumentFlow>? Flows { get; set; }
        
        /// <summary>
        ///     Trade history data.
        /// </summary>
        public IEnumerable<InstrumentHistoryRecord>? TradeHistory { get; set; }

        /// <summary>
        ///     Aggregated credit rating.
        /// </summary>
        public CreditRatingAggregated RatingAggregated { get; set; } = CreditRatingAggregated.Default;
    }
}
