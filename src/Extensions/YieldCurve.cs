
namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents yield curve 
    /// </summary>
    public readonly struct YieldCurve
    {
        public DateTime Date { get; init; }
        public CurveProvider Provider { get; init; }
        public double Tau1 { get; init; }
        public double Tau2 { get; init; }
        public double Beta0 { get; init; }   
        public double Beta1 { get; init; }
        public double Beta2 { get; init;}
        public double Beta3 { get; init;}
        public double G1 { get; init; }
        public double G2 { get; init; }
        public double G3 { get; init; }
        public double G4 { get; init; }
        public double G5 { get; init; }
        public double G6 { get; init; }
        public double G7 { get; init; }
        public double G8 { get; init; }
        public double G9 { get; init; }

        public double GetValueForTenor(double tenor) => Provider switch
        {
            CurveProvider.MOEX => GetValueForTenorMOEX(tenor),
            _ => .0
        };

        private double GetValueForTenorMOEX(double tenor)
        {
            // these constants are specified according to MOEX (https://www.moex.com/s2532)
            double k = 1.6, a1 = .0, a2 = .6;

            double[] aCoeffs = new double[9] { a1, a2, .0, .0, .0, .0, .0, .0, .0 }; // array of a's
            double[] bCoeffs = new double[9] { a2, .0, .0, .0, .0, .0, .0, .0, .0 }; // array of b's
            double[] gCoeffs = new double[9] { G1, G2, G3, G4, G5, G6, G7, G8, G9 }; // array of g's

            // filling adjust coefficients
            for (int i = 2; i <= 8; i++)
                aCoeffs[i] = aCoeffs[i - 1] + a2 * Math.Pow(k, i - 1);

            for (int i = 1; i <= 8; i++)
                bCoeffs[i] = bCoeffs[i - 1] * k;

            // original Nelson-Siegel formula:
            double gt = Beta0
                + (Beta1 + Beta2) * Tau1 / tenor * (1 - Math.Exp(-tenor / Tau1))
                - Beta2 * Math.Exp(-tenor / Tau1);

            // adding adjustment components to Nelson-Siegel original formaula:
            for (int i = 0; i <= 8; i++)
                gt += gCoeffs[i] * Math.Exp(-(Math.Pow(tenor - aCoeffs[i], 2) / Math.Pow(bCoeffs[i], 2)));

            return Math.Exp(gt / 10000) - 1;
        }    
    }

    public enum CurveProvider
    {
        None,
        MOEX,
        ECB,
        FRS
    }
}
