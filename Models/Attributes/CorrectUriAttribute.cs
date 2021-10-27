using System;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models.Attributes
{
    public class CorrectUriAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            return value != null && IsValidUri(value.ToString());
        }

        public bool IsValidUri(string uri) => Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;

    }
}
