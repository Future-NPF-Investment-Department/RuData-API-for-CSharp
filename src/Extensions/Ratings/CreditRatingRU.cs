
namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Russian generic rating scale values.
    /// </summary>
    /// <remarks>
    ///     <see cref="CreditRatingRU"/> is a bitmask for the cases when one object can have different ratings from different agencies.
    /// </remarks>
    [Flags]
    public enum CreditRatingRU
    {
        /// <summary>
        ///     Rating absence.
        /// </summary>
        [GenericRating("NR")]
        [AKRA("Снят", "Приостановлен", "NR"), ExpRA("Снят", "Приостановлен", "NR")]
        [NKR("Снят", "Приостановлен", "NR"), NRA("Снят", "Приостановлен", "NR")]
        [Fitch("Снят", "Приостановлен", "NR")]
        NR = 0x0,

        /// <summary>
        ///     AAA generic rating within Russian rating scale. Corresponds to AAA(ru) rating from AKRA, ruAAA rating
        ///     from ExpertRA, AAA.ru rating from NKR and AAA|ru| rating from NRA.
        ///     Implies 0.34% of probalility of default.
        /// </summary>
        [GenericRating("AAA", PD = 0.0034)]
        [AKRA("AAA(RU)", "eAAA(RU)", "AAA(ru.sf)", "eAAA(ru.sf)")]
        [ExpRA("ruAAA", "ruAAA(EXP)", "ruAAA.sf", "ruAAA.sf(EXP)", "A++")]
        [NKR("AAA.ru"), NRA("AAA|ru|")]
        [Fitch("AAA(rus)")]
        AAA = 1 << 19,

        /// <summary>
        ///     AA+ generic rating within Russian rating scale. Corresponds to AA+(ru) rating from AKRA, ruAA+ rating
        ///     from ExpertRA, AA+.ru rating from NKR and AA+|ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("AA+", PD = 0.0056)]
        [AKRA("AA+(RU)", "eAA+(RU)", "AA+(ru.sf)", "eAA+(ru.sf)")]
        [ExpRA("ruAA+", "ruAA+(EXP)", "ruAA+.sf", "ruAA+.sf(EXP)")]
        [NKR("AA+.ru"), NRA("AA+|ru|")]
        [Fitch("AA+(rus)")]
        AAplus = 1 << 18,

        /// <summary>
        ///     AA generic rating within Russian rating scale. Corresponds to AA(ru) rating from AKRA, ruAA rating
        ///     from ExpertRA, AA.ru rating from NKR and AA|ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("AA", PD = 0.0056)]
        [AKRA("AA(RU)", "eAA(RU)", "AA(ru.sf)", "eAA(ru.sf)")]
        [ExpRA("ruAA", "ruAA(EXP)", "ruAA.sf", "ruAA.sf(EXP)")]
        [NKR("AA.ru"), NRA("AA|ru|")]
        [Fitch("AA(rus)")]
        AA = 1 << 17,

        /// <summary>
        ///     AA- generic rating within Russian rating scale. Corresponds to AA-(ru) rating from AKRA, ruAA- rating
        ///     from ExpertRA, AA-.ru rating from NKR and AA-|ru| rating from NRA.
        ///     Implies 0.93% of probalility of default.
        /// </summary>
        [GenericRating("AA-", PD = 0.0093)]
        [AKRA("AA-(RU)", "eAA-(RU)", "AA-(ru.sf)", "eAA-(ru.sf)")]
        [ExpRA("ruAA-", "ruAA-(EXP)", "ruAA-.sf", "ruAA-.sf(EXP)", "A+ (I)")]
        [NKR("AA-.ru"), NRA("AA-|ru|")]
        [Fitch("AA-(rus)")]
        AAminus = 1 << 16,

        /// <summary>
        ///     A+ generic rating within Russian rating scale. Corresponds to A+(ru) rating from AKRA, ruA+ rating
        ///     from ExpertRA, A+.ru rating from NKR and A+|ru| rating from NRA.
        ///     Implies 0.93% of probalility of default.
        /// </summary>
        [GenericRating("A+", PD = 0.0093)]
        [AKRA("A+(RU)", "eA+(RU)", "A+(ru.sf)", "eA+(ru.sf)")]
        [ExpRA("ruA+", "ruA+(EXP)", "ruA+.sf", "ruA+.sf(EXP)", "A+ (II)")]
        [NKR("A+.ru"), NRA("A+|ru|")]
        [Fitch("A+(rus)")]
        Aplus = 1 << 15,

        /// <summary>
        ///     A generic rating within Russian rating scale. Corresponds to A(ru) rating from AKRA, ruA rating
        ///     from ExpertRA, A.ru rating from NKR and A|ru| rating from NRA.
        ///     Implies 1.55% of probalility of default.
        /// </summary>
        [GenericRating("A", PD = 0.0155)]
        [AKRA("A(RU)", "eA(RU)", "A(ru.sf)", "eA(ru.sf)")]
        [ExpRA("ruA", "ruA(EXP)", "ruA.sf", "ruA.sf(EXP)")]
        [NKR("A.ru"), NRA("A|ru|")]
        [Fitch("A(rus)")]
        A = 1 << 14,

        /// <summary>
        ///     A- generic rating within Russian rating scale. Corresponds to A-(ru) rating from AKRA, ruA- rating
        ///     from ExpertRA, A-.ru rating from NKR and A-|ru| rating from NRA.
        ///     Implies 1.55% of probalility of default.
        /// </summary>
        [GenericRating("A-", PD = 0.0155)]
        [AKRA("A-(RU)", "eA-(RU)", "A-(ru.sf)", "eA-(ru.sf)")]
        [ExpRA("ruA-", "ruA-(EXP)", "ruA-.sf", "ruA-.sf(EXP)", "A+ (III)")]
        [NKR("A-.ru"), NRA("A-|ru|")]
        [Fitch("A-(rus)")]
        Aminus = 1 << 13,

        /// <summary>
        ///     BBB+ generic rating within Russian rating scale. Corresponds to BBB+(ru) rating from AKRA, ruBBB+ rating
        ///     from ExpertRA, BBB+.ru rating from NKR and BBB+|ru| rating from NRA.
        ///     Implies 2.94% of probalility of default.
        /// </summary>
        [GenericRating("BBB+", PD = 0.0294)]
        [AKRA("BBB+(RU)", "eBBB+(RU)", "BBB+(ru.sf)", "eBBB+(ru.sf)")]
        [ExpRA("ruBBB+", "ruBBB+(EXP)", "ruBBB+.sf", "ruBBB+.sf(EXP)", "A (I)")]
        [NKR("BBB+.ru"), NRA("BBB+|ru|")]
        [Fitch("BBB+(rus)")]
        BBBplus = 1 << 12,

        /// <summary>
        ///     BBB generic rating within Russian rating scale. Corresponds to BBB(ru) rating from AKRA, ruBBB rating
        ///     from ExpertRA, BBB.ru rating from NKR and BBB|ru| rating from NRA.
        ///     Implies 2.94% of probalility of default.
        /// </summary>
        [GenericRating("BBB", PD = 0.0294)]
        [AKRA("BBB(RU)", "eBBB(RU)", "BBB(ru.sf)", "eBBB(ru.sf)")]
        [ExpRA("ruBBB", "ruBBB(EXP)", "ruBBB.sf", "ruBBB.sf(EXP)", "A (II)")]
        [NKR("BBB.ru"), NRA("BBB|ru|")]
        [Fitch("BBB(rus)")]
        BBB = 1 << 11,

        /// <summary>
        ///     BBB- generic rating within Russian rating scale. Corresponds to BBB-(ru) rating from AKRA, ruBBB- rating
        ///     from ExpertRA, BBB-.ru rating from NKR and BBB-|ru| rating from NRA.
        ///     Implies 4.66% of probalility of default.
        /// </summary>
        [GenericRating("BBB-", PD = 0.0466)]
        [AKRA("BBB-(RU)", "eBBB-(RU)", "BBB-(ru.sf)", "eBBB-(ru.sf)")]
        [ExpRA("ruBBB-", "ruBBB-(EXP)", "ruBBB-.sf", "ruBBB-.sf(EXP)")]
        [NKR("BBB-.ru"), NRA("BBB-|ru|")]
        [Fitch("BBB-(rus)")]
        BBBminus = 1 << 10,

        /// <summary>
        ///     BB+ generic rating within Russian rating scale. Corresponds to BB+(ru) rating from AKRA, ruBB+ rating
        ///     from ExpertRA, BB+.ru rating from NKR and BB+|ru| rating from NRA.
        ///     Implies 4.66% of probalility of default.
        /// </summary>
        [GenericRating("BB+", PD = 0.0466)]
        [AKRA("BB+(RU)", "eBB+(RU)", "BB+(ru.sf)", "eBB+(ru.sf)")]
        [ExpRA("ruBB+", "ruBB+(EXP)", "ruBB+.sf", "ruBB+.sf(EXP)")]
        [NKR("BB+.ru"), NRA("BB+|ru|")]
        [Fitch("BB+(rus)")]
        BBplus = 1 << 9,

        /// <summary>
        ///     BB generic rating within Russian rating scale. Corresponds to BB(ru) rating from AKRA, ruBB rating
        ///     from ExpertRA, BB.ru rating from NKR and BB|ru| rating from NRA.
        ///     Implies 7.17% of probalility of default.
        /// </summary>
        [GenericRating("BB", PD = 0.0717)]
        [AKRA("BB(RU)", "eBB(RU)", "BB(ru.sf)", "eBB(ru.sf)")]
        [ExpRA("ruBB", "ruBB(EXP)", "ruBB.sf", "ruBB.sf(EXP)", "A (III)")]
        [NKR("BB.ru"), NRA("BB|ru|")]
        [Fitch("BB(rus)")]
        BB = 1 << 8,

        /// <summary>
        ///     BB- generic rating within Russian rating scale. Corresponds to BB-(ru) rating from AKRA, ruBB- rating
        ///     from ExpertRA, BB-.ru rating from NKR and BB-|ru| rating from NRA.
        ///     Implies 11.95% of probalility of default.
        /// </summary>
        [GenericRating("BB-", PD = 0.1195)]
        [AKRA("BB-(RU)", "eBB-(RU)", "BB-(ru.sf)", "eBB-(ru.sf)")]
        [ExpRA("ruBB-", "ruBB-(EXP)", "ruBB-.sf", "ruBB-.sf(EXP)")]
        [NKR("BB-.ru"), NRA("BB-|ru|")]
        [Fitch("BB-(rus)")]
        BBminus = 1 << 7,

        /// <summary>
        ///     B+ generic rating within Russian rating scale. Corresponds to B+(ru) rating from AKRA, ruB+ rating
        ///     from ExpertRA, B+.ru rating from NKR and B+|ru| rating from NRA.
        ///     Implies 19.91% of probalility of default.
        /// </summary>
        [GenericRating("B+", PD = 0.1991)]
        [AKRA("B+(RU)", "eB+(RU)", "B+(ru.sf)", "eB+(ru.sf)")]
        [ExpRA("ruB+", "ruB+(EXP)", "ruB+.sf", "ruB+.sf(EXP)", "B++")]
        [NKR("B+.ru"), NRA("B+|ru|")]
        [Fitch("B+(rus)")]
        Bplus = 1 << 6,

        /// <summary>
        ///     B generic rating within Russian rating scale. Corresponds to B(ru) rating from AKRA, ruB rating
        ///     from ExpertRA, B.ru rating from NKR and B|ru| rating from NRA.
        ///     Implies 4297% of probalility of default.
        /// </summary>
        [GenericRating("B", PD = 0.4297)]
        [AKRA("B(RU)", "eB(RU)", "B(ru.sf)", "eB(ru.sf)")]
        [ExpRA("ruB", "ruB(EXP)", "ruB.sf", "ruB.sf(EXP)")]
        [NKR("B.ru"), NRA("B|ru|")]
        [Fitch("B(rus)")]
        B = 1 << 5,

        /// <summary>
        ///     B- generic rating within Russian rating scale. Corresponds to B-(ru) rating from AKRA, ruB- rating
        ///     from ExpertRA, B-.ru rating from NKR and B-|ru| rating from NRA.
        ///     Implies 7764% of probalility of default.
        /// </summary>
        [GenericRating("B-", PD = 0.7764)]
        [AKRA("B-(RU)", "eB-(RU)", "B-(ru.sf)", "eB-(ru.sf)")]
        [ExpRA("ruB-", "ruB-(EXP)", "ruB-.sf", "ruB-.sf(EXP)", "B+")]
        [NKR("B-.ru"), NRA("B-|ru|")]
        [Fitch("B-(rus)")]
        Bminus = 1 << 4,

        /// <summary>
        ///     CCC generic rating within Russian rating scale. Corresponds to CCC(ru) rating from AKRA, ruCCC rating
        ///     from ExpertRA, CCC.ru rating from NKR and CCC|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("CCC", PD = 0.9662)]
        [AKRA("CCC(RU)", "eCCC(RU)", "CCC(ru.sf)", "eCCC(ru.sf)")]
        [ExpRA("ruCCC", "ruCCC(EXP)", "ruCCC.sf", "ruCCC.sf(EXP)", "B")]
        [NKR("CCC.ru"), NRA("CCC|ru|")]
        [Fitch("CCC(rus)")]
        CCC = 1 << 3,

        /// <summary>
        ///     CC generic rating within Russian rating scale. Corresponds to CC(ru) rating from AKRA, ruCC rating
        ///     from ExpertRA, CC.ru rating from NKR and CC|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("CC", PD = 0.9662)]
        [AKRA("CC(RU)", "eCC(RU)", "CC(ru.sf)", "eCC(ru.sf)")]
        [ExpRA("ruCC", "ruCC(EXP)","ruCC.sf", "ruCC.sf(EXP)", "C++")]
        [NKR("CC.ru"), NRA("CC|ru|")]
        [Fitch("CC(rus)")]
        CC = 1 << 2,

        /// <summary>
        ///     C generic rating within Russian rating scale. Corresponds to C(ru) rating from AKRA, ruC rating
        ///     from ExpertRA, C.ru rating from NKR and C|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("C", PD = 0.9662)]
        [AKRA("C(RU)", "eC(RU)", "C(ru.sf)", "eC(ru.sf)")]
        [ExpRA("ruC", "ruC(EXP)", "ruC,sf", "ruC.sf(EXP)", "C+")]
        [NKR("C.ru"), NRA("C|ru|")]
        [Fitch("C(rus)")]
        C = 1 << 1,

        /// <summary>
        ///     D generic rating within Russian rating scale. Corresponds to D(ru) rating from AKRA, ruD rating
        ///     from ExpertRA, D.ru rating from NKR and D|ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("D", PD = 0.9950)]
        [AKRA("D(RU)", "eD(RU)", "D(ru.sf)", "eD(ru.sf)")]
        [ExpRA("ruD", "ruD(EXP)", "ruD.sf", "ruD.sf(EXP)", "C")]
        [NKR("D.ru"), NRA("D|ru|")]
        [Fitch("D(rus)")]
        D = 1 << 0,
    }
}
