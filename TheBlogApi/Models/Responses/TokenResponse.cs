using System;

namespace TheBlogApi.Models.Responses
{
    public class TokenResult
    {
        public TokenResult(int statusCode, string statusDescription = null)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public TokenData TokenData { get; set; }
    }

    public class TokenData
    {
        public string Token { get; set; }
        public DateTime ExpiredDateUtc { get; set; }
    }
}
