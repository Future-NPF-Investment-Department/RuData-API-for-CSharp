namespace RuDataAPI.Extensions.Mapping
{
    public enum CouponType
    {
        /// <summary>
        ///     Нулевой купон, дисконт.
        /// </summary>
        [EnumFieldStr("Дисконтный", "Нулевой")]
        Zero,

        /// <summary>
        ///     Постоянный. Купоны известны для всех периодов и равны между собой.
        /// </summary>
        [EnumFieldStr("Постоянный")]
        Constant,

        /// <summary>
        ///     Фиксированный. Купоны известны для всех периодов и НЕ равны между собой.
        /// </summary>
        [EnumFieldStr("Фиксированный")]
        Fixed,

        /// <summary>
        ///     Плавающий купон. Определяется исходя из какого-либо индикатора. Для всех будущих периодов неизвестен.
        /// </summary>
        [EnumFieldStr("Плавающий")]
        Floating,

        /// <summary>
        ///     Переменный купон. Купоны известны только до даты офферты.
        /// </summary>
        [EnumFieldStr("Переменный")]
        UpToOffer,

        /// <summary>
        ///     Прочее.
        /// </summary>
        [EnumFieldStr("Прочий", "Ипотечный", "Возможный рост ставки")]
        Other
    }
}
