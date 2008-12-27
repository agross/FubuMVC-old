using System;
using System.Collections.Generic;

namespace OpinionatedMVC.Core.Html
{
    public interface ICachedSet
    {
        bool Contains(string item);
        void Add(string item);
    }

    public class CachedSet : ICachedSet
    {
        private readonly HashSet<string> hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        public bool Contains(string item)
        {
            return hash.Contains(item);
        }

        public void Add(string item)
        {
            hash.Add(item);
        }
    }
}
