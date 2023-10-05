
namespace RuDataAPI.Extensions.Ratings
{
    /// <summary>
    ///     Aggregated credit rating. Based on international rating scale.
    /// </summary>
    public enum RatingUS
    {
        [RatingStr("NR")]
        [DefaultProb(0.0034)]
        [Fitch("Снят")]
        [SnP("Снят")]
        [Moodys("Снят")]
        [AKRA("Снят")]
        [ExpRA("Снят")]
        [NKR("Снят")]
        [NRA("Снят")]
        NR = 0,

        [RatingStr("AAA")]
        [DefaultProb(0.00)]
        [Fitch("AAA")]
        [SnP("AAA")]
        [Moodys("Aaa")]
        AAA = 22,

        [RatingStr("AA+")]
        [DefaultProb(0.0001)]
        [Fitch("AA+")]
        [SnP("AA+")]
        [Moodys("Aa1")]
        AAplus = 21,

        [RatingStr("AA")]
        [DefaultProb(0.0001)]
        [Fitch("AA")]
        [SnP("AA")]
        [Moodys("Aa2")]
        AA = 20,

        [RatingStr("AA-")]
        [DefaultProb(0.0002)]
        [Fitch("AA-")]
        [SnP("AA-")]
        [Moodys("Aa3")]
        AAminus = 19,

        [RatingStr("A+")]
        [DefaultProb(0.0003)]
        [Fitch("A+")]
        [SnP("A+")]
        [Moodys("A1")]
        Aplus = 18,

        [RatingStr("A")]
        [DefaultProb(0.0004)]
        [Fitch("A")]
        [SnP("A")]
        [Moodys("A2")]
        A = 17,

        [RatingStr("A-")]
        [DefaultProb(0.0007)]
        [Fitch("A-")]
        [SnP("A-")]
        [Moodys("A3")]
        Aminus = 16,

        [RatingStr("BBB+")]
        [DefaultProb(0.0012)]
        [Fitch("BBB+")]
        [SnP("BBB+")]
        [Moodys("Baa1")]
        BBBplus = 15,

        [RatingStr("BBB")]
        [DefaultProb(0.0020)]
        [Fitch("BBB")]
        [SnP("BBB")]
        [Moodys("Baa2")]
        BBB = 14,

        [RatingStr("BBB-")]
        [DefaultProb(0.0034)]
        [Fitch("BBB-")]
        [SnP("BBB-")]
        [Moodys("Baa3")]
        [AKRA("AAA(RU)")]
        [ExpRA("ruAAA")]
        [NKR("AAA.ru")]
        [NRA("AAA|ru|")]
        BBBminus = 13,

        [RatingStr("BB+")]
        [DefaultProb(0.0056)]
        [Fitch("BB+")]
        [SnP("BB+")]
        [Moodys("Ba1")]
        [AKRA("AA+(RU)", "AA(RU)")]
        [ExpRA("ruAA+", "ruAA")]
        [NKR("AA+.ru", "AA.ru")]
        [NRA("AA+|ru|", "AA|ru|")]
        BBplus = 12,

        [RatingStr("BB")]
        [DefaultProb(0.0093)]
        [Fitch("BB")]
        [SnP("BB")]
        [Moodys("Ba2")]
        [AKRA("AA-(RU)", "A+(RU)")]
        [ExpRA("ruAA-", "ruA+")]
        [NKR("AA-.ru", "A+.ru")]
        [NRA("AA-|ru|", "A+|ru|")]
        BB = 11,

        [RatingStr("BB-")]
        [DefaultProb(0.0155)]
        [Fitch("BB-")]
        [SnP("BB-")]
        [Moodys("Ba3")]
        [AKRA("A(RU)", "A-(RU)")]
        [ExpRA("ruA", "ruA-")]
        [NKR("A.ru", "A-.ru")]
        [NRA("A|ru|", "A-|ru|")]
        BBminus = 10,

        [RatingStr("B+")]
        [DefaultProb(0.0294)]
        [Fitch("B+")]
        [SnP("B+")]
        [Moodys("B1")]
        [AKRA("BBB+(RU)", "BBB(RU)")]
        [ExpRA("ruBBB+", "ruBBB")]
        [NKR("BBB+.ru", "BBB.ru")]
        [NRA("BBB+|ru|", "BBB|ru|")]
        Bp = 9,

        [RatingStr("B")]
        [DefaultProb(0.0466)]
        [Fitch("B")]
        [SnP("B")]
        [Moodys("B2")]
        [AKRA("BBB-(RU)", "BB+(RU)")]
        [ExpRA("ruBBB-", "ruBB+")]
        [NKR("BBB-.ru", "BB+.ru")]
        [NRA("BBB-|ru|", "BB+|ru|")]
        B = 8,

        [RatingStr("B-")]
        [DefaultProb(0.0717)]
        [Fitch("B-")]
        [SnP("B-")]
        [Moodys("B3")]
        [AKRA("BB(RU)")]
        [ExpRA("ruBB")]
        [NKR("BB.ru")]
        [NRA("BB|ru|")]
        Bminus = 7,

        [RatingStr("CCC+")]
        [DefaultProb(0.1195)]
        [Fitch("CCC+")]
        [SnP("CCC+")]
        [Moodys("Caa1")]
        [AKRA("BB-(RU)")]
        [ExpRA("ruBB-")]
        [NKR("BB-.ru")]
        [NRA("BB-|ru|")]
        CCCplus = 6,

        [RatingStr("CCC")]
        [DefaultProb(0.1991)]
        [Fitch("CCC")]
        [SnP("CCC")]
        [Moodys("Caa2")]
        [AKRA("B+(RU)")]
        [ExpRA("ruB+")]
        [NKR("B+.ru")]
        [NRA("B+|ru|")]
        CCC = 5,

        [RatingStr("CCC-")]
        [DefaultProb(0.4297)]
        [Fitch("CCC-")]
        [SnP("CCC-")]
        [Moodys("Caa3")]
        [AKRA("B(RU)")]
        [ExpRA("ruB")]
        [NKR("B.ru")]
        [NRA("B|ru|")]
        CCCminus = 4,

        [RatingStr("CC")]
        [DefaultProb(0.7764)]
        [Fitch("CC")]
        [SnP("CC")]
        [Moodys("Ca")]
        [AKRA("B-(RU)")]
        [ExpRA("ruB-")]
        [NKR("B-.ru")]
        [NRA("B-|ru|")]
        CC = 3,

        [RatingStr("C")]
        [DefaultProb(0.9662)]
        [Fitch("C")]
        [SnP("C")]
        [Moodys("C")]
        [AKRA("CCC(RU)", "CC(RU)", "C(RU)")]
        [ExpRA("ruCCC", "ruCC", "ruC")]
        [NKR("CCC.ru", "CC.ru", "C.ru")]
        [NRA("CCC|ru|", "CC|ru|", "C|ru|")]
        C = 2,

        [RatingStr("D")]
        [DefaultProb(0.9950)]
        [Fitch("D")]
        [SnP("D")]
        [AKRA("D(RU)")]
        [ExpRA("ruD")]
        [NKR("D.ru")]
        [NRA("D|ru|")]
        D = 1,
    }
}
