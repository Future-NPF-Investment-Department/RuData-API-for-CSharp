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

        public override string ToString()
        {
            const string ACNT   = "\x1b[38;5;210m";     // yellow color accent 
            const string RST    = "\x1b[0m";            // normal color accent
            const string UNDSCR = "\x1b[4m";            // underscore accent
            const char LF = '\n';

            string commonInfo = "";
            string issuerInfo = "";
            string datesInfo = "";
            string couponInfo = "";
            string ratingInfo = "";
            string flowsInfo = "";
            string historyInfo = "";


            string name = string.IsNullOrEmpty(Name) ? "UNKNOWN SECURITY" : Name;
            string isin = string.IsNullOrEmpty(Isin) ? string.Empty : $"{ACNT}ISIN{RST}: {Isin}";
            string ifxid = IfaxSecId is 0 ? string.Empty : $"{ACNT}INTERFAX{RST}: {IfaxSecId}";
            string moexid = string.IsNullOrEmpty(MoexSecId) || MoexSecId == isin ? string.Empty : $"{ACNT}MOEX{RST}: {MoexSecId}";
            string header = $"Instrument:\t" + name + $" ({isin} {moexid} {ifxid})" + LF;
            string status = $"Status:\t\t{Status}" + LF;
            string flags = $"Flags:\t\t{Flags}" + LF;
            string faceVal = "Face:\t\t" + $"{Currency} {CurrentFaceValue:n}" + LF;
            string mktvol = "Mkt. Vol.:\t" + $"{Currency} {MarketVolume:n}" + LF;
            string guarnty = GuaranteedValue is not 0 ? "Guaranty:" + $"{GuaranteedValue:n}" + LF : string.Empty;

            commonInfo = $"{UNDSCR}INSTRUMENT{RST}:" + LF + header + status + faceVal + mktvol + guarnty + flags + LF;



            string issuer = "";
            string issuername = string.IsNullOrEmpty(Issuer) ? "Unknown" : Issuer;
            string issuerinn = string.IsNullOrEmpty(IssuerInn) ? string.Empty + LF: $" ({ACNT}INN{RST}: {IssuerInn})" + LF;
            string sector = $"Sector:\t\t{IssuerSector}" + LF;

            if (string.IsNullOrEmpty(BorrowerInn) || BorrowerInn == IssuerInn)
            {
                issuer += "Issuer:\t\t" + issuername + issuerinn + sector;
            }
            else
            {
                string borrowername = string.IsNullOrEmpty(Borrower) ? "Unknown" : Borrower;
                string borrowerinn = string.IsNullOrEmpty(BorrowerInn) ? string.Empty + LF : $" ({ACNT}INN{RST}: {BorrowerInn})" + LF;
                string borrowerSector = $"Borrower sector:\t{BorrowerSector}" + LF;
                sector = "Issuer " + sector;
                issuer += "Issuer:\t\t" + issuername + issuerinn + "Borrower:\t\t" + borrowername + borrowerinn + sector + borrowerSector;
            }

            issuerInfo = $"{UNDSCR}ISSUER{RST}:" + LF + issuer + LF;


            string distDate = "Placement:\t" + PlacementDate.ToShortDateString() + LF;
            string matDate = AssetClass is InstrumentType.BOND ? "Maturity:\t" + MaturityDate.ToShortDateString() + LF : string.Empty + LF;
            datesInfo = $"{UNDSCR}DATES{RST}:" + LF + distDate + matDate + LF;



            // coupons
            if (AssetClass is InstrumentType.BOND)
            {
                string ctype = "Type:\t\t" + CouponType + LF;
                string cref = CouponType is CouponType.Floating ? "Reference:\t" + CouponReference + LF : string.Empty;
                string cbasis = "Basis:\t\t" + Basis.ToString() + LF;
                string clen = "Length:\t\t" + CouponLength.ToString() + LF;

                couponInfo = $"{UNDSCR}COUPON{RST}:" + LF + ctype + cref + clen + cbasis + LF;
            }


            // aggr ratings
            string aggrRatingBig3 = "International:\t\t" + ACNT + RatingAggregated.ToShortStringBig3() + RST + LF;
            string aggrRatingRu = "National:\t\t" + ACNT + RatingAggregated.ToShortStringRu() + RST + LF;
            string prob = "Default prob.:\t\t" + $"{RatingAggregated.DefaultProbability:0.00%}" + LF;
            string aggrRatingInfo = $"{UNDSCR}AGGREGATED RATING{RST}:" + LF + aggrRatingBig3 + aggrRatingRu + prob + LF;

            // last ratings
            string lastRatingsInfo = string.Empty;
            if (RatingAggregated.Ratings is not null)
            {
                string ratingslist = string.Empty;
                foreach (var creditRating in  RatingAggregated.Ratings)
                    ratingslist += creditRating.ToShortString() + LF;
                lastRatingsInfo = $"{UNDSCR}LAST RATINGS{RST}:" + LF + ratingslist + LF;
            }
            else
            {
                lastRatingsInfo = "No ratings found for that ISIN." + LF;
            }    
            
            ratingInfo = aggrRatingInfo + lastRatingsInfo;


            if (Flows is not null)
            {
                flowsInfo = $"{UNDSCR}FLOWS{RST}:" + LF;
                foreach (var flow in Flows)                
                    flowsInfo += $"{ACNT}{flow.PaymentType}{RST}|{flow.EndDate.ToShortDateString()}|{flow.Rate:0.00%}|{flow.Payment}|{flow.PeriodLength}" + LF;
                flowsInfo += LF;
            }

            return commonInfo + issuerInfo + datesInfo + couponInfo + ratingInfo + flowsInfo;
        }

    }
}
