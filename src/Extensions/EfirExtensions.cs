using Efir.DataHub.Models.Models.Bond;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.RuData;
using RuDataAPI.Extensions.Ratings;
using System.Collections.Concurrent;


// update GetLastRatings method in RuDataTools:
//      можно обойтись бес пересчета кол-ва ИНН
//      и без первых 2 блоков кода
//
// подумать над тем нужно ли использовать params в методах
// 
// RuDataTools.GetLastRatings мб должен возвращать ienumerable?
//
// поменять CreditRatinAggregation - убрать бит маску
//      и изменить метод ToString






namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Provides additional methods to get different sort of market data using EFIR.DataHub services.
    /// </summary>
    public static class EfirExtensions
    {

        /// <summary>
        ///     Extracts G-Curve for specified date.
        /// </summary>
        /// <param name="date">G-Curve date.</param>
        /// <param name="provider">G-Curve provider.</param>
        public static async Task<YieldCurve> GetGCurve(this EfirClient client, DateTime date, CurveProvider provider) => provider switch
        {
            CurveProvider.MOEX => (await client.GetGcurveParametersAsync(date)).ToYieldCurve(),
            _ => new YieldCurve()
        };


        /// <summary>
        ///     Extracts history of rating actions for specified collection of INN codes.
        /// </summary>
        /// <param name="startDate">Search period start date. If null, calculated as start date minus 365 days.</param>
        /// <param name="endDate">Search period end date. If null, TODAY is used.</param>
        /// <param name="inns">Collection of INN codes.</param>
        /// <returns>Collection of <see cref="CreditRatingAction"/>.</returns>
        public static async Task<IEnumerable<CreditRatingAction>> ExGetRatingActionsByInns(this EfirClient client, DateTime? startDate = null, DateTime? endDate = null, params string[] inns)
        {
            var end = endDate is not null ? endDate.Value : DateTime.Now;
            var start = startDate is not null ? startDate.Value : end.AddDays(-365);

            var filter = RuDataTools.CreateRatingFilter(inns, "INN");
            var historyRaw = await client.GetRatingHistoryAsync(start, end, filter);
            var missingInnCodes = inns.Except(historyRaw.Select(ratHist => ratHist.inn));
            var retval = historyRaw.Select(r => r.ToCreditRating());

            foreach (var inncode in missingInnCodes)
                retval = retval.Concat(RuDataTools.CreateDefaultRatings(inncode));

            return retval;
        }


        /// <summary>
        ///     Extracts history of rating actions for specified collection of ratings. Ratings provided must be on one of the known scales. For example ruAAA, AAA(RU), AAA.ru, AAA|ru|.
        /// </summary>
        /// <param name="startDate">Search period start date. If null, calculated as start date minus 365 days.</param>
        /// <param name="endDate">Search period end date. If null, TODAY is used.</param>
        /// <param name="ratings">Collection of ratings in string format.</param>
        /// <returns>Collection of <see cref="CreditRatingAction"/>.</returns>
        public static async Task<IEnumerable<CreditRatingAction>> ExGetRatingActionsByRatings(this EfirClient client, DateTime? startDate = null, DateTime? endDate = null, params string[] ratings)
        {
            var end = endDate is not null ? endDate.Value : DateTime.Now;
            var start = startDate is not null ? startDate.Value : end.AddDays(-365);

            var filter = RuDataTools.CreateRatingFilter(ratings, "LAST");
            var historyRaw = await client.GetRatingHistoryAsync(start, end, filter);

            return historyRaw.Select(r => r.ToCreditRating()).Where(s => !string.IsNullOrEmpty(s.Inn));
        }


        /// <summary>
        ///     Extracts last known rating actions for specified collection of INN codes.
        /// </summary>
        /// <param name="date">Date as of which the latest known rating actions are searched.</param>
        /// <param name="inns">Collection of INN codes.</param>
        /// <returns>Collection of <see cref="CreditRatingAction"/>.</returns>
        public static async Task<IEnumerable<CreditRatingAction>> ExGetLastRatingActionsByInn(this EfirClient client, DateTime? date = null, params string[] inns)
        {
            var end = date is not null ? date.Value : DateTime.Now;
            var start = date is not null ? date.Value.AddDays(-365) : DateTime.Now.AddDays(-365);
            var hist = await client.ExGetRatingActionsByInns(start, end, inns);
            var last = RuDataTools.GetLastRatings(hist);
            return last;
        }


        /// <summary>
        ///     Extracts last known rating actions for specified ISIN-code.
        /// </summary>
        /// <param name="date">Date as of which the latest known rating actions are searched.</param>
        /// <param name="isin">Security ISIN-code.</param>
        /// <param name="date">Date as of which the latest known rating actions are searched.</param>
        /// <returns>Collection of <see cref="CreditRatingAction"/>.</returns>
        public static async Task<CreditRatingAction[]> ExGetLastRatingActionsByIsin(this EfirClient client, string isin, DateTime? date = null)
        {
            if (RuDataTools.IsIsinCode(isin) is false)
                throw new Exception($"Not an ISIN: {isin}.");

            var secinfo = await client.GetFinToolRefDataAsync(isin, RefDataCols.ALLCODES);

            var end = date is not null ? date.Value : DateTime.Now;
            var start = date is not null ? date.Value.AddDays(-365) : DateTime.Now.AddDays(-365);

            var hist = await client.ExGetRatingActionsByInns(start, end, secinfo.issuerinn);
            var histForIsin = hist.Any(r => r.Isin == isin)
                ? hist.Where(r => r.Isin == isin)
                : hist;

            var last = RuDataTools.GetLastRatings(histForIsin);

            return last;
        }


        /// <summary>
        ///     Extracts last known rating actions for specified collection of ratings. Ratings provided must be on one of the known scales. For example ruAAA, AAA(RU), AAA.ru, AAA|ru|.
        /// </summary>
        /// <param name="date">Date as of which the latest known rating actions are searched.</param>
        /// <param name="ratings">Collection of ratings in string format.</param>
        /// <returns>Collection of <see cref="CreditRatingAction"/>.</returns>
        public static async Task<IEnumerable<CreditRatingAction>> ExGetLastRatingActionsByRatings(this EfirClient client, DateTime? date = null, params string[] ratings)
        {
            var end = date is not null ? date.Value : DateTime.Now;
            var start = end.AddDays(-365);
            var hist = await client.ExGetRatingActionsByRatings(start, end, ratings);
            return RuDataTools.GetLastRatings(hist);
        }


        /// <summary>
        ///     Exctract history of trade history for specified collection of ISIN-codes.
        /// </summary>
        /// <param name="startDate">Trade history search start date.</param>
        /// <param name="endDate">Trade history search end date</param>
        /// <param name="isins">Collection of ISIN-codes.</param>
        /// <returns>Collection of <see cref="InstrumentHistoryRecord"/>.</returns>
        public static async Task<IEnumerable<InstrumentHistoryRecord>> ExGetTradeHistory(this EfirClient client, DateTime startDate, DateTime endDate, params string[] isins)
        {
            var histRaw = await client.GetTradeHistoryAsync(startDate, endDate, isins);
            var hist = histRaw.Select(hr => hr.ToHistoryRecord());
            return hist;
        }


        /// <summary>
        ///     Exctract flows for specified collection of ISIN-codes.
        /// </summary>
        /// <param name="isins">Collection of ISIN-codes.</param>
        /// <returns>Collection of <see cref="InstrumentFlow"/>.</returns>
        public static async Task<IEnumerable<InstrumentFlow>> ExGetInstrumentFlows(this EfirClient client, params string[] isins)
        {
            var flowsRaw = await client.GetEventsCalendarAsync(isins);
            var flows = flowsRaw.Select(hr => hr.ToFlow());
            return flows;
        }


        /// <summary>
        ///     Exctracts all info for specified INN-code, including main parameters, flows, trade history and last rating actions.
        /// </summary>
        /// <param name="isin">ISIN-code.</param>
        /// <returns><see cref="InstrumentInfo"/> instance.</returns>
        /// <exception cref="Exception">is thrown if bad ISIN code provided.</exception>
        public static async Task<InstrumentInfo> ExGetInstrumentInfo(this EfirClient client, string isin)
        {
            if (!RuDataTools.IsIsinCode(isin))
                throw new Exception($"Not an ISIN: {isin}.");

            var end = DateTime.Now.Date.AddDays(-1);
            var start = end.AddDays(-30);

            var secTask = client.GetFinToolRefDataAsync(isin);
            var histTask = client.ExGetTradeHistory(start, end, isin);
            var flowsTask = client.ExGetInstrumentFlows(isin);


            var sec = await secTask;
            var ratings = await client.ExGetLastRatingActionsByInn(null, sec.issuerinn);
            var flows = await flowsTask;
            var hist = await histTask;

            return RuDataTools.CreateInstrumentInfo(sec, flows, hist, ratings);
        }


        public static async Task<HolidaysFields[]> ExGetHolidays(this EfirClient client, Calendar cdr = Calendar.RU)
        {
            int year = DateTime.Now.Year;
            DateTime start = new(year - 1, 12, 20);
            DateTime end = new(year, 12, 31);
            var holidays = await client.GetHolidaysAsync(start, end, cdr);
            return holidays;
        }

        /// <summary>
        ///     Search bond analogs by criteria provided. 
        /// </summary>
        /// <param name="query">Search criteria info.</param>
        /// <returns>Collection of <see cref="InstrumentInfo"/>.</returns>
        public static async Task<InstrumentInfo[]> ExSearchBonds(this EfirClient client, EfirSecQueryDetails query)
        {
            // define time period for history 
            var end = DateTime.Now.Date.AddDays(-1);
            var start = end.AddDays(-30);

            // create query string
            string querystr = query.ToString();

            // construct string representation for ratings
            string[] ratingStrings = Array.Empty<string>();
            if (query.RuRatingLow != null || query.RuRatingHigh != null)
            {
                var range = Rating.GetRatingRange(query.RuRatingLow, query.RuRatingHigh);
                ratingStrings = RuDataTools.GetRatingRangeStrings(range);
            }
            else if (query.Big3RatingLow != null || query.Big3RatingHigh != null)
            {
                var range = Rating.GetRatingRange(query.Big3RatingLow, query.Big3RatingHigh);
                ratingStrings = RuDataTools.GetRatingRangeStrings(range);
            }

            // получаем все уникальные ИНН у которых хоть когда за последний год был рейтинг из массива ratingStrings
            var ratings = await client.ExGetRatingActionsByRatings(null, null, ratingStrings);
            var innCodesChunks = ratings.Select(ra => ra.Inn).Distinct().Chunk(100);
            var allRatings = Enumerable.Empty<CreditRatingAction>();
            
            var tasks = new List<Task<FintoolReferenceDataFields[]>>();
            foreach (string[] chunk in innCodesChunks)
            {
                // по полученным ИНН выстаскиваем последние имеющиеся ретйинги на сегодня
                var chunkRatings = await client.ExGetLastRatingActionsByInn(null, chunk);
                allRatings = allRatings.Concat(chunkRatings);
                var innCodes = chunkRatings.Select(r => r.Inn).Distinct();

                string fullQueryString = querystr + $" AND ISSUERINN in ('{string.Join("', '", innCodes)}')";

                // get securities data that satisfies specified criteria
                var chunkTask = client.FindSecuritiesAsync(fullQueryString);
                tasks.Add(chunkTask);
            }

            var secTasksResult = await Task.WhenAll(tasks);
            var secs = secTasksResult.SelectMany(s => s);

            // extract ISIN codes from data received
            var isins = secs.Select(sec => sec.isincode).ToArray();

            // start tasks to get history records and flows for extracted ISIN codes
            var histTask = client.ExGetTradeHistory(start, end, isins);
            var flowsTask = client.ExGetInstrumentFlows(isins);

            // waiting for tasks started before
            var hist = await histTask;
            var flows = await flowsTask;

            // create buffer for resulting collection
            var result = new List<InstrumentInfo>();

            // fill in the buffer 
            foreach (var sec in secs)
            {
                // filter isin-specific flows, hist and ratings
                var secflows = flows.Where(f => f.Isin == sec.isincode).Any()
                    ? flows.Where(f => f.Isin == sec.isincode)
                    : null;

                var sechist = hist.Where(h => h.Isin == sec.isincode).Any()
                    ? hist.Where(h => h.Isin == sec.isincode)
                    : null;

                var secratings = allRatings.Where(r => r.Isin == sec.isincode).Any()
                    ? allRatings.Where(r => r.Isin == sec.isincode)
                    : allRatings.Where(r => r.Inn == sec.issuerinn);

                // constructing InstrumentInfo instance
                var si = RuDataTools.CreateInstrumentInfo(sec, secflows, sechist, secratings);

                // if rating boundaries do exist and are violated then go next
                if (query.RuRatingLow != null && si.RatingAggregated < query.RuRatingLow.Value) continue;
                if (query.RuRatingHigh != null && si.RatingAggregated > query.RuRatingHigh.Value) continue;
                if (query.Big3RatingLow != null && si.RatingAggregated < query.Big3RatingLow.Value) continue;
                if (query.Big3RatingHigh != null && si.RatingAggregated > query.Big3RatingHigh.Value) continue;

                // otherwise add to result collection
                result.Add(si);
            }

            // cast to array and return
            return result.ToArray();
        }
    }
}
