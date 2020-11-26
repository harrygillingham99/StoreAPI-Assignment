using System;
using System.Collections.Generic;

namespace store_api.Objects
{
    public class Basket : DataStoreItem
    {
        public List<int> SelectedProducts { get; set; }
        public bool HasPlacedOrder { get; set; }

        public string UserUid { get; set; }

        public DateTime? DateOrdered { get; set; }
    }
}