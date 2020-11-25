using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        }
    }
}
