namespace RuDataAPI.Extensions.Mapping
{
    public enum CouponType
    {
        /// <summary>
        ///     Нулевой купон, дисконт.
        /// </summary>
        Zero,
        /// <summary>
        ///     Постоянный. Купоны известны для всех периодов и равны между собой.
        /// </summary>
        Constant,
        /// <summary>
        ///     Постоянный. Купоны известны для всех периодов и НЕ равны между собой.
        /// </summary>
        Fixed,
        /// <summary>
        ///     Плавающий купон. Определяется исходя из какого-либо индикатора. Для всех будущих периодов неизвестен.
        /// </summary>
        Floating,
        /// <summary>
        ///     Переменный купон. Купоны известны только до даты офферты.
        /// </summary>
        UpToOffer,
        /// <summary>
        ///     Прочее.
        /// </summary>
        Other
    }
}
