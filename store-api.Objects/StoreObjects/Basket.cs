using System;
using System.Collections.Generic;

namespace store_api.Objects.StoreObjects
{
    public class Basket : DataStoreItem
    {
        public Dictionary<string, string> ProductAndQuantity { get; set; }
        public bool HasPlacedOrder { get; set; }

        public string UserUid { get; set; }

        public DateTime? DateOrdered { get; set; }
    }
}