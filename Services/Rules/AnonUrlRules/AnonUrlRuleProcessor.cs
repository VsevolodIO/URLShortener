using System.Collections.Generic;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener.Services.Rules.AnonUrlRules
{
    public class AnonUrlRuleProcessor : IAnonUrlRuleProcessor
    {
        private readonly IEnumerable<IAnonUrlRule> _rules;

        public AnonUrlRuleProcessor(IEnumerable<IAnonUrlRule> rules)
        {
            _rules = rules;
        }

        public async Task<(bool, IEnumerable<string>)> PassesAllRulesAsync(AnonUrl anonUrl)
        {
            var passedRules = true;

            var errors = new List<string>();

            foreach (var rule in _rules)
            {
                if (!await rule.CompliesWithRuleAsync(anonUrl))
                {
                    errors.Add(rule.ErrorMessage);
                    passedRules = false;
                }
            }

            return (passedRules, errors);
        }
    }
}
