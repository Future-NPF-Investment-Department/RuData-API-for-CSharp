using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.RuData;
using RuDataAPI.Extensions.Mapping;
using RuDataAPI.Extensions.Ratings;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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
        public static async Task<double> ExCalculateGcurveForDateAsync(this EfirClient client, double tenor)
        {
            return await client.ExCalculateGcurveForDateAsync(DateTime.Now, tenor);
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
        public static async Task<double> ExCalculateGcurveForDateAsync(this EfirClient client, DateTime date, double tenor)
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
        public static async Task<EfirSecurity> ExGetEfirSecurityAsync(this EfirClient client, string Isin, bool loadCoupons = false)
        {
            FintoolReferenceDataFields[] data = await client.GetSecurityDataAsync(Isin);
            if (data.Length == 0)
                throw new Exception($"EFIR: no securities found for ISIN: {Isin}");

            var sec = EfirSecurity.ConvertFromEfirFields(data[0]);

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


        public static async Task<CreditRating[]> ExGetRaitingHistoryAsync(this EfirClient client, params string[] inns)
        {
            // split target sequence into chunks of size 100.
            string[][] chunks = (inns.Length > 100) 
                ? inns.Chunk(100).ToArray() 
                : new string[][] { inns };

            List<CreditRating> history = new(inns.Length);

            foreach (var chunk in chunks)
            {
                var data = await client.GetRatingHistoryAsync(chunk);
                history.AddRange(data.Select(r => CreditRating.ConvertFromEfirRatingsFields(r)));
            }
            return history.ToArray();
        }

        /// <summary>
        ///     Returns ratings history for specified issuer or its security.
        /// </summary>
        /// <param name="innOrIsin">Issuer INN or its ISIN.</param>
        /// <returns>Array of <see cref="CreditRating"/>.</returns>
        public static async Task<CreditRating[]> ExGetAllRatingsAsync(this EfirClient client, string innOrIsin)
        {
            if (_ratings.TryGetValue(innOrIsin, out CreditRating[]? ratings))
                return ratings;

            var data = await client.GetRatingHistoryAsync(innOrIsin);   
            var ratingsNew = new CreditRating[data.Length];
            for (int i = 0; i < ratingsNew.Length; i++)
                ratingsNew[i] = CreditRating.ConvertFromEfirRatingsFields(data[i]);

            // caching
            _ratings.Add(innOrIsin, ratingsNew);

            return ratingsNew;
        }


        /// <summary>
        ///     Finds securities according to specified parameters (<see cref="EfirSecQueryDetails"/>).
        /// </summary>
        /// <param name="query">Query details.</param>
        /// <returns>Array of <see cref="EfirSecurity"/> each of which satisfies <see cref="EfirSecQueryDetails"/> object.</returns>
        public static async Task<EfirSecurity[]> ExFindAnalogsAsync(this EfirClient client, EfirSecQueryDetails query)
        {
            string querystr = query.ToString();
            var securities = await client.FindSecuritiesAsync(querystr);

            var inns = securities
                .Select(sec => sec.borrowerinn)
                .Where(inn => inn is not null)
                .Distinct()
                .ToArray();

            var raitings = await client.ExGetRaitingHistoryAsync(inns);
            var analogs = new List<EfirSecurity>();

            foreach (var rs in securities)
            {
                var sec = EfirSecurity.ConvertFromEfirFields(rs);
                var raiting = RuDataTools.CreateAggregatedRating(raitings.Where(r => r.Inn == sec.IssuerInn).ToArray());

                if (query.Big3RatingLow is null)
                {
                    if (query.RuRatingLow is not null)
                    {
                        if (raiting < query.RuRatingLow.Value) continue;
                    }
                }
                else
                {
                    if (raiting < query.Big3RatingLow.Value) continue;
                }


                if (query.Big3RatingHigh is null)
                {
                    if (query.RuRatingHigh is not null)
                    {
                        if (raiting > query.RuRatingHigh.Value) continue;
                    }
                }
                else
                {
                    if (raiting > query.Big3RatingHigh.Value) continue;
                }

                sec.RatingAggregated = raiting;
                analogs.Add(sec);
            }
            return analogs.ToArray();
        }
    }
}
