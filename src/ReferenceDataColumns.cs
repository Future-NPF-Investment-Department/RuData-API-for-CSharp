namespace RuDataAPI
{
    /// <summary>
    ///     Column names bitmask for EFIR FintoolReferenceData method response.     
    /// </summary>
    [Flags]
    public enum RefDataCols : long
    {
        ALL                   = long.MaxValue,      // ВСЕ ПОЛЯ
        ALLCODES              = 0x1F,               // ВСЕ КОДЫ (FINTOOLID, ISINCODE, MOEX_CODE, ISSUERINN, BORROWERINN)
                                                    
        FINTOOLID             = (long)1 << 0,       // Идентификатор финансового инструмента в базе Интерфакс
        ISINCODE              = (long)1 << 1,       // ISIN-код финансового инструмента
        MOEX_CODE             = (long)1 << 2,       // Идентификатор инструмента на Московской бирже (SECID)
        ISSUERINN             = (long)1 << 3,       // ИНН / TIN оператора выпуска финансового инструмента
        BORROWERINN           = (long)1 << 4,       // ИНН / TIN реального заемщика(эмитента) облигаций
        NICKNAME              = (long)1 << 5,       // Краткое наименование финансового инструмента
        ISSUERNAME_NRD        = (long)1 << 6,       // Название оператора выпуска финансового инструмента по базе НРД
        FINTOOLTYPE           = (long)1 << 7,       // Тип финансового инструмента (наименование)
        STATUS                = (long)1 << 8,       // Состояние выпуска финансового инструмента
        CURRENTFACEVALUE_NRD  = (long)1 << 9,       // Текущий номинал облигации с учетом амортизации по базе НРД
        FACEFTNAME            = (long)1 << 10,      // Валюта номинала финансового инструмента
        SUMMARKETVAL          = (long)1 << 11,      // Объем в обращении в валюте номинала
        COUPONTYPE            = (long)1 << 12,      // Вид купона
        COUPONRATE            = (long)1 << 13,      // Ставка текущего купона
        FIRSTCOUPONDATE       = (long)1 << 14,      // Дата начала первого купонного периода
        COUPONTYPENAME_NRD    = (long)1 << 15,      // Наименование типа купонного периода
        FLOATRATENAME         = (long)1 << 16,      // Определение плавающей процентной ставки по купону
        NUMCOUPONS            = (long)1 << 17,      // Количество купонов всего
        BASIS                 = (long)1 << 18,      // Базис НКД по базе Интерфакс
        ISSUERSECTOR          = (long)1 << 19,      // Сектор эмитента
        ISSUERCOUNTRY         = (long)1 << 20,      // Страна эмитента
        ENDMTYDATE            = (long)1 << 21,      // Дата погашения финансового инструмента
        BEGDISTDATE           = (long)1 << 22,      // Дата начала размещения
        ENDDISTDATE           = (long)1 << 23,      // Дата окончания размещения
        ISSUBORDINATED        = (long)1 << 24,      // Признак субординированных облигаций (1 - да)
        BONDSTRUCTURALPAR     = (long)1 << 25,      // Структурный параметр облигации
        SECURITIZATION        = (long)1 << 26,      // Секьюритизация
        HAVEINDEXEDFV         = (long)1 << 27,      // Облигация с индексируемым номиналом
        ISCONVERTIBLE         = (long)1 << 28,      // Наличие возможности конвертации / осуществленная конвертация в другой инструмент (1 - да)
        ISGUARANTEED          = (long)1 << 29,      // Признак наличия по выпуску гарантии (1-да)
        GUARANTVAL            = (long)1 << 30,      // Гарантированная сумма
        BORROWERNAME          = (long)1 << 31,      // Название реального заемщика (эмитента) облигаций
        BORROWERSECTOR        = (long)1 << 32,      // Сектор реального заемщика
        FACEVALUE             = (long)1 << 33,      // Начальный номинал финансового инструмента
        HAVEREPAYMENT         = (long)1 << 34,      // Наличие возможности досрочного погашения по инициативе эмитента
    }
}
