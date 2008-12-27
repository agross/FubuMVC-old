using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class AutoControllerConfiguration
    {
        private readonly HashSet<Type> _discoveredTypes = new HashSet<Type>();
        private readonly HashSet<DiscovererdAction> _discoveredActions = new HashSet<DiscovererdAction>();

        public void AddDiscoveredType(Type type)
        {
            _discoveredTypes.Add(type);
        }

        public IEnumerable<Type> GetDiscoveredTypes()
        {
            return _discoveredTypes.AsEnumerable();
        }

        public void AddDiscoveredAction(DiscovererdAction action)
        {
            _discoveredActions.Add(action);
        }

        public IEnumerable<DiscovererdAction> GetDiscoveredActions()
        {
            return _discoveredActions.AsEnumerable();
        }
    }

    public class DiscovererdAction : IEquatable<DiscovererdAction>
    {
        public Type ControllerType { get; set; }
        public Type InputType { get; set; }
        public Type OutputType{ get; set;}
        public MethodInfo Action { get; set; }

        public bool Equals(DiscovererdAction obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ControllerType, ControllerType) && Equals(obj.Action, Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (DiscovererdAction)) return false;
            return Equals((DiscovererdAction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ControllerType.GetHashCode()*397) ^ Action.GetHashCode();
            }
        }
    }
}