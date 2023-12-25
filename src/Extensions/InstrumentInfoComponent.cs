
namespace RuDataAPI.Extensions
{
    [Flags]
    public enum InstrumentInfoComponent
    {
        Core            = 1 << 0,
        Flows           = 1 << 1,
        TradeHistory    = 1 << 2,
        Ratings         = 1 << 3
    }
}
