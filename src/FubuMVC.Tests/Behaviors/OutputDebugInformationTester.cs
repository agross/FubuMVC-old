using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class OutputDebugInformationTester
    {
        private OutputDebugInformation _behavior;
        private TestOutputModel _outputModel;

        [SetUp]
        public void SetUp()
        {
            _outputModel = new TestOutputModel();
            _behavior = new OutputDebugInformation(new FubuConventions(), new FubuConfiguration(new FubuConventions()))
            {
                InsideBehavior = new DefaultBehavior()
            };
        }

        [Test]
        public void should_render()
        {
            _behavior.Invoke(new TestInputModel(), i => _outputModel);
            _behavior.Result.ShouldBeOfType<RenderDebugInformationResult>();
        }
    }
}