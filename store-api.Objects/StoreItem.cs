using System;

namespace store_api.Objects
{
    public class StoreItem
    {
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

    }

    public class RequestWrapper<T>
    {
        public T Request { get; set; }
        public string JwtToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
