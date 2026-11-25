using System.Collections.Generic;

namespace AseanPhonebook
{
    public static class CountryHelper
    {
        // Maps Country Code -> Country Name
        public static readonly Dictionary<int, string> CodeToName = new Dictionary<int, string>
        {
            { 60, "Malaysia" },
            { 62, "Indonesia" },
            { 63, "Philippines" },
            { 65, "Singapore" },
            { 66, "Thailand" }
        };

        // Maps Search Menu Choice -> Country Code
        public static readonly Dictionary<int, int> MenuChoiceToCode = new Dictionary<int, int>
        {
            { 1, 63 }, // Philippines
            { 2, 66 }, // Thailand
            { 3, 65 }, // Singapore
            { 4, 62 }, // Indonesia
            { 5, 60 }  // Malaysia
        };

        public static bool IsValidCountryCode(int code)
        {
            return CodeToName.ContainsKey(code);
        }

        public static string GetCountryName(int code)
        {
            return CodeToName.TryGetValue(code, out string? name) ? name : "Unknown";
        }
    }
}