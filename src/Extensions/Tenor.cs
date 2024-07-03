using System.Globalization;

namespace RuDataAPI.Extensions
{
    /// <summary>
    ///     Represents time tenor.
    /// </summary>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public readonly struct Tenor : IParsable<Tenor>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    {
        // tenor base: day, week, month, year, etc..
        public enum TenorBase : int
        {
            D = 1,                                          // 1 day
            W = 7,                                          // 1 week   = 7 days          
            M = 30,                                         // 1 month  = 30 days
            Q = 91,                                         // 1 quarter = 91 day
            Y = 365,                                        // 1 year   = 365 days
        }

#pragma warning disable IDE1006 // Naming Styles
        private const TenorBase D = TenorBase.D;
        private const TenorBase W = TenorBase.W;
        private const TenorBase M = TenorBase.M;
        private const TenorBase Q = TenorBase.Q;
        private const TenorBase Y = TenorBase.Y;
#pragma warning restore IDE1006 // Naming Styles


        // tenor components
        private readonly Dictionary<TenorBase, int> _cmp = new()
        {
            {TenorBase.D , 0 },                             // number of days in tenor
            {TenorBase.W , 0 },                             // number of weeks in tenor
            {TenorBase.M , 0 },                             // number of months in tenor
            {TenorBase.Q , 0 },                             // number of quarters in tenor
            {TenorBase.Y , 0 },                             // number of years in tenor
        };

        private readonly int _days;                         // number of days in tenor.
        private readonly double _years;                     // number of years in tenor.

        /// <summary>
        ///     Creates new tenor from components specified.
        /// </summary>
        /// <param name="components"></param>
        public Tenor(params (TenorBase, int)[] components)
        {
            foreach (var (baseUnit, count) in components)
                _cmp[baseUnit] += count;
            Rearrange();
            _days = GetNumberOfDays();
            _years = GetNumberOfYears();
        }

        /// <summary>
        ///     Creates new tenor from specified number of days.
        /// </summary>
        /// <param name="days">Number of days.</param>
        public Tenor (int days)
        {
            if (days < 0) NegativeTenorException();
            _cmp[D] = days;
            Rearrange();
            _days = days;
            _years = GetNumberOfYears();
        }

        /// <summary>
        ///     Creates new tenor from specified number of years.
        /// </summary>
        /// <param name="years">Number of years.</param>
        public Tenor (double years)
        {
            if (years < 0) NegativeTenorException();
            int fullyears = (int)years;
            _cmp[Y] = fullyears;
            if (years % 0.25 == 0)
                _cmp[Q] = (int)((years - fullyears) / 0.25); 
            else
                _cmp[D] = (int)((years - fullyears) * 365);
            Rearrange();
            _days = GetNumberOfDays();
            _years = years;
        }

        /// <summary>
        ///     Tenor length in days.
        /// </summary>
        public int Days => _days;

        /// <summary>
        ///     Tenor length in years.
        /// </summary>
        public double Years => _years;

        /// <summary>
        ///     Tenor components (e.g. days, weeks, months, quarters, years)
        /// </summary>
        public IReadOnlyDictionary<TenorBase, int> Components => _cmp;

        /// <summary>
        ///     Parses tenor from specified string. 
        /// </summary>
        /// <param name="s">String representation of tenor.</param>
        public static Tenor Parse(string s)
            => Parse(s, CultureInfo.InvariantCulture);

        /// <summary>
        ///     Parses string representation of tenor (e.g. 1W, 1Y, 2Y6M, ...) into a value. 
        /// </summary>
        public static Tenor Parse(string s, IFormatProvider? provider)
        {
            ReadOnlySpan<char> bases =
                stackalloc[] { 'D', 'W', 'M', 'Q', 'Y' };

            s = s.Trim().Replace("_", "");
            List<(TenorBase, int)> components = new();
            int i = 0;
            foreach (char c in s)
            {
                char upper = char.ToUpper(c);
                if (bases.Contains(upper))
                {
                    int len = s.IndexOf(c, i);
                    int n = 1;
                    if (len > 0)
                    {
                        if (int.TryParse(s.AsSpan(i, len - i), provider, out int res)) 
                            n = res;
                        else throw new FormatException($"Wrong tenor format {s}");
                    }
                    if (n < 0) 
                        throw new FormatException("Negative tenors not allowed.");
                    TenorBase b = Enum.Parse<TenorBase>(upper.ToString());
                    components.Add((b, n));
                    i = len + 1;
                }
            }

            if (components.Count == 0 && int.TryParse(s, provider, out int days))            
                components.Add((D, days));
            
            return new Tenor(components.ToArray());
        }

        /// <summary>
        ///     Tries to parse string representation of tenor into a value. 
        /// </summary>
        public static bool TryParse(string? s, IFormatProvider? provider, out Tenor result)
        {
            try
            {                
                result = Parse(s!, provider); 
                return true;
            }
            catch 
            {
                result = new Tenor(0); ;
                return false;
            }
        }

        /// <summary>
        ///     Converts number of months to number of days. 
        /// </summary>
        private static int MonthsToDays(int months)
        {
            if (months % 12 == 0) return months / 12 * (int)Y;
            if (months % 3 == 0) return months / 3 * (int)Q;
            return months * (int)M;
        }

        /// <summary>
        ///     Calculates number of days in time tenor. 
        /// </summary>
        private int GetNumberOfDays()
        {
            int days = 0
                + _cmp[D] * (int)D
                + _cmp[W] * (int)W
                + _cmp[Y] * (int)Y
                + MonthsToDays(_cmp[M])
                ;

            return days;
        }

        /// <summary>
        ///     Calculates number of years in time tenor. 
        /// </summary>
        private double GetNumberOfYears()
        {
            double years = 0
                + _cmp[Y] 
                + _cmp[W] * (int)W / 365.0
                + _cmp[D] * (int)D / 365.0
                + _cmp[M] / 12.0
                ;

            return years;
        }

        /// <summary>
        ///     Rearranges tenor components for standard combinations such as 365D, 52W, 12M, etc.
        /// </summary>
        private void Rearrange()
        {
            // do not change order of IF blocks !!

            // 365 days ---> 1 year
            if (_cmp[D] % 365 == 0)
            {
                _cmp[Y] += _cmp[D] / 365;
                _cmp[D] = 0;
            }

            // 91 days ---> 3 months
            if (_cmp[D] % 91 == 0)
            {
                _cmp[M] += _cmp[D] / 91 * 3;
                _cmp[D] = 0;
            }

            // 7 days ---> 1 week
            if (_cmp[D] % 7 == 0)
            {
                _cmp[W] += _cmp[D] / 7;
                _cmp[D] = 0;
            }

            // 52 weeks ---> 1 year
            if (_cmp[W] % 52 == 0)
            {
                _cmp[Y] += _cmp[W] / 52;
                _cmp[W] = 0;
            }

            // 4 weeks ---> 1 month
            if (_cmp[W] % 4 == 0)
            {
                _cmp[M] += _cmp[W] / 4;
                _cmp[W] = 0;
            }

            // 1 quarter ---> 3 months
            if (_cmp[Q] > 0)
            {
                _cmp[M] += _cmp[Q] * 3;
                _cmp[Q] = 0;
            }

            // 12 months ---> 1 year
            if (_cmp[M] % 12 == 0)
            {
                _cmp[Y] += _cmp[M] / 12;
                _cmp[M] = 0;
            }
        }

        /// <summary>
        ///     Returns string representation of tenor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retval = string.Empty;
            foreach (var (baseUnit, count) in _cmp.Reverse())
                if (count > 0) retval += $"{count}{baseUnit}";
            return retval;
        }

        /// <summary>
        ///     Determines whether the specified tenor is equal to the current <see cref="Tenor"/> instance.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Tenor t) 
                return t.Days == this.Days;
            return false;
        }

        private static void NegativeTenorException()        
            => throw new Exception("Negative tenor are not allowed.");

        public static Tenor operator +(Tenor t1, Tenor t2)
            => new(t1._days + t2._days);

        public static DateTime operator +(DateTime d, Tenor t)        
            => d.AddDays(t.Days);
        

        public static DateTime operator +(Tenor t, DateTime d)        
            => d.AddDays(t.Days);
        

        public static DateTime operator -(DateTime d, Tenor t)        
            => d.AddDays(-t.Days);
        

        public static implicit operator Tenor(int days)
        {
            if (days < 0) NegativeTenorException();
            return new(days);
        }

        public static implicit operator Tenor(double years)
        {
            if (years < 0) NegativeTenorException();
            return new(years);
        }

        public static implicit operator Tenor(TimeSpan span)
        {
            return new(span.Days);
        }

        public static bool operator ==(Tenor t1, Tenor t2)
            => t1.Days == t2.Days;        

        public static bool operator !=(Tenor t1, Tenor t2)
            => t1.Days != t2.Days;
    }
}
