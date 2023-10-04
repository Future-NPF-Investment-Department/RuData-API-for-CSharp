using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuDataAPI.Extensions.Mapping
{
    public class RatingMapAttribute : Attribute
    {
        private const string _FITCH      = "Fitch";
        private const string _MOODYS     = "Moodys";
        private const string _SNP        = "SNP";
        private const string _AKRA       = "Akra";
        private const string _EXPRA      = "ExpRA";
        private const string _NKR        = "NKR";
        private const string _NRA        = "NRA";

        private readonly Dictionary<string, string> _map = new()
        {
            {_FITCH, string.Empty},
            {_MOODYS, string.Empty},
            {_SNP, string.Empty},
            {_AKRA, string.Empty},
            {_EXPRA, string.Empty},
            {_NKR, string.Empty},
            {_NRA, string.Empty},
        };

        public RatingMapAttribute(string RATING, string FITCH, string MOODYS, string SNP, string AKRA, string EXPRA, string NKR, string NRA) 
        {
            _map[_FITCH]     = FITCH;
            _map[_MOODYS]    = MOODYS;
            _map[_SNP]       = SNP;
            _map[_AKRA]      = AKRA;
            _map[_EXPRA]     = EXPRA;
            _map[_NKR]       = NKR;
            _map[_NRA]       = NRA;
            Rating           = RATING;
        }

        public string Rating { get; init; }
    }
}
