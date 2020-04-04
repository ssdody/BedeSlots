using BedeSlots.Data.Models;
using System.Collections.Generic;

namespace BedeSlots.Web
{
    public static class WebConstants
    {
        public const string UserRole = "User";
        public const string AdminRole = "Admin";
        public const string MasterAdminRole = "MasterAdmin";

        public const string AdminArea = "Admin";

        public const string AdminEmail = "admin@admin.com";
        public const string AdminName = "Administrator";

        public static Dictionary<Currency, string> CurrencySymbols = new Dictionary<Currency, string>
        {
            { Currency.USD, "$" },
            { Currency.EUR, "€" },
            { Currency.GBP, "£" },
            { Currency.BGN, "lv" }
        };

        public const double MaxAmount = 1000000d;
    }
}
