
namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Russian generic rating scale values.
    /// </summary>
    public enum CreditRatingRU
    {
        /// <summary>
        ///     Rating absence.
        /// </summary>
        [GenericRating("NR")]
        [AKRA("Снят"), ExpRA("Снят"), NKR("Снят"), NRA("Снят")]
        NR = 0,

        /// <summary>
        ///     AAA generic rating within Russian rating scale. Corresponds to AAA(ru) rating from AKRA, ruAAA rating
        ///     from ExpertRA, AAA.ru rating from NKR and AAA|ru| rating from NRA.
        ///     Implies 0.34% of probalility of default.
        /// </summary>
        [GenericRating("AAA"), DefaultProb(0.0034)]
        [AKRA("AAA(RU)"), ExpRA("ruAAA"), NKR("AAA.ru"), NRA("AAA|ru|")]
        AAA = 22,

        /// <summary>
        ///     AA+ generic rating within Russian rating scale. Corresponds to AA+(ru) rating from AKRA, ruAA+ rating
        ///     from ExpertRA, AA+.ru rating from NKR and AA+|ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("AA+"), DefaultProb(0.0056)]
        [AKRA("AA+(RU)"), ExpRA("ruAA+"), NKR("AA+.ru"), NRA("AA+|ru|")]
        AAplus = 21,

        /// <summary>
        ///      generic rating within Russian rating scale. Corresponds to (ru) rating from AKRA, ru rating
        ///     from ExpertRA, .ru rating from NKR and |ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("AA"), DefaultProb(0.0056)]
        [AKRA("AA(RU)"), ExpRA("ruAA"), NKR("AA.ru"), NRA("AA|ru|")]
        AA = 20,

        /// <summary>
        ///     AA- generic rating within Russian rating scale. Corresponds to AA-(ru) rating from AKRA, ruAA- rating
        ///     from ExpertRA, AA-.ru rating from NKR and AA-|ru| rating from NRA.
        ///     Implies 0.93% of probalility of default.
        /// </summary>
        [GenericRating("AA-"), DefaultProb(0.0093)]
        [AKRA("AA-(RU)"), ExpRA("ruAA-"), NKR("AA-.ru"), NRA("AA-|ru|")]
        AAminus = 19,

        /// <summary>
        ///     A+ generic rating within Russian rating scale. Corresponds to A+(ru) rating from AKRA, ruA+ rating
        ///     from ExpertRA, A+.ru rating from NKR and A+|ru| rating from NRA.
        ///     Implies 0.93% of probalility of default.
        /// </summary>
        [GenericRating("A+"), DefaultProb(0.0093)]
        [AKRA("A+(RU)"), ExpRA("ruA+"), NKR("A+.ru"), NRA("A+|ru|")]
        Aplus = 18,

        /// <summary>
        ///     A generic rating within Russian rating scale. Corresponds to A(ru) rating from AKRA, ruA rating
        ///     from ExpertRA, A.ru rating from NKR and A|ru| rating from NRA.
        ///     Implies 1.55% of probalility of default.
        /// </summary>
        [GenericRating("A"), DefaultProb(0.0155)]
        [AKRA("A(RU)"), ExpRA("ruA"), NKR("A.ru"), NRA("A|ru|")]
        A = 17,

        /// <summary>
        ///     A- generic rating within Russian rating scale. Corresponds to A-(ru) rating from AKRA, ruA- rating
        ///     from ExpertRA, A-.ru rating from NKR and A-|ru| rating from NRA.
        ///     Implies 1.55% of probalility of default.
        /// </summary>
        [GenericRating("A-"), DefaultProb(0.0155)]
        [AKRA("A-(RU)"), ExpRA("ruA-"), NKR("A-.ru"), NRA("A-|ru|")]
        Aminus = 16,

        /// <summary>
        ///     BBB+ generic rating within Russian rating scale. Corresponds to BBB+(ru) rating from AKRA, ruBBB+ rating
        ///     from ExpertRA, BBB+.ru rating from NKR and BBB+|ru| rating from NRA.
        ///     Implies 2.94% of probalility of default.
        /// </summary>
        [GenericRating("BBB+"), DefaultProb(0.0294)]
        [AKRA("BBB+(RU)"), ExpRA("ruBBB+"), NKR("BBB+.ru"), NRA("BBB+|ru|")]
        BBBplus = 15,

        /// <summary>
        ///     BBB generic rating within Russian rating scale. Corresponds to BBB(ru) rating from AKRA, ruBBB rating
        ///     from ExpertRA, BBB.ru rating from NKR and BBB|ru| rating from NRA.
        ///     Implies 2.94% of probalility of default.
        /// </summary>
        [GenericRating("BBB"), DefaultProb(0.0294)]
        [AKRA("BBB(RU)"), ExpRA("ruBBB"), NKR("BBB.ru"), NRA("BBB|ru|")]
        BBB = 14,

        /// <summary>
        ///     BBB- generic rating within Russian rating scale. Corresponds to BBB-(ru) rating from AKRA, ruBBB- rating
        ///     from ExpertRA, BBB-.ru rating from NKR and BBB-|ru| rating from NRA.
        ///     Implies 4.66% of probalility of default.
        /// </summary>
        [GenericRating("BBB-"), DefaultProb(0.0466)]
        [AKRA("BBB-(RU)"), ExpRA("ruBBB-"), NKR("BBB-.ru"), NRA("BBB-|ru|")]
        BBBminus = 13,

        /// <summary>
        ///     BB+ generic rating within Russian rating scale. Corresponds to BB+(ru) rating from AKRA, ruBB+ rating
        ///     from ExpertRA, BB+.ru rating from NKR and BB+|ru| rating from NRA.
        ///     Implies 4.66% of probalility of default.
        /// </summary>
        [GenericRating("BB+"), DefaultProb(0.0466)]
        [AKRA("BB+(RU)"), ExpRA("ruBB+"), NKR("BB+.ru"), NRA("BB+|ru|")]
        BBplus = 12,

        /// <summary>
        ///     BB generic rating within Russian rating scale. Corresponds to BB(ru) rating from AKRA, ruBB rating
        ///     from ExpertRA, BB.ru rating from NKR and BB|ru| rating from NRA.
        ///     Implies 7.17% of probalility of default.
        /// </summary>
        [GenericRating("BB"), DefaultProb(0.0717)]
        [AKRA("BB(RU)"), ExpRA("ruBB"), NKR("BB.ru"), NRA("BB|ru|")]
        BB = 11,

        /// <summary>
        ///     BB- generic rating within Russian rating scale. Corresponds to BB-(ru) rating from AKRA, ruBB- rating
        ///     from ExpertRA, BB-.ru rating from NKR and BB-|ru| rating from NRA.
        ///     Implies 11.95% of probalility of default.
        /// </summary>
        [GenericRating("BB-"), DefaultProb(0.1195)]
        [AKRA("BB-(RU)"), ExpRA("ruBB-"), NKR("BB-.ru"), NRA("BB-|ru|")]
        BBminus = 10,

        /// <summary>
        ///     B+ generic rating within Russian rating scale. Corresponds to B+(ru) rating from AKRA, ruB+ rating
        ///     from ExpertRA, B+.ru rating from NKR and B+|ru| rating from NRA.
        ///     Implies 19.91% of probalility of default.
        /// </summary>
        [GenericRating("B+"), DefaultProb(0.1991)]
        [AKRA("B+(RU)"), ExpRA("ruB+"), NKR("B+.ru"), NRA("B+|ru|")]
        Bp = 9,

        /// <summary>
        ///     B generic rating within Russian rating scale. Corresponds to B(ru) rating from AKRA, ruB rating
        ///     from ExpertRA, B.ru rating from NKR and B|ru| rating from NRA.
        ///     Implies 4297% of probalility of default.
        /// </summary>
        [GenericRating("B"), DefaultProb(0.4297)]
        [AKRA("B(RU)"), ExpRA("ruB"), NKR("B.ru"), NRA("B|ru|")]
        B = 8,

        /// <summary>
        ///     B- generic rating within Russian rating scale. Corresponds to B-(ru) rating from AKRA, ruB- rating
        ///     from ExpertRA, B-.ru rating from NKR and B-|ru| rating from NRA.
        ///     Implies 7764% of probalility of default.
        /// </summary>
        [GenericRating("B-"), DefaultProb(0.7764)]
        [AKRA("B-(RU)"), ExpRA("ruB-"), NKR("B-.ru"), NRA("B-|ru|")]
        Bminus = 7,

        /// <summary>
        ///     CCC generic rating within Russian rating scale. Corresponds to CCC(ru) rating from AKRA, ruCCC rating
        ///     from ExpertRA, CCC.ru rating from NKR and CCC|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("CCC"), DefaultProb(0.9662)]
        [AKRA("CCC(RU)"), ExpRA("ruCCC"), NKR("CCC.ru"), NRA("CCC|ru|")]
        CCC = 5,

        /// <summary>
        ///     CC generic rating within Russian rating scale. Corresponds to CC(ru) rating from AKRA, ruCC rating
        ///     from ExpertRA, CC.ru rating from NKR and CC|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("CC"), DefaultProb(0.9662)]
        [AKRA("CC(RU)"), ExpRA("ruCC"), NKR("CC.ru"), NRA("CC|ru|")]
        CC = 3,

        /// <summary>
        ///     C generic rating within Russian rating scale. Corresponds to C(ru) rating from AKRA, ruC rating
        ///     from ExpertRA, C.ru rating from NKR and C|ru| rating from NRA.
        ///     Implies 96.62% of probalility of default.
        /// </summary>
        [GenericRating("C"), DefaultProb(0.9662)]
        [AKRA("C(RU)"), ExpRA("ruC"), NKR("C.ru"), NRA("C|ru|")]
        C = 2,

        /// <summary>
        ///     D generic rating within Russian rating scale. Corresponds to D(ru) rating from AKRA, ruD rating
        ///     from ExpertRA, D.ru rating from NKR and D|ru| rating from NRA.
        ///     Implies 0.56% of probalility of default.
        /// </summary>
        [GenericRating("D"), DefaultProb(0.9950)]
        [AKRA("D(RU)"), ExpRA("ruD"), NKR("D.ru"), NRA("D|ru|")]
        D = 1,
    }
}
