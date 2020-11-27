namespace store_api.Objects
{
    public class AuthedRequest
    {
        public string JwtToken { get; set; }
    }

    public class AuthedRequestWrapper<T> : AuthedRequest
    {
        public T Request { get; set; }
    }
}
