using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.RuData;
using RuDataAPI.Extensions.Mapping;
using RuDataAPI.Extensions.Ratings;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Provides additional methods to get different sort of market data using EFIR.DataHub services.
    /// </summary>
    public static class EfirExtensions
    {
        // cach variables
        private static readonly Dictionary<DateTime, GCurveOFZResponse> _gcparams = new();
        private static readonly Dictionary<string, CreditRating[]> _ratings = new();

        /// <summary>
        ///     Calculates current MOEX G-Curve rate for specified tenor.
        /// </summary>
        /// <param name="tenor">MOEX yield curve tenor in years.</param>
        /// <remarks>
        ///     For more details about GCurve construction methodology see <see href="https://www.moex.com/s2532">
        ///             MOEX G-Curve reference page</see>.    
        /// </remarks>
        public static async Task<double> CalculateGcurveForDateAsync(this EfirClient client, double tenor)
        {
            return await client.CalculateGcurveForDateAsync(DateTime.Now, tenor);
        }


        /// <summary>
        ///     Calculates MOEX G-Curve rate for specified tenor and date.
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
            if (_gcparams.ContainsKey(date) is false)
                _gcparams.Add(date, await client.GetGcurveParametersAsync(date.Date));            

            // these constants are specified according to MOEX (https://www.moex.com/s2532)
            double k = 1.6, a1 = .0, a2 = .6;

            double beta0 = _gcparams[date].beta0val.HasValue
                ? (double)_gcparams[date].beta0val!.Value
                : throw new Exception($"GCurve BETA0 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta1 = _gcparams[date].beta1val.HasValue
                ? (double)_gcparams[date].beta1val!.Value
                : throw new Exception($"GCurve BETA1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double beta2 = _gcparams[date].beta2val.HasValue
                ? (double)_gcparams[date].beta2val!.Value
                : throw new Exception($"GCurve BETA2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double tau = _gcparams[date].tauval.HasValue
                ? (double)_gcparams[date].tauval!.Value
                : throw new Exception($"GCurve TAU param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g1 = _gcparams[date].g1val.HasValue
                ? (double)_gcparams[date].g1val!.Value
                : throw new Exception($"GCurve g1 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g2 = _gcparams[date].g2val.HasValue
                ? (double)_gcparams[date].g2val!.Value
                : throw new Exception($"GCurve g2 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g3 = _gcparams[date].g3val.HasValue
                ? (double)_gcparams[date].g3val!.Value
                : throw new Exception($"GCurve g3 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g4 = _gcparams[date].g4val.HasValue
                ? (double)_gcparams[date].g4val!.Value
                : throw new Exception($"GCurve g4 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g5 = _gcparams[date].g5val.HasValue
                ? (double)_gcparams[date].g5val!.Value
                : throw new Exception($"GCurve g5 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g6 = _gcparams[date].g6val.HasValue
                ? (double)_gcparams[date].g6val!.Value
                : throw new Exception($"GCurve g6 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g7 = _gcparams[date].g7val.HasValue
                ? (double)_gcparams[date].g7val!.Value
                : throw new Exception($"GCurve g7 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g8 = _gcparams[date].g8val.HasValue
                ? (double)_gcparams[date].g8val!.Value
                : throw new Exception($"GCurve g8 param is null when trying to calculate G-Curve value for {date:dd.MM.yyyy}.");

            double g9 = _gcparams[date].g9val.HasValue
                ? (double)_gcparams[date].g9val!.Value
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

        /// <summary>
        ///     Returns ratings history for specified issuer or its security.
        /// </summary>
        /// <param name="innOrIsin">Issuer INN or its ISIN.</param>
        /// <returns>Array of <see cref="CreditRating"/>.</returns>
        public static async Task<CreditRating[]> GetAllRatingsAsync(this EfirClient client, string innOrIsin)
        {
            if (_ratings.ContainsKey(innOrIsin))
                return _ratings[innOrIsin];

            var data = await client.GetRatingHistoryAsync(innOrIsin);   
            CreditRating[] ratings = new CreditRating[data.Length];
            for (int i = 0; i < ratings.Length; i++)
                ratings[i] = CreditRating.New(data[i]);

            // caching
            _ratings.Add(innOrIsin, ratings);

            return ratings;
        }
    }
}
