namespace RuDataAPI.Extensions.Mapping
{
    /// <summary>
    ///     Represents the status of financial intrument / security.
    /// </summary>
    public enum InstrumentStatus
    {
        [EnumFieldStr("Не определено")]
        Na,

        [EnumFieldStr("В обращении", "Размещен")]
        InCirculation,

        [EnumFieldStr("Готовится", "Размещается")]
        InPreparation,

        [EnumFieldStr("Дефолт")]
        Defaulted,

        [EnumFieldStr("Досрочно погашен", "Погашен")]
        Redeemed,

        [EnumFieldStr("Аннулирован", "Прекращение", "Приостановлен", "Списано")]
        Canceled,
    }
}
