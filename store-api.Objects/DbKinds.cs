using System.ComponentModel;

namespace store_api.Objects
{
    public static class DbKinds
    {
        public enum DbCollections
        {
            [Description("Categories")]
            Categories = 1,
            [Description("Products")]
            Products = 2,
            [Description("Baskets")]
            Baskets = 3,

        }
    }
}
