using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PotrebAuto.Servises
{
    /// <summary>
    /// Автоматически определяет номера столбцов Excel по заголовкам.
    /// Поддерживает многострочные заголовки (объединяет текст из нескольких строк),
    /// нечувствителен к регистру, пробелам и знакам препинания.
    /// </summary>
    public static class ColumnResolver
    {
        private static readonly Regex _nonAlphaNum = new Regex(@"[^\p{L}\p{N}]+", RegexOptions.Compiled);

        /// <summary>
        /// Нормализует строку для сравнения: нижний регистр, убирает знаки
        /// препинания и лишние пробелы. "ПУ, Гкал" → "пу гкал".
        /// </summary>
        public static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            return _nonAlphaNum.Replace(s.ToLowerInvariant().Trim(), " ").Trim();
        }

        // Строит "составной заголовок" для каждого столбца: объединяет непустые
        // ячейки из нескольких строк заголовка через пробел.
        // Например, если над столбцом: строка1="ПУ", строка2="с ВНР", строка3="Гкал" →
        // составной заголовок = "пу с внр гкал" (нормализованный).
        private static Dictionary<int, string> BuildCompoundHeaders(
            ExcelWorksheet worksheet, int headerRowStart, int headerRowEnd)
        {
            var headers = new Dictionary<int, string>();
            if (worksheet.Dimension == null) return headers;
            int maxCol = worksheet.Dimension.End.Column;

            for (int col = 1; col <= maxCol; col++)
            {
                var parts = new List<string>();
                for (int row = headerRowStart; row <= headerRowEnd; row++)
                {
                    var text = worksheet.Cells[row, col]?.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                        parts.Add(text);
                }
                if (parts.Any())
                    headers[col] = Normalize(string.Join(" ", parts));
            }
            return headers;
        }

        /// <summary>
        /// Определяет номера столбцов по заголовкам файла.
        /// </summary>
        /// <param name="worksheet">Лист Excel.</param>
        /// <param name="headerRowStart">Первая строка заголовка (1-based).</param>
        /// <param name="headerRowEnd">Последняя строка заголовка (1-based).</param>
        /// <param name="aliases">Словарь: имя поля → список известных вариантов заголовка.</param>
        /// <returns>Словарь: имя поля → номер столбца (1-based) или -1 если не найден.</returns>
        public static Dictionary<string, int> Resolve(
            ExcelWorksheet worksheet,
            int headerRowStart,
            int headerRowEnd,
            Dictionary<string, string[]> aliases)
        {
            var result = aliases.Keys.ToDictionary(k => k, _ => -1);
            if (worksheet.Dimension == null) return result;

            var compoundHeaders = BuildCompoundHeaders(worksheet, headerRowStart, headerRowEnd);

            // Индекс: нормализованный заголовок → первый столбец с таким заголовком.
            // Нужен для быстрого поиска по точному совпадению.
            var headerToCol = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var kvp in compoundHeaders)
            {
                if (!headerToCol.ContainsKey(kvp.Value))
                    headerToCol[kvp.Value] = kvp.Key;
            }

            // Проход 1: точное совпадение заголовка с псевдонимом.
            // Итерация: поле → псевдонимы (в порядке приоритета) → столбцы.
            // Так первый псевдоним в списке имеет приоритет, а не первый столбец.
            foreach (var fieldKvp in aliases)
            {
                string fieldName = fieldKvp.Key;
                foreach (var alias in fieldKvp.Value)
                {
                    string normAlias = Normalize(alias);
                    if (string.IsNullOrEmpty(normAlias)) continue;
                    if (headerToCol.TryGetValue(normAlias, out int col))
                    {
                        result[fieldName] = col;
                        break; // Нашли совпадение для этого поля — следующие псевдонимы не нужны
                    }
                }
            }

            // Проход 2: псевдоним целиком входит в заголовок (для составных длинных заголовков).
            // Требуем минимум 4 символа. Границы слов — пробелы вокруг псевдонима,
            // чтобы "номер" не находился внутри "неравномерности".
            // Та же логика: поле → псевдонимы → столбцы. Первый псевдоним имеет приоритет.
            var stillMissing = result.Where(r => r.Value == -1).Select(r => r.Key).ToHashSet();
            if (stillMissing.Any())
            {
                foreach (var fieldName in stillMissing.ToList())
                {
                    if (!aliases.TryGetValue(fieldName, out var fieldAliases)) continue;
                    bool found = false;
                    foreach (var alias in fieldAliases)
                    {
                        if (found) break;
                        string normAlias = Normalize(alias);
                        if (normAlias.Length < 4) continue;
                        foreach (var headerKvp in compoundHeaders)
                        {
                            string paddedHeader = " " + headerKvp.Value + " ";
                            if (paddedHeader.Contains(" " + normAlias + " "))
                            {
                                result[fieldName] = headerKvp.Key;
                                stillMissing.Remove(fieldName);
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Находит столбец, с которого начинается числовая последовательность 1, 2, 3...
        /// (дни месяца в строке заголовка). Возвращает -1 если не найдено.
        /// </summary>
        public static int DetectDatesStart(ExcelWorksheet worksheet, int headerRowStart, int headerRowEnd)
        {
            if (worksheet.Dimension == null) return -1;
            int maxCol = worksheet.Dimension.End.Column;

            for (int row = headerRowStart; row <= headerRowEnd; row++)
            {
                for (int col = 1; col <= maxCol - 2; col++)
                {
                    object v1 = worksheet.Cells[row, col]?.Value;
                    object v2 = worksheet.Cells[row, col + 1]?.Value;
                    object v3 = worksheet.Cells[row, col + 2]?.Value;

                    if (IsValue(v1, 1) && IsValue(v2, 2) && IsValue(v3, 3))
                        return col;
                }
            }
            return -1;
        }

        private static bool IsValue(object cellValue, int expected)
        {
            if (cellValue == null) return false;
            if (cellValue is double d) return Math.Abs(d - expected) < 0.01;
            return int.TryParse(cellValue.ToString(), out int i) && i == expected;
        }

        /// <summary>
        /// Аналог Resolve, но при неполном совпадении автоматически пробует расширить
        /// диапазон строк заголовка на 1 строку. Помогает работать с файлами, у которых
        /// заголовок занимает больше строк, чем указано в конфиге.
        /// </summary>
        public static Dictionary<string, int> ResolveExtending(
            ExcelWorksheet worksheet,
            int headerRowStart,
            int headerRowEnd,
            Dictionary<string, string[]> aliases,
            int extraRows = 1)
        {
            if (!aliases.Any())
                return aliases.Keys.ToDictionary(k => k, _ => -1);

            var best = Resolve(worksheet, headerRowStart, headerRowEnd, aliases);
            int bestFound = best.Count(kv => kv.Value > 0);
            int totalFields = best.Count;

            for (int extra = 1; extra <= extraRows && bestFound < totalFields; extra++)
            {
                var extended = Resolve(worksheet, headerRowStart, headerRowEnd + extra, aliases);
                int found = extended.Count(kv => kv.Value > 0);
                if (found > bestFound)
                {
                    best = extended;
                    bestFound = found;
                }
            }
            return best;
        }

        /// <summary>
        /// Возвращает имя поля → номер столбца, используя auto-detected значение
        /// или fallback из конфига.
        /// </summary>
        public static int GetColumnOrFallback(Dictionary<string, int> detected, string field, int fallback)
        {
            return detected.TryGetValue(field, out int col) && col > 0 ? col : fallback;
        }

        /// <summary>
        /// Показывает информационное предупреждение для полей, которые авто-определение
        /// не смогло найти по заголовкам. Вызывается когда UseAutoDetect=true.
        /// detected — результат Resolve/ResolveExtending, fields — (имя поля, отображаемое имя).
        /// </summary>
        public static void WarnAutoDetectMissed(
            string filePath,
            Dictionary<string, int> detected,
            (string Field, string DisplayName)[] fields)
        {
            var missed = new System.Collections.Generic.List<string>();
            foreach (var f in fields)
            {
                int col = detected.TryGetValue(f.Field, out int c) ? c : -1;
                if (col <= 0) missed.Add(f.DisplayName);
            }
            if (!missed.Any()) return;

            string fileName = System.IO.Path.GetFileName(filePath);
            System.Windows.MessageBox.Show(
                $"Файл: {fileName}\n\n" +
                $"Не найдены по заголовкам:\n• {string.Join("\n• ", missed)}\n\n" +
                "Используются номера столбцов из настроек в качестве запасного варианта.\n" +
                "Если данные некорректны — укажите номера вручную в настройках.",
                "Автоопределение: не найдены столбцы",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
    }
}
