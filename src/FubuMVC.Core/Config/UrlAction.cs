using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.Core.Config
{
    public class UrlAction
    {
        private readonly HashSet<string> _otherUrlStubs = new HashSet<string>();

        public UrlAction()
        {
            UniqueId = Guid.NewGuid();
        }

        public Guid UniqueId { get; private set; }

        public string PrimaryUrlStub { get; set; }
        public IEnumerable<string> OtherUrlStubs { get { return _otherUrlStubs; } }

        public IEnumerable<string> AllUrlStubs { get { return new[] {PrimaryUrlStub}.Concat(_otherUrlStubs); } }

        public void AddOtherUrlStub(string urlStub)
        {
            _otherUrlStubs.Add(urlStub);
        }

        public void RemoveOtherUrlStub(string urlStub)
        {
            _otherUrlStubs.Remove(urlStub);
        }
     

        #region Equality Members

        public bool Equals(UrlAction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.UniqueId.Equals(UniqueId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UrlAction)) return false;
            return Equals((UrlAction) obj);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        #endregion
    }
}