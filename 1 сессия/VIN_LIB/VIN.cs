using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace VIN_LIB
{
    public class VIN
    {
        // Словарь содержит в себе модельные годы автомобилей, начиная с 1980-го
        // A: 1980; B: 1981, etc.
        private Dictionary<char, int> yearsModel = new Dictionary<char, int>();

        // Словарь countries содержит в себе информацию о странах по их кодам.
        // AA: ЮАР; 1A: США, etc.
        private Dictionary<string, string> countries = new Dictionary<string, string>();

        // Регулярка ниже парсит VIN и режет на отдельные куски.
        // wmi - World Manufacturers entification
        // vds - vehicle description section
        // sign - Control sign
        // modelYear - vehicle model year
        // vis - vehicle identification section
        public readonly Regex vinRule = new Regex(
            @"^(?<wmi>[a-z1-9-[oiq]]{3})(?<vds>[a-z0-9-[oiq]]{5})(?<sign>[0-9x]{1})(?<modelYear>[a-y1-9-[oiqu]]{1})(?<vis>[a-z0-9-[oiq]]{7})$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Сгенерированные коды стран с их названиями.
        private readonly string countryCodes =
            "AA-AH ЮАРAJ-AN Котд'Ивуар;BA-BE Ангола;BF-BK Кения;BL-BR Танзания;CA-CE Бенин;CF-CK Мадагаскар;CL-CR Тунис;DA-DE " +
            "Египет;DF-DK Марокко;DL-DR Замбия;EA-EE Эфиопия;EF-EK Мозамбик;FA-FE Гана;FF-FK Нигерия;JA-JT Япония;KA-KE " +
            "ШриЛанка;KF-KK Израиль;KL-KR Южная Корея;KS-K0 Казахстан;LA-L0 Китай;MA-ME Индия;MF-MK Индонезия;ML-MR " +
            "Таиланд;NF-NK Пакистан;NL-NR Турция;PA-PE Филиппины;PF-PK Сингапур;PL-PR Малайзия;RA-RE ОАЭ;RF-RK Тайвань;RL-RR " +
            "Вьетнам;RS-R0 Саудовска я Аравия;SA-SM Великобритания;SN-ST Германия;SU-SZ Польша;S1-S4 Латвия;TA-TH " +
            "Швейцария;TJ-TP Чехия;TR-TV Венгрия;TW-T1 Португалия;UH-UM Дания;UN-UT Ирландия;UU-UZ Румыния;U5-U7 " +
            "Словакия;VA-VE Австрия;VF-VR Франция;VS-VW Испания;VX-V2 Сербия;V3-V5 Хорватия;V0-V6 Эстония;WA-W0 " +
            "Германия;XA-XE Болгария;XF-XK Греция;XL-XR Нидерланды;XS-XW СССР/СНГ;XX-X2 Люксембург;X0-X3 Россия;YA-YE " +
            "Бельгия;YF-YK Финляндия;YL-YR Мальта;YS-YW Швеция;YX-Y2 Норвегия;Y3-Y5 Беларусь;Y0-Y6 Украина;ZA-ZR Италия;" +
            "1A-10 США;2A-20 Канада;3A-3W Мексика;3X-37 Коста Рика;30-38 Каймановы острова;4A-40 США;5A-50 США;6A-6W " +
            "Австралия;7A-7E Новая Зеландия;8A-8E Аргентина;8F-8K Чили;8L-8R Эквадор;8S-8W Перу;8X-82 Венесуэла;9A-9E " +
            "Бразилия;9F-9K Колумбия;9L-9R Парагвай;9S-9W Уругвай;9X-92 Тринидад и Тобаго;93-99 Бразилия;ZX-Z2 " +
            "Словения;Z3-Z5 Литва;Z0-Z6 Россия";

        /// <summary>
        /// Простой конструктор, заполняющий словарь yearsModel при инициализации.
        /// </summary>
        public VIN()
        {
            FillYears();
            FillCountries();
        }

        /// <summary>
        /// Проверяет валидность VIN
        /// </summary>
        /// <param name="vin">VIN, требующий проверки</param>
        /// <returns>true, если VIN валиден</returns>
        public Boolean CheckVIN(String vin)
        {
            return vinRule.Match(vin).Success;
        }

        /// <summary>
        /// Получает страну-изготовитель транспорта
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public String GetVINCountry(String vin)
        {
            string countryCode = GetValue(vinRule.Match(vin), "wmi");
            foreach (KeyValuePair<string, string> entry in countries)
            {
                if (new Regex(entry.Key, RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(countryCode).Success)
                    return entry.Value;
            }
            return "";
        }

        /// <summary>
        /// Получает год изготовления транспорта
        /// </summary>
        /// <param name="vin">VIN, прошедший проверку регуляркой</param>
        /// <returns>год от 1980</returns>
        public int GetTransportYear(String vin)
        {
            return GetTransportYear(vinRule.Match(vin));
        }
//////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Разбирает значения, полученные с matched.
        /// </summary>
        /// <param name="matched">VIN, прошедший проверку регуляркой</param>
        /// <param name="key">Требуемое значение</param>
        /// <returns>Полученное значение</returns>
        public String GetValue(Match matched, String key)
        {
            if (matched.Success)
                return matched.Groups[key].Value;
            return "";
        }

        private int GetTransportYear(Match matched)
        {
            if (!matched.Success)
                return -1;
            char val = matched.Groups["modelYear"].Value[0];
            return yearsModel[val];
        }

        /// <summary>
        /// Заполняет словарь yearsModel.
        /// </summary>
        private void FillYears()
        {
            int year = 1980;
            for (int i = 65; i <= 89; ++i)  // A..Z
            {
                if (i == 81 || i == 79 || i == 73 || i == 85)
                    continue;
                yearsModel.Add((char)i, year);
                ++year;
            }
            for (int i = 49; i <= 57; ++i)  // 0..9
            {
                yearsModel.Add((char)i, year);
                ++year;
            }
        }

        /// <summary>
        /// Заполняет словарь countries.
        /// </summary>
        private void FillCountries()
        {
            string[] codes = countryCodes.Split(';');
            foreach (string code in codes)
            {
                char[] sep = { ' ' };
                string[] codeInfo = code.Split(sep, 2);
                // Преобразует AA-AH в правило A[A-H]
                if ((codeInfo[0][1] >= 'A' && codeInfo[0][1] <= 'Z') && (codeInfo[0][4] >= '0' && codeInfo[0][4] <= '9'))
                    countries.Add(
                        codeInfo[0][0] + "[" + codeInfo[0][1] + "-Z0-" + codeInfo[0][4] + "]",
                        codeInfo[1]);
                else
                    countries.Add(
                        codeInfo[0][0] + "[" + codeInfo[0][1] + "-" + codeInfo[0][4] + "]",
                        codeInfo[1]);
            }
        }
    }
}
