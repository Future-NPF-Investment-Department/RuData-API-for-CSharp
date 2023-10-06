
namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     International (US) generic rating scale values. 
    ///     Serves as global rating scale to which other rating scales can be reduced.
    /// </summary>
    public enum CreditRatingUS
    {
        /// <summary>
        ///     Rating absence.
        /// </summary>
        [GenericRating("NR")]
        [Fitch("Снят"), SnP("Снят"), Moodys("Снят")]
        [AKRA("Снят"), ExpRA("Снят"), NKR("Снят"), NRA("Снят")]
        NR = 0,

        /// <summary>
        ///     AAA generic rating. Corresponds to AAA rating (FITCH, SnP) and Aaa rating (Moody's).
        ///     Implies 0% of probalility of default.
        /// </summary>
        [GenericRating("AAA"), DefaultProb(0.00)]
        [Fitch("AAA"), SnP("AAA"), Moodys("Aaa")]
        AAA = 22,

        /// <summary>
        ///     AA+ generic rating. Corresponds to AA+ rating (FITCH, SnP) and Aa1 rating (Moody's).
        ///     Implies 0.01% of probalility of default.
        /// </summary>
        [GenericRating("AA+"), DefaultProb(0.0001)]
        [Fitch("AA+"), SnP("AA+"), Moodys("Aa1")]
        AAplus = 21,

        /// <summary>
        ///     AA generic rating. Corresponds to AA rating (FITCH, SnP) and Aa2 rating (Moody's).
        ///     Implies 0.01% of probalility of default.
        /// </summary>
        [GenericRating("AA"), DefaultProb(0.0001)]
        [Fitch("AA"), SnP("AA"), Moodys("Aa2")]
        AA = 20,

        /// <summary>
        ///     AA- generic rating. Corresponds to AA- rating (FITCH, SnP) and Aa3 rating (Moody's).
        ///     Implies 0.02% of probalility of default.
        /// </summary>
        [GenericRating("AA-"), DefaultProb(0.0002)]
        [Fitch("AA-"), SnP("AA-"), Moodys("Aa3")]
        AAminus = 19,

        /// <summary>
        ///     A+ generic rating. Corresponds to A rating (FITCH, SnP) and A1 rating (Moody's).
        ///     Implies 0.03% of probalility of default.
        /// </summary>
        [GenericRating("A+"), DefaultProb(0.0003)]
        [Fitch("A+"), SnP("A+"), Moodys("A1")]
        Aplus = 18,

        /// <summary>
        ///     A generic rating. Corresponds to A rating (FITCH, SnP) and A2 rating (Moody's).
        ///     Implies 0.04% of probalility of default.
        /// </summary>
        [GenericRating("A"), DefaultProb(0.0004)]
        [Fitch("A"), SnP("A"), Moodys("A2")]
        A = 17,

        /// <summary>
        ///     A- generic rating. Corresponds to A- rating (FITCH, SnP) and A3 rating (Moody's).
        ///     Implies 0.07% of probalility of default.
        /// </summary>
        [GenericRating("A-"), DefaultProb(0.0007)]
        [Fitch("A-"), SnP("A-"), Moodys("A3")]
        Aminus = 16,

        /// <summary>
        ///     BBB+ generic rating. Corresponds to BBB+ rating (FITCH, SnP) and Baa1 rating (Moody's).
        ///     Implies 0.12% of probalility of default.
        /// </summary>
        [GenericRating("BBB+"), DefaultProb(0.0012)]
        [Fitch("BBB+"), SnP("BBB+"), Moodys("Baa1")]
        BBBplus = 15,

        /// <summary>
        ///     BBB generic rating. Corresponds to BBB rating (FITCH, SnP) and Baa2 rating (Moody's).
        ///     Implies 0.20% of probalility of default.
        /// </summary>
        [GenericRating("BBB"), DefaultProb(0.0020)]
        [Fitch("BBB"), SnP("BBB"), Moodys("Baa2")]
        BBB = 14,

        /// <summary>
        ///     BBB- generic rating. Corresponds to BBB rating (FITCH, SnP), Baa3 rating (Moody's), 
        ///     AAA(ru) rating (AKRA), ruAAA rating (ExpertRA), AAA.ru rating (NKR) and AAA|ru| rating (NRA).
        ///     Implies 0.34% of probalility of default.
        /// </summary>
        [GenericRating("BBB-"), DefaultProb(0.0034)]
        [Fitch("BBB-"), SnP("BBB-"), Moodys("Baa3")]
        [AKRA("AAA(RU)", "eAAA(RU)", "AAA(ru.sf)", "eAAA(ru.sf)")]
        [ExpRA("ruAAA", "ruAAA(EXP)", "ruAAA.sf", "ruAAA.sf(EXP)")]
        [NKR("AAA.ru"), NRA("AAA|ru|")]
        BBBminus = 13,

        /// <summary>
        ///     BB+ generic rating. Corresponds to BB+ rating from FITCH or SnP, Ba1 rating from Moody's, AA+(ru) or AA(RU) ratings 
        ///     from AKRA, ruAA+ or ruAA ratings from ExpertRA, AA+.ru or AA.ru ratings from NKR and AA+|ru| or AA|ru| ratings from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("BB+"), DefaultProb(0.0056)]
        [Fitch("BB+"), SnP("BB+"), Moodys("Ba1")]
        [AKRA("AA+(RU)", "AA(RU)", "eAA+(RU)", "eAA(RU)", "AA+(ru.sf)", "AA(ru.sf)", "eAA+(ru.sf)", "eAA(ru.sf)")] 
        [ExpRA("ruAA+", "ruAA", "ruAA+(EXP)", "ruAA(EXP)", "ruAA+.sf", "ruAA.sf", "ruAA+.sf(EXP)", "ruAA.sf(EXP)")]
        [NKR("AA+.ru", "AA.ru"), NRA("AA+|ru|", "AA|ru|")]
        BBplus = 12,

        /// <summary>
        ///     BB generic rating. Corresponds to BB rating from FITCH or SnP, Ba2 rating from Moody's, AA-(ru) or A+(RU) ratings 
        ///     from AKRA, ruAA- or ruA+ ratings from ExpertRA, AA-.ru or A+.ru ratings from NKR and AA-|ru| or A+|ru| ratings from NRA.
        ///     Implies 0.93% of probalility of default.
        /// </summary>
        [GenericRating("BB"), DefaultProb(0.0093)]
        [Fitch("BB"), SnP("BB"), Moodys("Ba2")]
        [AKRA("AA-(RU)", "A+(RU)", "eAA-(RU)", "eA+(RU)", "AA-(ru.sf)", "A+(ru.sf)", "eAA-(ru.sf)", "eA+(ru.sf)")]
        [ExpRA("ruAA-", "ruA+", "ruAA-(EXP)", "ruA+(EXP)", "ruAA-.sf", "ruA+.sf", "ruAA-.sf(EXP)", "ruA+.sf(EXP)")]
        [NKR("AA-.ru", "A+.ru"), NRA("AA-|ru|", "A+|ru|")]
        BB = 11,

        /// <summary>
        ///     BB- generic rating. Corresponds to BB- rating from FITCH or SnP, Ba3 rating from Moody's, A(ru) or A-(RU) ratings 
        ///     from AKRA, ruA or ruA- ratings from ExpertRA, A.ru or A-.ru ratings from NKR and A|ru| or A-|ru| ratings from NRA.
        ///     Implies 1.55% of probalility of default.
        /// </summary>
        [GenericRating("BB-"), DefaultProb(0.0155)]
        [Fitch("BB-"), SnP("BB-"), Moodys("Ba3")]
        [AKRA("A(RU)", "A-(RU)", "eA(RU)", "eA-(RU)", "A(ru.sf)", "A-(ru.sf)", "eA(ru.sf)", "eA-(ru.sf)")]
        [ExpRA("ruA", "ruA-", "ruA(EXP)", "ruA-(EXP)", "ruA.sf", "ruA-.sf", "ruA.sf(EXP)", "ruA-.sf(EXP)")]
        [NKR("A.ru", "A-.ru"), NRA("A|ru|", "A-|ru|")]
        BBminus = 10,

        /// <summary>   
        ///     B+ generic rating. Corresponds to B+ rating from FITCH or SnP, B1 rating from Moody's, BBB+(ru) or BBB(RU) ratings 
        ///     from AKRA, ruBBB+ or ruBBB ratings from ExpertRA, BBB+.ru or BBB.ru ratings from NKR and BBB+|ru| or BBB|ru| ratings from NRA.
        ///     Implies 2.94% of probalility of default.
        /// </summary>
        [GenericRating("B+"), DefaultProb(0.0294)]
        [Fitch("B+"), SnP("B+"), Moodys("B1")]
        [AKRA("BBB+(RU)", "BBB(RU)", "eBBB+(RU)", "eBBB(RU)", "BBB+(ru.sf)", "BBB(ru.sf)", "eBBB+(ru.sf)", "eBBB(ru.sf)")]
        [ExpRA("ruBBB+", "ruBBB", "ruBBB+(EXP)", "ruBBB(EXP)", "ruBBB+.sf", "ruBBB.sf", "ruBBB+.sf(EXP)", "ruBBB.sf(EXP)")]
        [NKR("BBB+.ru", "BBB.ru"), NRA("BBB+|ru|", "BBB|ru|")]
        Bp = 9,

        /// <summary>   
        ///     B generic rating. Corresponds to B rating from FITCH or SnP, B2 rating from Moody's, BBB-(ru) or BB+(RU) ratings 
        ///     from AKRA, ruBBB- or ruBB+ ratings from ExpertRA, BBB-.ru or BB+.ru ratings from NKR and BBB-|ru| or BB+|ru| ratings from NRA.
        ///     Implies 4.66% of probalility of default.
        /// </summary>
        [GenericRating("B"), DefaultProb(0.0466)]
        [Fitch("B"), SnP("B"), Moodys("B2")]
        [AKRA("BBB-(RU)", "BB+(RU)", "eBBB-(RU)", "eBB+(RU)", "BBB-(ru.sf)", "BB+(ru.sf)", "eBBB-(ru.sf)", "eBB+(ru.sf)")]
        [ExpRA("ruBBB-", "ruBB+", "ruBBB-(EXP)", "ruBB+(EXP)", "ruBBB-.sf", "ruBB+.sf", "ruBBB-.sf(EXP)", "ruBB+.sf(EXP)")]
        [NKR("BBB-.ru", "BB+.ru"), NRA("BBB-|ru|", "BB+|ru|")]
        B = 8,

        /// <summary>   
        ///     B- generic rating. Corresponds to B- rating from FITCH or SnP, B3 rating from Moody's, BB(ru) rating
        ///     from AKRA, ruBB rating from ExpertRA, BB.ru rating from NKR and BB|ru| rating from NRA.
        ///     Implies 7.17% of probalility of default.
        /// </summary>
        [GenericRating("B-"), DefaultProb(0.0717)]
        [Fitch("B-"), SnP("B-"), Moodys("B3")]
        [AKRA("BB(RU)", "eBB(RU)", "BB(ru.sf)", "eBB(ru.sf)")]
        [ExpRA("ruBB", "ruBB(EXP)", "ruBB.sf", "ruBB.sf(EXP)")]
        [NKR("BB.ru"), NRA("BB|ru|")]
        Bminus = 7,

        /// <summary> 
        ///     CCC+ generic rating. Corresponds to CCC+ rating from FITCH or SnP, Caa1 rating from Moody's, BB-(ru) rating
        ///     from AKRA, ruBB- rating from ExpertRA, BB-.ru rating from NKR and BB-|ru| rating from NRA.
        ///     Implies 11.95% of probalility of default.
        /// </summary>
        [GenericRating("CCC+"), DefaultProb(0.1195)]
        [Fitch("CCC+"), SnP("CCC+"), Moodys("Caa1")]
        [AKRA("BB-(RU)", "eBB-(RU)", "BB-(ru.sf)", "eBB-(ru.sf)")]
        [ExpRA("ruBB-", "ruBB-(EXP)", "ruBB-.sf", "ruBB-.sf(EXP)")]
        [NKR("BB-.ru"), NRA("BB-|ru|")]
        CCCplus = 6,

        /// <summary> 
        ///     CCC generic rating. Corresponds to CCC rating from FITCH or SnP, Caa2 rating from Moody's, B+(ru) rating
        ///     from AKRA, ruB+ rating from ExpertRA, B+.ru rating from NKR and B+|ru| rating from NRA.
        ///     Implies 19.91% of probalility of default.
        /// </summary>
        [GenericRating("CCC"), DefaultProb(0.1991)]
        [Fitch("CCC"), SnP("CCC"), Moodys("Caa2")]
        [AKRA("B+(RU)", "eB+(RU)", "B+(ru.sf)", "eB+(ru.sf)")]
        [ExpRA("ruB+", "ruB+(EXP)", "ruB+.sf", "ruB+.sf(EXP)")]
        [NKR("B+.ru"), NRA("B+|ru|")]
        CCC = 5,

        /// <summary> 
        ///     CCC- generic rating. Corresponds to CCC- rating from FITCH or SnP, Caa3 rating from Moody's, B(ru) rating
        ///     from AKRA, ruB rating from ExpertRA, B.ru rating from NKR and B|ru| rating from NRA.
        ///     Implies 42.97% of probalility of default.
        /// </summary>
        [GenericRating("CCC-"), DefaultProb(0.4297)]
        [Fitch("CCC-"), SnP("CCC-"), Moodys("Caa3")]
        [AKRA("B(RU)", "eB(RU)", "B(ru.sf)", "eB(ru.sf)")]
        [ExpRA("ruB", "ruB(EXP)", "ruB.sf", "ruB(EXP).sf")]
        [NKR("B.ru"), NRA("B|ru|")]
        CCCminus = 4,

        /// <summary> 
        ///     CC generic rating. Corresponds to CC rating from FITCH or SnP, Ca rating from Moody's, B-(ru) rating
        ///     from AKRA, ruB- rating from ExpertRA, B-.ru rating from NKR and B-|ru| rating from NRA.
        ///     Implies 77.64% of probalility of default.
        /// </summary>
        [GenericRating("CC"), DefaultProb(0.7764)]
        [Fitch("CC"), SnP("CC"), Moodys("Ca")]
        [AKRA("B-(RU)", "eB-(RU)", "B-(ru.sf)", "eB-(ru.sf)")]
        [ExpRA("ruB-", "ruB-(EXP)", "ruB-.sf", "ruB-.sf(EXP)")]
        [NKR("B-.ru"), NRA("B-|ru|")]
        CC = 3,

        /// <summary>   
        ///     C generic rating. Corresponds to C rating from FITCH or SnP, C rating from Moody's, CCC(ru) or CC(RU) or C(RU) ratings 
        ///     from AKRA, ruCCC or ruCC or ruC ratings from ExpertRA, CCC.ru or CC.ru or C.ru ratings from NKR and CCC|ru| or CC|ru| or C|ru| ratings from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("C"), DefaultProb(0.9662)]
        [Fitch("C"), SnP("C"), Moodys("C")]
        [AKRA("CCC(RU)", "CC(RU)", "C(RU)", "eCCC(RU)", "eCC(RU)", "eC(RU)", "CCC(ru.sf)", "CC(ru.sf)", "C(ru.sf)", "eCCC(ru.sf)", "eCC(ru.sf)", "eC(ru.sf)")]
        [ExpRA("ruCCC", "ruCC", "ruC", "ruCCC(EXP)", "ruCC(EXP)", "ruC(EXP)", "ruCCC.sf", "ruCC.sf", "ruC,sf", "ruCCC.sf(EXP)", "ruCC.sf(EXP)", "ruC.sf(EXP)")]
        [NKR("CCC.ru", "CC.ru", "C.ru"), NRA("CCC|ru|", "CC|ru|", "C|ru|")]
        C = 2,

        /// <summary> 
        ///     D generic rating. Corresponds to D rating from FITCH or SnP, D(ru) rating
        ///     from AKRA, ruD rating from ExpertRA, D.ru rating from NKR and D|ru| rating from NRA.
        ///     Implies 99.50% of probalility of default.
        /// </summary>
        [GenericRating("D"), DefaultProb(0.9950)]
        [Fitch("D"), SnP("D")] 
        [AKRA("D(RU)", "eD(RU)", "D(ru.sf)", "eD(ru.sf)")]
        [ExpRA("ruD", "ruD(EXP)", "ruD.sf", "ruD.sf(EXP)")]
        [NKR("D.ru"), NRA("D|ru|")]
        D = 1,
    }
}
