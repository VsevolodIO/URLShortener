using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener.Services.Rules.AnonUrlRules
{
    public interface IAnonUrlRule
    {
        Task<bool> CompliesWithRuleAsync(AnonUrl anonUrl);

        string ErrorMessage { get; }
    }
}
