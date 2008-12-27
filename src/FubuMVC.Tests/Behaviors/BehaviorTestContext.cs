using System;
using NUnit.Framework;
using FubuMVC.Core.Behaviors;

namespace FubuMVC.Tests.Behaviors
{
    public abstract class BehaviorTestContext<BEHAVIOR>
        where BEHAVIOR : IControllerActionBehavior
    {
        protected BEHAVIOR _behavior;
        protected TestController _controller;
        protected TestOutputModel _outputModel;
        protected object _input;

        protected virtual BEHAVIOR CreateBehavior()
        {
            return Activator.CreateInstance<BEHAVIOR>();
        }

        [SetUp]
        public void SetUp()
        {
            _behavior = CreateBehavior();
            _controller = new TestController();
            _input = new TestInputModel();
            _outputModel = new TestOutputModel();

            beforeEach();
        }

        [TearDown]
        public void TearDown()
        {
            afterEach();
        }

        protected virtual void beforeEach() { }
        protected virtual void afterEach() { }

    }
}