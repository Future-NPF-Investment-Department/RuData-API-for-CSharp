
namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents yield curve. 
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

        /// <summary>
        ///     Calculates curve value for specified tenor.
        /// </summary>
        /// <param name="tenor">Time tenor</param>
        public double GetValueForTenor(Tenor tenor) => Provider switch
        {
            CurveProvider.MOEX => GetValueForTenorMOEX(tenor),
            _ => .0
        };

        /// <summary>
        ///     calculates forward interest rate.
        /// </summary>
        /// <param name="t1">Tenor of r to calculate.</param>
        /// <param name="dt">Forward period.</param>        
        public double GetForwardValueForTenor(Tenor t1, Tenor dt)
        {            
            double r1 = GetValueForTenor(t1);           // rate for tenor t1 
            double t2 = t1.Years + dt.Years;            // tenor t2 = t1 + dt 
            double r2 = GetValueForTenor(t2);           // rate for tenor t2
            double accum                                // accumulation func a(t)
                = Math.Pow(1 + r2, t2)                  // a(t) = (1 + r)^t 
                / Math.Pow(1 + r1, t1.Years);           // a(t) = (1 + r2)^t2 / (1 + r1)^t1    
            return Math.Pow(accum, 1 / dt.Years) - 1;   // return forward rate            
        }

        private double GetValueForTenorMOEX(Tenor tenor)
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
                + (Beta1 + Beta2) * Tau1 / tenor.Years * (1 - Math.Exp(-tenor.Years / Tau1))
                - Beta2 * Math.Exp(-tenor.Years / Tau1);

            // adding adjustment components to Nelson-Siegel original formaula:
            for (int i = 0; i <= 8; i++)
                gt += gCoeffs[i] * Math.Exp(-(Math.Pow(tenor.Years - aCoeffs[i], 2) / Math.Pow(bCoeffs[i], 2)));

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
