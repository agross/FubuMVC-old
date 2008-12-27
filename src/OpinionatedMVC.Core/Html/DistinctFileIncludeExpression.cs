using System;

namespace OpinionatedMVC.Core.Html
{
    public class DistinctFileIncludeExpression
    {
        private readonly string[] args;
        private readonly string includeTemplate;
        private readonly ICachedSet cachedSet;

        public DistinctFileIncludeExpression(ICachedSet cache, string includeTemplate, params string[] args)
        {
            this.args = args;
            this.includeTemplate = includeTemplate;
            cachedSet = cache;
        }

        public override string ToString()
        {
            var output = String.Format(includeTemplate, args);

            if (cachedSet.Contains(output)) return String.Empty;
            cachedSet.Add(output);

            return output;
        }
    }
}