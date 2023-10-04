using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuDataAPI.Extensions.Mapping
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RatingAttribute : Attribute
    {
        protected const string FITCH      = "Fitch";
        protected const string MOODYS     = "Moodys";
        protected const string SNP        = "SNP";
        protected const string AKRA       = "Akra";
        protected const string EXPRA      = "ExpRA";
        protected const string NKR        = "NKR";
        protected const string NRA        = "NRA";

        private protected readonly Dictionary<string, string[]> _map = new();

        protected RatingAttribute() { }

        public RatingAttribute(string rating)         
            => Rating = rating;
        
        public string Rating { get; init; } = null!;
    }
}
