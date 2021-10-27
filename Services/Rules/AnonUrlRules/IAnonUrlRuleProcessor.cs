using System.Collections.Generic;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener.Services.Rules.AnonUrlRules
{
    public interface IAnonUrlRuleProcessor
    {
        Task<(bool, IEnumerable<string>)> PassesAllRulesAsync(AnonUrl anonUrl);
    }
}
