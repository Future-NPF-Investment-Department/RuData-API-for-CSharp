using Efir.DataHub.Models.Models.Bond;
using Efir.DataHub.Models.Models.Moex;
using Efir.DataHub.Models.Models.Rating;
using Efir.DataHub.Models.Models.Archive;
using RuDataAPI.Extensions.Mapping;
using RuDataAPI.Extensions.Ratings;
using System.Reflection;
using System.Text.RegularExpressions;
using Iuliia;
using Efir.DataHub.Models.Models.Info;
using Efir.DataHub.Models.Models.RuData;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Auxiliary tools.
    /// </summary>
    internal static class RuDataTools
    {
        /// <summary>
        ///     Maps string value to specified enum field using attributes specified for this field.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="strval">value to parse.</param>
        /// <returns>Field of specified enum.</returns>
        internal static TEnum MapToEnum<TEnum>(string strval) where TEnum : struct, Enum
        {
            if (strval is "") return default;

            Type enumType = typeof(TEnum);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                var attr = field.GetCustomAttribute<EnumFieldStrAttribute>();
                if (attr is not null && attr.Values.Contains(strval))
                    return Enum.Parse<TEnum>(field.Name);
            }
            return default;
        }

        /// <summary>
        ///     Get representation string for specified enum-mapping value.
        /// </summary>
        internal static string GetEnumPrintStrig<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.Name == value.ToString())
                {
                    var attr = field.GetCustomAttribute<EnumFieldStrAttribute>();
                    if (attr is not null)
                        return attr.PrintString;
                }
            }
            return string.Empty;
        }











        public static string[] GetRatingRangeStrings(CreditRatingUS[] range)
        {
            List<string> ret = new();
            foreach (var rating in range)
                ret.AddRange(Rating.ExtractUnderlyingRatings(rating));
            return ret.ToArray();
        }






        public static string[] GetRatingRangeStrings(CreditRatingRU[] range)
        {
            List<string> ret = new();
            foreach (var rating in range)
                ret.AddRange(Rating.ExtractUnderlyingRatings(rating));
            return ret.ToArray();
        }


        internal static string CreateRatingFilter(string[] items, string fieldName)
        {
            string iscr = "IS_CREDIT_RATING = 1";
            string term = "RATING_TERM = 'Долгосрочный'";
            string ra = "RATING_AGENCY IN ('Moody''s', 'Standard & Poor''s', 'Fitch Ratings', 'АКРА', 'Эксперт РА', 'НКР', 'НРА')";
            string inn = $"{fieldName} IN ('{string.Join("', '", items)}')";
            return string.Join(" AND ", iscr, term, ra, inn);
        }

        /// <summary>
        ///     Creates default ratings set for specified INN code. All ratings are NR.
        /// </summary>
        /// <param name="inn">INN code</param>
        /// <returns>Array of <see cref="CreditRatingAction"/>.</returns>
        internal static CreditRatingAction[] CreateDefaultRatings(string inn)
        {
            return new CreditRatingAction[7]
            {
                new CreditRatingAction() { Scale = CreditRatingScale.International,   Inn = inn,   Agency = RatingAgency.FITCH    },
                new CreditRatingAction() { Scale = CreditRatingScale.International,   Inn = inn,   Agency = RatingAgency.MOODYS   },
                new CreditRatingAction() { Scale = CreditRatingScale.International,   Inn = inn,   Agency = RatingAgency.SNP      },
                new CreditRatingAction() { Scale = CreditRatingScale.National,        Inn = inn,   Agency = RatingAgency.AKRA     },
                new CreditRatingAction() { Scale = CreditRatingScale.National,        Inn = inn,   Agency = RatingAgency.RAEX     },
                new CreditRatingAction() { Scale = CreditRatingScale.National,        Inn = inn,   Agency = RatingAgency.NKR      },
                new CreditRatingAction() { Scale = CreditRatingScale.National,        Inn = inn,   Agency = RatingAgency.NRA      }
            };
        }

        /// <summary>
        ///     Selects last ratings for particular ratings history. 
        ///     This method support history for multiple INN codes.
        /// </summary>
        /// <returns>Array of <see cref="CreditRatingAction"/></returns>
        internal static CreditRatingAction[] GetLastRatings(IEnumerable<CreditRatingAction> history)
        {
            // count number of unique inn codes
            int uniqueInnCodesNumber = history.DistinctBy(r => r.Inn)
                .Select(r => r.Inn).Count();

            // if only 1 inn code then group history by agency
            // and find max rating date in each group
            if (uniqueInnCodesNumber == 1)            
                return history.GroupBy(r => r.Agency)
                .Select(g => g.MaxBy(r => r.Date)!)
                .ToArray();

            // if multiple inn codes then group history by unique inn codes
            // then in each group find last ratings for each agency
            return history
                .GroupBy(r => r.Inn)
                .SelectMany(g1 => g1.GroupBy(r => r.Agency).Select(g2 => g2.MaxBy(r => r.Date)!))
                .ToArray();
        }

        /// <summary>
        ///     Aggregates ratings using specified metod.
        /// </summary>
        /// <param name="ratings"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static CreditRatingAggregated AggregateRatings(IEnumerable<CreditRatingAction> ratings, RatingAggregationMethod method = default)
        {
            // ratings should be only for single INN-code.
            // ratings could be issuer-targeted or isin-targeted.
            // if they are both targeted issuer-targeted are selected.

            const CreditRatingScale NATIONAL = CreditRatingScale.National;

            if (ratings.Any() is false) return CreditRatingAggregated.Default;

            var filtered = ratings.Any(r => r.Object is CreditRatingTarget.Issuer)
                ? ratings.Where(r => r.Object is CreditRatingTarget.Issuer)
                : ratings;

            var aggrrat = new CreditRatingAggregated();
            IEnumerable<CreditRatingUS> usRatings = filtered.Select(r => Rating.ParseRatingUS(r.Value, r.Agency));
            IEnumerable<CreditRatingRU> ruRatings = filtered.Any(r => r.Scale is NATIONAL)
                ? filtered.Where(r => r.Scale is NATIONAL).Select(r => Rating.ParseRatingRU(r.Value, r.Agency))
                : filtered.Select(r => Rating.ParseRatingRU(r.Value, r.Agency));

            // all possible ratings are used for aggregation
            if (method is RatingAggregationMethod.Any)
            {
                aggrrat.RatingRu = ruRatings.Aggregate((rur1, rur2) => rur1 | rur2);
                aggrrat.RatingBig3 = usRatings.Aggregate((rur1, rur2) => rur1 | rur2);
                aggrrat.DefaultProbability = aggrrat.RatingBig3 is not CreditRatingUS.NR
                ? Rating.GetDefaultProbality(usRatings.Min())
                : Rating.GetDefaultProbality(ruRatings.Min());
            }

            // minimum rating is selected
            if (method is RatingAggregationMethod.Min)
            {
                aggrrat.RatingRu = ruRatings.Min();
                aggrrat.RatingBig3 = usRatings.Min();
                aggrrat.DefaultProbability = aggrrat.RatingBig3 is not CreditRatingUS.NR
                ? Rating.GetDefaultProbality(aggrrat.RatingRu)
                : Rating.GetDefaultProbality(aggrrat.RatingBig3);
            }

            // maximum rating is selected
            if (method is RatingAggregationMethod.Max)
            {
                aggrrat.RatingRu = ruRatings.Max();
                aggrrat.RatingBig3 = usRatings.Max();
                aggrrat.DefaultProbability = aggrrat.RatingBig3 is not CreditRatingUS.NR
                ? Rating.GetDefaultProbality(aggrrat.RatingRu)
                : Rating.GetDefaultProbality(aggrrat.RatingBig3);
            }

            aggrrat.Ratings = filtered;
            aggrrat.Issuer = filtered.First().IssuerName;

            return aggrrat;
        }

        /// <summary>
        ///     Converts <see cref="TimeTableV2Fields"/> object to <see cref="InstrumentFlow"/> object.
        /// </summary>
        internal static InstrumentFlow ToFlow(this TimeTableV2Fields fields)
        {
            if (fields.TypeOperation is null)
                throw new EfirFieldNullValueException($"Intrument flow has undefined operation type. ISIN: {fields.ISINcode}; FLOWID: {fields.EventID}");

            var flow = new InstrumentFlow()
            {
                Id              = fields.EventID ?? default,
                Isin            = fields.ISINcode ?? string.Empty,
                StartDate       = fields.BeginEventPer ?? default,
                EndDate         = fields.EventDate ?? default,
                PeriodLength    = fields.EventPeriod is not null ? (int)fields.EventPeriod! : 0,
                Rate            = fields.Value.HasValue ? (double)fields.Value/100.0 : null,
                Payment         = fields.Pay1Bond.HasValue ? (double)fields.Pay1Bond : null,
                PaymentType     = MapToEnum<FlowType>(fields.TypeOperation)
            };
            return flow;
        }

        /// <summary>
        ///     Converts <see cref="RatingsHistoryFields"/> object to <see cref="CreditRatingAction"/> object.
        /// </summary>
        internal static CreditRatingAction ToCreditRating(this RatingsHistoryFields fields)
        {
            if (fields.last is null)
                throw new EfirFieldNullValueException($"Raiting value is null. INN: {fields.inn}; Agency: {fields.rating_agency}");

            var rating = new CreditRatingAction
            {
                Value = fields.last is "Снят" || fields.last is "Приостановлен" ? "NR" : fields.last,
                Agency = MapToEnum<RatingAgency>(fields.rating_agency),
                Date = fields.dt ?? default,
                PreviousValue = fields.prev,
                IssuerName = IuliiaTranslator.Translate(fields.short_name_org, Schemas.Mosmetro),
                Isin = fields.isin ?? string.Empty,
                PressRelease = fields.press_release,
                Inn = fields.inn,
                Object = MapToEnum<CreditRatingTarget>(fields.rating_object_type),
                Scale = MapToEnum<CreditRatingScale>(fields.scale_type),
                Currency = MapToEnum<CreditRatingCurrency>(fields.scale_cur),
                Action = MapToEnum<RatingAction>(fields.change),
                Outlook = MapToEnum<CreditRatingOutlook>(fields.forecast ?? string.Empty)
            };
            return rating;
        }

        /// <summary>
        ///     Converts <see cref="EndOfDayOnExchangeFields"/> object to <see cref="InstrumentHistoryRecord"/> object.
        /// </summary>
        internal static InstrumentHistoryRecord ToHistoryRecord(this EndOfDayOnExchangeFields fields)
        {
            if (fields.time is null)
                throw new EfirFieldNullValueException($"Trade date is null. ISIN: {fields.isin}.");

            return new InstrumentHistoryRecord
            {
                Date = fields.time.Value,
                Open = fields.open is null ? default : (double)fields.open,
                High = fields.high is null ? default : (double)fields.high,
                Low = fields.low is null ? default : (double)fields.low,
                Close = fields.last is null ? default : (double)fields.last,
                VolumeTraded = fields.val_acc is null ? default : (double)fields.val_acc,
                AmountTraded = fields.vol_acc is null ? default : (long)fields.vol_acc,
                FaceValue = fields.facevalue is null ? default : (double)fields.facevalue,
                AccruedInterest = fields.accruedint is null ? default : (double)fields.accruedint,
                Isin = fields.isin is null ? string.Empty : fields.isin,
                ExchangeName = fields.exch is null? string.Empty : fields.exch,
                Capitalization = fields.mcap is null ? default : (double)fields.mcap,
                Yield = fields.last_yield is null ? default : (double)fields.last_yield,
                NumberOfDeals = fields.deal_acc is null ? default : (long)fields.deal_acc,
            };
        }

        /// <summary>
        ///     Converts <see cref="HistoryStockBondsFields"/> object to <see cref="InstrumentHistoryRecord"/> object.
        /// </summary>
        internal static InstrumentHistoryRecord ToHistoryRecord(this HistoryStockBondsFields fields)
        {
            if (fields.tradedate is null)
                throw new EfirFieldNullValueException($"Trade date is null. MOEXCODE: {fields.isin}.");

            return new InstrumentHistoryRecord
            {
                Date = fields.tradedate.Value,
                Open = fields.open is null ? default : (double)fields.open,
                High = fields.high is null ? default : (double)fields.high,
                Low = fields.low is null ? default : (double)fields.low,
                Close = fields.close is null ? default : (double)fields.close,
                VolumeTraded = fields.marketprice3tradesvalue is null ? default : (double)fields.marketprice3tradesvalue,
            };
        }

        /// <summary>
        ///     Converts <see cref="HistoryStockSharesFields"/> object to <see cref="InstrumentHistoryRecord"/> object.
        /// </summary>
        internal static InstrumentHistoryRecord ToHistoryRecord(this HistoryStockSharesFields fields)
        {
            if (fields.tradedate is null)
                throw new EfirFieldNullValueException($"Trade date is null. MOEXCODE: {fields.isin}.");

            return new InstrumentHistoryRecord
            {
                Date = fields.tradedate.Value,
                Open = fields.open is null ? default : (double)fields.open,
                High = fields.high is null ? default : (double)fields.high,
                Low = fields.low is null ? default : (double)fields.low,
                Close = fields.close is null ? default : (double)fields.close,
                VolumeTraded = fields.marketprice3tradesvalue is null ? default : (double)fields.marketprice3tradesvalue,
            };
        }

        /// <summary>
        ///     Defines if string is ISIN-code.
        /// </summary>
        /// <param name="code">Code string.</param>
        /// <returns>True if string is ISIN code. Otherwise false.</returns>
        internal static bool IsIsinCode(string code)
        {
            /* ISIN code (according to ISO 6166) consists of:
             *  - 2 alphabetic characters which represents code for the issuing country (ex.: US, XS, RU)
             *  - 9 alpha-numeric characters which identifies the security
             *  - 1 numerical check digit */

            string isinPattern = "([A-Z]{2})([A-Z0-9]{9})([0-9]{1})$";
            return Regex.IsMatch(code, isinPattern);
        }

        /// <summary>
        ///     Checks specified input for bad ISIN codes.
        /// </summary>
        /// <param name="isins">Collection of ISIN-codes to check.</param>
        /// <returns>First bad ISIN found, otherwise null.</returns>
        internal static string? FindFirstBadIsin(params string[] isins)
        {
            foreach (var isin in isins)
                if (IsIsinCode(isin) is false)
                    return isin;
            return null;
        }

        /// <summary>
        ///     Adds bussiness days to specified date.
        /// </summary>
        /// <param name="date">Initial date.</param>
        /// <param name="days">Number of days to add. Could be negative.</param>
        /// <param name="holidays">collection of holiday dates.</param>
        internal static DateTime AddBusinessDays(this DateTime date, int days, IEnumerable<DateTime> holidays)
        {
            if (days == 0) return date;
            int dt = days < 0 ? -1 : 1;
            int period = Math.Abs(days);
            while (period > 0)            
            {
                date += new TimeSpan(dt, 0, 0, 0);
                if (holidays.Contains(date)) continue;                
                period--;
            }
            return date;
        }

        /// <summary>
        ///     Creates InstrumentInfo using FinToolRefData, flows, trade history and last ratings
        /// </summary>
        internal static InstrumentInfo CreateInstrumentInfo(FintoolReferenceDataFields secData, IEnumerable<InstrumentFlow>? flows, 
            IEnumerable<InstrumentHistoryRecord>? history, IEnumerable<CreditRatingAction>? ratings)
        {
            InstrumentInfo sec = new()
            {
                IfaxSecId = secData.fintoolid ?? default,
                MoexSecId = secData.moex_code ?? string.Empty,
                Name = IuliiaTranslator.Translate(secData.nickname ?? string.Empty, Schemas.Mosmetro),
                Isin = secData.isincode ?? string.Empty,
                Issuer = IuliiaTranslator.Translate(secData.issuername_nrd ?? string.Empty, Schemas.Mosmetro),
                Borrower = IuliiaTranslator.Translate(secData.borrowername ?? string.Empty, Schemas.Mosmetro),
                IssuerInn = secData.issuerinn ?? string.Empty,
                BorrowerInn = secData.borrowerinn ?? string.Empty,
                PlacementDate = secData.begdistdate ?? default,
                MaturityDate = secData.endmtydate ?? default,
                Currency = secData.faceftname ?? string.Empty,
                CouponReference = IuliiaTranslator.Translate(secData.floatratename ?? string.Empty, Schemas.Mosmetro),
                CouponType = MapToEnum<CouponType>(secData.coupontype),
                CouponLength = MapToEnum<CouponPeriodType>(secData.coupontypename_nrd),
                Basis = MapToEnum<CouponBasis>(secData.basis),
                Status = MapToEnum<InstrumentStatus>(secData.status),
                AssetClass = MapToEnum<InstrumentType>(secData.fintooltype),
                MarketVolume = secData.summarketval is not null ? (double)secData.summarketval : default,
                IssuerSector = MapToEnum<IssuerSector>(secData.issuersector),
                BorrowerSector = MapToEnum<IssuerSector>(secData.borrowersector),
                CurrentFaceValue = secData.currentfacevalue_nrd is not null ? (double)secData.currentfacevalue_nrd : default,
                InitialFaceValue = secData.facevalue is not null ? (double)secData.facevalue : default,
                GuaranteedValue = secData.guarantval is not null ? (double)secData.guarantval : default                
            };

            if (secData.issubordinated is 1)
                sec.Flags |= InstrumentFlags.Subordinated;
            if (secData.bondstructuralpar is not null)
                sec.Flags |= InstrumentFlags.Structured;
            if (secData.securitization is not null)
                sec.Flags |= InstrumentFlags.Secured;
            if (secData.haveindexedfv is true)
                sec.Flags |= InstrumentFlags.Linker;
            if (secData.isconvertible is 1)
                sec.Flags |= InstrumentFlags.Convertible;
            if (secData.isguaranteed is 1)
                sec.Flags |= InstrumentFlags.Guaranteed;
            if (secData.endmtydate is null)
                sec.Flags |= InstrumentFlags.Perpetual;
            if (secData.haverepayment is true)
                sec.Flags |= InstrumentFlags.Callable;


            if (flows is not null)
                sec.Flows = flows;

            if (history is not null)
                sec.TradeHistory = history;

            if (ratings is not null)            
                sec.RatingAggregated = AggregateRatings(ratings);           

            return sec;
        }

        /// <summary>
        ///     Convertes <see cref="GCurveOFZResponse"/> to <see cref="YieldCurve"/>.
        /// </summary>
        internal static YieldCurve ToYieldCurve(this GCurveOFZResponse gcparams)
        {
            double beta0 = gcparams.beta0val.HasValue
                ? (double)gcparams.beta0val!.Value
                : throw new Exception($"GCurve BETA0 param is null.");

            double beta1 = gcparams.beta1val.HasValue
                ? (double)gcparams.beta1val!.Value
                : throw new Exception($"GCurve BETA1 param is null.");

            double beta2 = gcparams.beta2val.HasValue
                ? (double)gcparams.beta2val!.Value
                : throw new Exception($"GCurve BETA0 param is null.");

            double tau = gcparams.tauval.HasValue
                ? (double)gcparams.tauval!.Value
                : throw new Exception($"GCurve BETA0 param is null.");

            double g1 = gcparams.g1val.HasValue
            ? (double)gcparams.g1val!.Value
                : throw new Exception($"GCurve G1 param is null.");

            double g2 = gcparams.g2val.HasValue
                ? (double)gcparams.g2val!.Value
                : throw new Exception($"GCurve G2 param is null.");

            double g3 = gcparams.g3val.HasValue
                ? (double)gcparams.g3val!.Value
                : throw new Exception($"GCurve G3 param is null.");

            double g4 = gcparams.g4val.HasValue
                ? (double)gcparams.g4val!.Value
                : throw new Exception($"GCurve G4 param is null.");

            double g5 = gcparams.g5val.HasValue
                ? (double)gcparams.g5val!.Value
                : throw new Exception($"GCurve G5 param is null.");

            double g6 = gcparams.g6val.HasValue
                ? (double)gcparams.g6val!.Value
                : throw new Exception($"GCurve G6 param is null.");

            double g7 = gcparams.g7val.HasValue
                ? (double)gcparams.g7val!.Value
                : throw new Exception($"GCurve G7 param is null.");

            double g8 = gcparams.g8val.HasValue
                ? (double)gcparams.g8val!.Value
                : throw new Exception($"GCurve G8 param is null.");

            double g9 = gcparams.g9val.HasValue
                ? (double)gcparams.g9val!.Value
                : throw new Exception($"GCurve G9 param is null.");

            return new YieldCurve()
            {
                Date = gcparams.dt ?? default,
                Provider = CurveProvider.MOEX,
                Tau1 = tau,
                Beta0 = beta0,
                Beta1 = beta1,
                Beta2 = beta2,
                G1 = g1,
                G2 = g2,
                G3 = g3,
                G4 = g4,
                G5 = g5,
                G6 = g6,
                G7 = g7,
                G8 = g8,
                G9 = g9
            };

        }

        

    }
}