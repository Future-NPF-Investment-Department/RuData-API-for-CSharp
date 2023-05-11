using Efir.DataHub.Models.Models.RuData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Provides additional methods to get different sort of market data using EFIR.DataHub services.
    /// </summary>
    public static class EfirExtensions
    {
        /// <summary>
        ///     Calculates MOEX G-Curve rate for specified tenor.
        /// </summary>
        /// <param name="date">date of MOEX yield curve.</param>
        /// <param name="tenor">MOEX yield curve tenor in years.</param>
        /// <remarks>
        ///     For more details about GCurve construction methodology see <see href="https://www.moex.com/s2532">
        ///             MOEX G-Curve reference page</see>.    
        /// </remarks>
        public static async Task<double> CalculateGcurveForDate(this EfirClient client, DateTime date, double tenor)
        {
            GCurveOFZResponse curveParams = await client.GetGcurveParameters(date);

            // these constants are specified according to MOEX (https://www.moex.com/s2532)
            double k = 1.6, a1 = .0, a2 = .6;

            double beta0 = curveParams.beta0val.HasValue 
                ? (double)curveParams.beta0val.Value 
                : throw new Exception($"GCurve BETA0 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta1 = curveParams.beta1val.HasValue
                ? (double)curveParams.beta1val.Value
                : throw new Exception($"GCurve BETA1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta2 = curveParams.beta2val.HasValue
                ? (double)curveParams.beta2val.Value
                : throw new Exception($"GCurve BETA2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double tau = curveParams.tauval.HasValue
                ? (double)curveParams.tauval.Value
                : throw new Exception($"GCurve TAU param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g1 = curveParams.g1val.HasValue
                ? (double)curveParams.g1val.Value
                : throw new Exception($"GCurve g1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g2 = curveParams.g2val.HasValue
                ? (double)curveParams.g2val.Value
                : throw new Exception($"GCurve g2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g3 = curveParams.g3val.HasValue
                ? (double)curveParams.g3val.Value
                : throw new Exception($"GCurve g3 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g4 = curveParams.g4val.HasValue
                ? (double)curveParams.g4val.Value
                : throw new Exception($"GCurve g4 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g5 = curveParams.g5val.HasValue
                ? (double)curveParams.g5val.Value
                : throw new Exception($"GCurve g5 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g6 = curveParams.g6val.HasValue
                ? (double)curveParams.g6val.Value
                : throw new Exception($"GCurve g6 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g7 = curveParams.g7val.HasValue
                ? (double)curveParams.g7val.Value
                : throw new Exception($"GCurve g7 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g8 = curveParams.g8val.HasValue
                ? (double)curveParams.g8val.Value
                : throw new Exception($"GCurve g8 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g9 = curveParams.g9val.HasValue
                ? (double)curveParams.g9val.Value
                : throw new Exception($"GCurve g9 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            //                                  0   1   2   3   4   5   6   7   8
            double[] aCoeffs = new double[9] { a1, a2, .0, .0, .0, .0, .0, .0, .0 }; // array of a's
            double[] bCoeffs = new double[9] { a2, .0, .0, .0, .0, .0, .0, .0, .0 }; // array of b's
            double[] gCoeffs = new double[9] { g1, g2, g3, g4, g5, g6, g7, g8, g9 }; // array of g's

            // filling adjust coefficients
            for (int i = 2; i <= 8; i++)
            {
                double previous = aCoeffs[i - 1];
                aCoeffs[i] = previous + a2 * Math.Pow(k, i - 1);
            }

            for(int i = 1; i <= 8; i++)
            {
                bCoeffs[i] = bCoeffs[i - 1] * k;
            }

            // original Nelson-Siegel formula:
            double gt = beta0
                + (beta1 + beta2) * tau / tenor * (1 - Math.Exp(-tenor / tau))
                - beta2 * Math.Exp(-tenor / tau);

            // adding adjustment components to Nelson-Siegel original formaula:
            for (int i = 0; i <= 8; i++)
            {
                gt += gCoeffs[i] * Math.Exp(-(Math.Pow(tenor - aCoeffs[i], 2) / Math.Pow(bCoeffs[i], 2)));
            }

            return Math.Exp(gt/10000)-1;
        }
    }
}
