using System;
using System.Collections.Generic;

namespace store_api.Objects
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal PricePerUnit { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class RequestWrapper<T>
    {
        public T Request { get; set; }
        public string JwtToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class VoidRequest
    {
        public string JwtToken { get; set; }
    }
    public class ItemSelection
    {
        public List<int> SelectedItemKeys { get; set; }
    }
}
