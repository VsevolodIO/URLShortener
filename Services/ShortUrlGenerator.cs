using System;
using System.Linq;


namespace URLShortener.Services
{
    public class ShortUrlGenerator : IUrlGenerator
    {
        private static readonly Random Random = new Random();
        public string GetRandomShortUrl()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
