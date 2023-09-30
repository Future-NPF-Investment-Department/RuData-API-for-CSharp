using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.RuData;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Provides additional methods to get different sort of market data using EFIR.DataHub services.
    /// </summary>
    public static class EfirExtensions
    {
        // cach variables
        private static DateTime? _date;
        private static GCurveOFZResponse? _params;

        /// <summary>
        ///     Calculates MOEX G-Curve rate for specified tenor.
        /// </summary>
        /// <param name="date">date of MOEX yield curve.</param>
        /// <param name="tenor">MOEX yield curve tenor in years.</param>
        /// <remarks>
        ///     For more details about GCurve construction methodology see <see href="https://www.moex.com/s2532">
        ///             MOEX G-Curve reference page</see>.    
        /// </remarks>
        public static async Task<double> CalculateGcurveForDateAsync(this EfirClient client, DateTime date, double tenor)
        {
            // caching
            if (_params is null || _date != date)
            {
                _date = date;
                _params = await client.GetGcurveParametersAsync(date.Date);
            }

            // these constants are specified according to MOEX (https://www.moex.com/s2532)
            double k = 1.6, a1 = .0, a2 = .6;

            double beta0 = _params.beta0val.HasValue
                ? (double)_params.beta0val.Value
                : throw new Exception($"GCurve BETA0 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta1 = _params.beta1val.HasValue
                ? (double)_params.beta1val.Value
                : throw new Exception($"GCurve BETA1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta2 = _params.beta2val.HasValue
                ? (double)_params.beta2val.Value
                : throw new Exception($"GCurve BETA2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double tau = _params.tauval.HasValue
                ? (double)_params.tauval.Value
                : throw new Exception($"GCurve TAU param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g1 = _params.g1val.HasValue
                ? (double)_params.g1val.Value
                : throw new Exception($"GCurve g1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g2 = _params.g2val.HasValue
                ? (double)_params.g2val.Value
                : throw new Exception($"GCurve g2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g3 = _params.g3val.HasValue
                ? (double)_params.g3val.Value
                : throw new Exception($"GCurve g3 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g4 = _params.g4val.HasValue
                ? (double)_params.g4val.Value
                : throw new Exception($"GCurve g4 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g5 = _params.g5val.HasValue
                ? (double)_params.g5val.Value
                : throw new Exception($"GCurve g5 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g6 = _params.g6val.HasValue
                ? (double)_params.g6val.Value
                : throw new Exception($"GCurve g6 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g7 = _params.g7val.HasValue
                ? (double)_params.g7val.Value
                : throw new Exception($"GCurve g7 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g8 = _params.g8val.HasValue
                ? (double)_params.g8val.Value
                : throw new Exception($"GCurve g8 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g9 = _params.g9val.HasValue
                ? (double)_params.g9val.Value
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

            for (int i = 1; i <= 8; i++)
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

            return Math.Exp(gt / 10000) - 1;
        }

        /// <summary>
        ///     Fetch static security data from EFIR server.
        /// </summary>
        /// <param name="Isin">Security ISIN code.</param>
        /// <param name="loadCoupons">Flag to load coupons for bond.</param>
        /// <returns><see cref="EfirSecurity"/></returns>
        public static async Task<EfirSecurity> GetEfirSecurityAsync(this EfirClient client, string Isin, bool loadCoupons = false)
        {
            FintoolReferenceDataFields[] data = await client.GetSecurityDataAsync(Isin);
            if (data.Length == 0)
                throw new Exception($"EFIR: no securities found for ISIN: {Isin}");

            var sec = new EfirSecurity(data[0]);

            if (loadCoupons && sec.SecurityId is not null && sec.AssetClass is "Облигация")
            {
                sec.EventsSchedule = new List<SecurityEvent>();
                var events = await client.GetEventsCalendarAsync(sec.SecurityId.Value);
                if (events.Length > 0)
                    foreach (var coupon in events)
                        sec.EventsSchedule.Add(new SecurityEvent(coupon));
            }
            return sec;
        }
    }
}
