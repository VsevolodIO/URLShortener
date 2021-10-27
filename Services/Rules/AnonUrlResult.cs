using System.Collections.Generic;
using URLShortener.Models;

namespace URLShortener.Services.Rules
{
    public class AnonUrlResult
    {
        private AnonUrlResult(AnonUrl anonUrl, bool passedRules, IEnumerable<string> errors)
        {
            AnonUrl = anonUrl;
            UrlCreatingSuccessful = passedRules;
            Errors = errors;
        }

        public AnonUrl AnonUrl { get; }

        public bool UrlCreatingSuccessful { get; }

        public IEnumerable<string> Errors { get; }

        public static AnonUrlResult Success(AnonUrl anonUrl) => new AnonUrlResult(anonUrl, true, null);

        public static AnonUrlResult Failure(IEnumerable<string> errors) => new AnonUrlResult(null, false, errors);
    }
}
