using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace REG_MARK_LIB
{
    public class RegMark
    {
        private readonly string seriesChars = "ABCEHKMOPTYX";

        private readonly Regex markRule = new Regex(
            @"^(?<before>[abekmhopctyx])(?<number>[0-9]{3})(?<after>[abekmhopctyx]{2})(?<region>[0-9]{2,3})$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Проверяет валидность регистрационного номера.
        /// </summary>
        /// <param name="mark">регистрационный номер</param>
        /// <returns>true, если валиден</returns>
        public Boolean CheckMark(String mark)
        {
            return markRule.Match(mark).Success;
        }

        /// <summary>
        /// Получает следующий регистрационный номер
        /// </summary>
        /// <remarks>
        /// <example>
        ///     A999AA124 -> A000AB124
        ///     <code>
        ///         RegMark regMark = new RegMark();
        ///         string mark = GetNextMarkAfter("A999AA124");
        ///     </code>
        /// </example>
        /// </remarks>
        /// <param name="mark">регистрационный номер</param>
        /// <returns></returns>
        public String GetNextMarkAfter(String mark)
        {
            Match matched = markRule.Match(mark);
            // Если regMark не прошел проверку - возвращаем пустую строку.
            if (!matched.Success)
                return "";

            // Разбираем regMark
            string series = GetValue(matched, "before") + GetValue(matched, "after");
            int num = int.Parse(GetValue(matched, "number"), NumberStyles.Number);
            int length = seriesChars.Length;
            string result;

            if (num < 999)
            {
                // Если число меньше 999, то просто увеличиваем его.
                ++num;
                result = GetValue(matched, "before") + num.ToString("000") + GetValue(matched, "after");
            }
            else
            {
                // В противном случае возвращаем число к 000 и разбираем символы, если это возможно.
                int[] indexes = { seriesChars.IndexOf(series[0]), seriesChars.IndexOf(series[1]), seriesChars.IndexOf(series[2]) };
                if (indexes[0] < length)
                    result = series[0] + "000" + series[1] + seriesChars[indexes[2] + 1];
                else if (indexes[1] < length)
                    result = series[0] + "000" + seriesChars[indexes[1] + 1] + seriesChars[0];
                else if (indexes[0] < length)
                    result = seriesChars[indexes[0] + 1] + "000" + seriesChars[0] + seriesChars[0];
                else
                    result = seriesChars[0] + "000" + seriesChars[0] + seriesChars[0];
            }

            return result + GetValue(matched, "region");
        }

        /// <summary>
        /// Получает следующий регистрационный номер в переданном диапазоне.
        /// <para>Захватывает <c>rangeStart</c> и <c>rangeEnd</c></para>
        /// <para>Если prevMark не входит в диапазон, то возвращает <c>"Out of stock"</c></para>
        /// </summary>
        /// <param name="prevMark"></param>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        /// <returns></returns>
        public String GetNextMarkAfterInRange(String prevMark, String rangeStart, String rangeEnd)
        {
            if (GetCombinationsCountInRange(prevMark, rangeEnd) <= 0)
                return "out of stock";
            else if (GetCombinationsCountInRange(rangeStart, rangeEnd) <= 0)
                return "out of stock";
            return GetNextMarkAfter(prevMark);
        }

        public int GetCombinationsCountInRange(String mark1, String mark2)
        {
            return MarkToInt(markRule.Match(mark2)) - MarkToInt(markRule.Match(mark1));
        }

        private String GetValue(Match matched, String key)
        {
            if (matched.Success)
                return matched.Groups[key].Value;
            return "";
        }

        private int MarkToInt(Match matched)
        {
            int result = Convert.ToInt32(GetValue(matched, "number"));
            result += seriesChars.IndexOf(GetValue(matched, "after")[1]) * 1000;
            result += seriesChars.IndexOf(GetValue(matched, "after")[0]) * 1000 * seriesChars.Length;
            result += seriesChars.IndexOf(GetValue(matched, "before")[0]) * 1000 * seriesChars.Length * seriesChars.Length;
            return result;
        }
    }
}
