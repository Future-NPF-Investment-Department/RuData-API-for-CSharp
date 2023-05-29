using Efir.DataHub.Models.Models.Info;

namespace RuDataAPI.Extensions
{
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
            CouponPeriod = finref.coupontypename_nrd;
            FirstCouponStartDate = finref.firstcoupondate;
            Status = finref.status;
            AssetClass = finref.fintooltype;
            IssueVolume = (double?)finref.summarketval;
            IssueSector = finref.issuersector;
        }

        public long? SecurityId { get; set; }
        public string? ShortName { get;set; }
        public string? FullName { get; set; }
        public string? Isin { get; set; }
        public string? IssuerName { get; set; }
        public DateTime? PlacementDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string? Currency { get; set; }
        public double? Notional { get; set; }
        public string? CouponReferenceRateName { get; set; }
        public string? CouponType { get; set; }
        public string? CouponPeriod { get; set; } 
        public DateTime? FirstCouponStartDate { get; set; }       
        public string? Status { get; set; }
        public string? AssetClass { get; set; }
        public double? IssueVolume { get; set; }
        public string? IssueSector { get; set; }




    }
}
