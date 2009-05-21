using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config
{
    public class ControllerActionConfig
    {
        public ControllerActionConfig(MethodInfo actionMethod, Type invokerType, Delegate actionFunc)
            : this(invokerType)
        {
            ActionMethod = actionMethod;
            ActionName = GetActionName(actionMethod);
            ControllerType = actionMethod.DeclaringType;
            ActionDelegate = actionFunc;
        }

        public ControllerActionConfig(Type invokerType)
        {
            InvokerType = invokerType;
            UniqueID = Guid.NewGuid().ToString();
            Behaviors = new List<Type>();
            OtherUrls = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);            
        }

        protected IList<Type> Behaviors { get; set; }
        protected HashSet<string> OtherUrls { get; set; }
        public Type InvokerType { get; set; }
        public Type ControllerType { get; set;}
        public Delegate ActionDelegate { get; set; }
        public MethodInfo ActionMethod { get; set; }
        public string ActionName { get; set; }
        public string PrimaryUrl { get; set; }

        public string UniqueID { get; private set; }

        public void UseViewFrom<CONTROLLER, OUTPUT>(Expression<Func<CONTROLLER, OUTPUT>> expression)
            where CONTROLLER : class
            where OUTPUT : class
        {
            ActionName = GetActionName(ReflectionHelper.GetMethod(expression));
        }

        //TODO: This is smelly, but how else do I get generic-typed stuff out of non-generic class like this?
        // I need to somehow carry along the generic type payload, without actually being generic
        public virtual Func<CONTROLLER, INPUT, OUTPUT> GetActionFunc<CONTROLLER, INPUT, OUTPUT>()
            where CONTROLLER : class
            where INPUT : class, new()
            where OUTPUT : class
        {
            return (Func<CONTROLLER, INPUT, OUTPUT>)ActionDelegate;
        }

        public virtual IEnumerable<string> GetOtherUrls() { return OtherUrls; }
        public virtual IEnumerable<Type> GetBehaviors() { return Behaviors; }

        public static string GetActionName(MethodInfo method)
        {
            return method.Name.ToLowerInvariant();
        }

        public bool IsTheSameActionAs<C>(Expression<Func<C, object>> otherFunc)
           where C : class
        {
            return ActionMethod == ReflectionHelper.GetMethod(otherFunc);
        }

        public virtual bool IsTheSameActionAs(ControllerActionConfig otherConfig)
        {
            return ActionMethod == otherConfig.ActionMethod;
        }

        public virtual void ApplyDefaultBehaviors(IEnumerable<Type> defaultBehaviors)
        {
            Behaviors.AddRange(defaultBehaviors);
        }

        public virtual void AddBehavior<BEHAVIOR>()
            where BEHAVIOR : IControllerActionBehavior
        {
            Behaviors.Add(typeof (BEHAVIOR));
        }

        public virtual void RemoveBehavior<BEHAVIOR>()
            where BEHAVIOR : IControllerActionBehavior
        {
            Behaviors.Remove(typeof (BEHAVIOR));
        }

        public virtual void RemoveAllBehaviors()
        {
            Behaviors.Clear();
        }

        public virtual void AddOtherUrl(string url)
        {
            OtherUrls.Add(url);
        }

        public virtual void RemoveOtherUrl(string url)
        {
            OtherUrls.Remove(url);
        }
    }
}
