using System.Globalization;

namespace RuDataAPI.Extensions
{
    public readonly struct Tenor : IParsable<Tenor>
    {
        // tenor base: day, week, month, year, etc..
        public enum TenorBase : int
        {
            D = 1,                                          // 1 day
            W = 7,                                          // days in 1 week            
            M = 30,                                         // days in 1 month
            Q = 91,                                         // days in 1 quater
            Y = 365,                                        // days in year
        }

        // tenor components
        private readonly Dictionary<TenorBase, int> _cmp = new()
        {
            {TenorBase.D , 0 },                             // number of days in tenor
            {TenorBase.W , 0 },                             // number of weeks in tenor
            {TenorBase.M , 0 },                             // number of months in tenor
            {TenorBase.Q , 0 },                             // number of quaters in tenor
            {TenorBase.Y , 0 },                             // number of years in tenor
        };


#pragma warning disable IDE1006 // Naming Styles
        private const TenorBase D = TenorBase.D;
        private const TenorBase W = TenorBase.W;
        private const TenorBase M = TenorBase.M;
        private const TenorBase Q = TenorBase.Q;
        private const TenorBase Y = TenorBase.Y;
#pragma warning restore IDE1006 // Naming Styles

        private readonly int _days; // number of days in tenor.

        public Tenor(params (TenorBase, int)[] components)
        {
            foreach (var (baseUnit, count) in components)
                _cmp[baseUnit] += count;
            Rearrange();
            _days = GetNumberOfDays();
        }

        public Tenor (int days)
        {
            _cmp[D] = days;
            Rearrange();
            _days = days;
        }

        public int Days => _days;
        public IReadOnlyDictionary<TenorBase, int> Components => _cmp;

        public static Tenor Parse(string s)
            => Parse(s, CultureInfo.InvariantCulture);


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
            //throw new Exception($"Wrong tenor format {s}");
        }

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


        private static int MonthsToDays(int months)
        {
            if (months % 12 == 0) return months / 12 * (int)Y;
            if (months % 3 == 0) return months / 3 * (int)Q;
            return months * (int)M;
        }


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

            // 1 quater ---> 3 months
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

        public override string ToString()
        {
            string retval = string.Empty;
            foreach (var (baseUnit, count) in _cmp.Reverse())
                if (count > 0) retval += $"{count}{baseUnit}";
            return retval;
        }

        public static DateTime operator +(DateTime d, Tenor t)        
            => d.AddDays(t.Days);
        

        public static DateTime operator +(Tenor t, DateTime d)        
            => d.AddDays(t.Days);
        

        public static DateTime operator -(DateTime d, Tenor t)        
            => d.AddDays(-t.Days);
        

        public static implicit operator Tenor(int days)
        {
            if (days < 0) 
                throw new Exception("Negative tenors not allowed.");
            return new(days);
        }

        public static bool operator ==(Tenor t1, Tenor t2)
            => t1.Days == t2.Days;        

        public static bool operator !=(Tenor t1, Tenor t2)
            => t1.Days != t2.Days;
    }
}
