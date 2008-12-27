using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Controller;

namespace OpinionatedMVC.Core
{
    public interface IUrlProvider
    {
        string UrlFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> expression)
            where CONTROLLER : IOpinionatedController;

        string UrlForAction<CONTROLLER, INMODEL>(Expression<Func<CONTROLLER, Func<INMODEL, object>>> expression, object id)
            where CONTROLLER : IOpinionatedController;
    }
}