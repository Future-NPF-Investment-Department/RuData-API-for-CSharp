namespace RuDataAPI.Extensions.Mapping
{
    public enum IssuerSector
    {
        /// <summary>
        ///     Не определен.
        /// </summary>
        Undefined,

        /// <summary>
        ///     Банки.
        /// </summary>
        [EnumFieldStr("Банки")]
        Banks,

        /// <summary>
        ///     Горнодобыча.
        /// </summary>
        [EnumFieldStr("Горнодобыча")]
        Mining,

        /// <summary>
        ///     Государственные.
        /// </summary>
        [EnumFieldStr("Государственные")]
        Sovereign,

        /// <summary>
        ///     ЖКХ.
        /// </summary>
        [EnumFieldStr("ЖКХ")]
        HousingServices,

        /// <summary>
        ///     Леспром.
        /// </summary>
        [EnumFieldStr("Леспром, бумага")]
        Forestry,

        /// <summary>
        ///     Легкая промышленность.
        /// </summary>
        [EnumFieldStr("Лёгкая промышленность")]
        LightIndustry,

        /// <summary>
        ///     Машиностроение.
        /// </summary>
        [EnumFieldStr("Машиностроение")]
        Machinery,

        /// <summary>
        ///     Медицина и здоровье. (Не фармацевтика!)
        /// </summary>
        [EnumFieldStr("Медицина, здоровье")]
        Healthcare,

        /// <summary>
        ///     Металлургия
        /// </summary>
        [EnumFieldStr("Металлургический")]
        Metallurgical,

        /// <summary>
        ///     Недвижимость
        /// </summary>
        [EnumFieldStr("Недвижимость")]
        RealEstate,

        /// <summary>
        ///     Автомобили и запчасти. (Не машиностроение!)
        /// </summary>
        [EnumFieldStr("Автомобили и запчасти")]
        CarIndustry,

        /// <summary>
        ///     Нефтегазовый.
        /// </summary>
        [EnumFieldStr("Нефтегазовый")]
        OilAndGas,

        /// <summary>
        ///     Пищевая промышленность и с/х.
        /// </summary>
        [EnumFieldStr("Пищевая промышленность и с/х")]
        Food,

        /// <summary>
        ///     Туризм, отдых, досуг.
        /// </summary>
        [EnumFieldStr("Путешествия, досуг")]
        Tourism,

        /// <summary>
        ///     Ретейл.
        /// </summary>
        [EnumFieldStr("Ритейл")]
        Retail,

        /// <summary>
        ///     СМИ.
        /// </summary>
        [EnumFieldStr("СМИ")]
        Media,

        /// <summary>
        ///     Страхование.
        /// </summary>
        [EnumFieldStr("Страхование")]
        Insurance,

        /// <summary>
        ///     Строительство.
        /// </summary>
        [EnumFieldStr("Строительство")]
        Building,

        /// <summary>
        ///     Телекоммуникации.
        /// </summary>
        [EnumFieldStr("Телекоммуникации")]
        Telecom,

        /// <summary>
        ///     Технологии.
        /// </summary>
        [EnumFieldStr("Технологии")]
        Tech,

        /// <summary>
        ///     Транспорт.
        /// </summary>
        [EnumFieldStr("Транспорт")]
        Transport,

        /// <summary>
        ///     Фармацевтика.
        /// </summary>
        [EnumFieldStr("Фармацевтика")]
        Pharmacy,

        /// <summary>
        ///     Финансовый сервис.
        /// </summary>
        [EnumFieldStr("Финанс.сервисы")]
        Finance,

        /// <summary>
        ///     Финансовый сервис - ИК, ФК.
        /// </summary>
        [EnumFieldStr("Фин.сервис")]
        Investing,

        /// <summary>
        ///     Финансовый сервис - Лизинг.
        /// </summary>
        [EnumFieldStr("Фин.сервис - Лизинг")]
        Leasing,

        /// <summary>
        ///     Финансовый сревис - НПФ, УК, ПИФ.
        /// </summary>
        [EnumFieldStr("Фин.сервис - УК, НПФ, ПИФ")]
        Pension,

        /// <summary>
        ///     Финасовый сервис - ИА.
        /// </summary>
        [EnumFieldStr("Фин.сервис - ИА")]
        MortgageAgent,

        /// <summary>
        ///     Химпром.
        /// </summary>
        [EnumFieldStr("Химпром, минудобрения")]
        Chemicals,

        /// <summary>
        ///     Электроэнергетика.
        /// </summary>
        [EnumFieldStr("Электроэнергетика")]
        ElectricPower,

        /// <summary>
        ///     Прочее.
        /// </summary>
        [EnumFieldStr("Прочие")]
        Other
    }
}
