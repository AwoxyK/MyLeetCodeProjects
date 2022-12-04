using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NumberToEnglishWords
{
    public class NumberToEnglish
    {
        // Twenty Five Million Three Hundred Eighty Three One Hundred Eight
        // Twenty Five Million Three Hundred Eighty Three Thousand One Hundred Eight
        public static string Convert(uint number)
        {
            if (number == 0)
            {
                return "Zero";
            }

            string stringified = number.ToString();                                           // "1125"
            string[] numberCategories = GetNumberCategories(stringified).ToArray();           // { "125", "1" }
            List<string> words = new();

            int numberCategoriesCount = numberCategories.Length;

            for (int index = numberCategoriesCount - 1; index >= 0; index--)
            {
                string numberCategory = numberCategories[index];
                int categoryLevel = index + 1;

                string numberCategoryPronounce = GetNumberCategoryPronounce(numberCategory, categoryLevel);
                words.Add(numberCategoryPronounce);
            }


            return string.Join(' ', words).RemoveRepeatingWhitespace();
        }

        private static string GetNumberCategoryPronounce(string numberCategory, int categoryLevel = 1)
        {
            if (numberCategory.Length is >3 or <1)
            {
                throw new ArgumentException("Argument: numberCategory length is out of range [1-3]");
            }

            StringBuilder sb = new();
            bool hasHundred = false;
            bool areOnesUsed = false;

            if (numberCategory.Length >= 3)
            {
                string toAppend = GetDigitPronounce(numberCategory[0]) + " Hundred";
                sb.Append(toAppend);
                hasHundred = true;
            }

            sb.Append(" ");

            if (numberCategory.Length >= 2)
            {

                char tenDigit = hasHundred ? numberCategory[1] : numberCategory[0];
                char oneDigit = numberCategory.Last();
                string toAppend = tenDigit switch
                {
                     '8' => "Eighty",
                     '5' => "Fifty",
                     '4' => "Forty",
                     '3' => "Thirty",
                     '2' => "Twenty",
                     '1' => GetExceptionNumber(oneDigit, out areOnesUsed),
                     '0' => "",
                     _ => GetDigitPronounce(tenDigit) + "ty"  // 6 ("Sixty"), 7 ("Seventy") and 9 ("Ninety") are end with "ty"
                };

                sb.Append(toAppend);
            }
            
            if (!areOnesUsed)
            {
                sb.Append(" ");

                char oneDigit = numberCategory.Last();
                string toAppend = oneDigit switch
                {
                    '0' => "",
                    _ => GetDigitPronounce(oneDigit)
                };

                sb.Append(toAppend);
            }

            sb.Append(" ");

            string toAppendFinal = categoryLevel switch
            {
                4 => "Billion",
                3 => "Million",
                2 => "Thousand",
                1 => "",
                _ => throw new ArgumentException("Argument: categoryLevel is incorrect")
            };

            sb.Append(toAppendFinal);

            return sb.ToString().Trim();
        }

        private static string GetExceptionNumber(char oneDigit, out bool areOnesUsed)
        {
            if (oneDigit < '0' || oneDigit > '9')
            {
                throw new ArgumentException("Argument: oneDigit is out of range [0-9]");
            }

            areOnesUsed = true;

            string[] tensPronounce = new[]
            {
                "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
            };

            return tensPronounce[oneDigit - '0'];
        }

        private static string GetDigitPronounce(char digit) => digit switch
        {
            '0' => "Zero",
            '1' => "One",
            '2' => "Two",
            '3' => "Three",
            '4' => "Four",
            '5' => "Five",
            '6' => "Six",
            '7' => "Seven",
            '8' => "Eight",
            '9' => "Nine",
            _ => throw new ArgumentException("Argument: digit is out of range [0-9].")
        };

        private static IEnumerable<string> GetNumberCategories(string stringified)
        {
            string reversedStringified = new string(stringified.Reverse().ToArray());         // "5211"
            List<string> numberCategories = new();                                  // { "521", "1" } => { "125", "1" }

            string temp = string.Empty;

            foreach (char sym in reversedStringified)
            {
                temp += sym;
                if (temp.Length >= 3)
                {
                    numberCategories.Add(temp);
                    temp = string.Empty;
                }
            }

            if (temp.Length > 0)
            {
                numberCategories.Add(temp);
            }

            return numberCategories.Select(element => new string(element.Reverse().ToArray())).ToArray();
        }
    }
}