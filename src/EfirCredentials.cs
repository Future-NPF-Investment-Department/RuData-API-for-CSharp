using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RuDataAPI
{
    public class EfirCredentials
    {
        [JsonIgnore]
        public static EfirCredentials Empty { get; } = new EfirCredentials();
        public string? Url { get; init; }
        public string? Login { get; init; }
        public string? Password { get; init; }
    }
}
