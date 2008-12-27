using System;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.Behaviors;
using FubuMVC.Core.Behaviors;
using FubuMVC.Tests;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Behaviors
{
    [TestFixture]
    public class UnitOfWorkBehaviorTester
    {
        private IUnitOfWork _uow;
        private access_the_database_through_a_unit_of_work _behavior;
        private TestInputModel _input;
        private TestOutputModel _output;
        private IControllerActionBehavior _insideBehavior;
        private Func<TestInputModel, TestOutputModel> _actionFunc;

        [SetUp]
        public void SetUp()
        {
            _uow = MockRepository.GenerateMock<IUnitOfWork>();
            _insideBehavior = MockRepository.GenerateStub<IControllerActionBehavior>();
            _behavior = new access_the_database_through_a_unit_of_work(_uow) {InsideBehavior = _insideBehavior};
            _input = new TestInputModel();
            _output = new TestOutputModel();

            _actionFunc = i => new TestOutputModel();
        }

        private void executeTest()
        {
            _behavior.Invoke(_input, _actionFunc);
        }

        [Test]
        public void should_wrap_unit_of_work_around_action_and_dispose_when_finished()
        {
            executeTest();

            _insideBehavior.AssertWasCalled(b => b.Invoke(_input, _actionFunc));
            
            _uow.AssertWasCalled(u=>u.Dispose());
        }

        [Test]
        public void should_commit_the_unit_of_work_if_there_were_no_errors()
        {
            executeTest();

            _uow.AssertWasCalled(u=>u.Commit());
        }

        [Test]
        public void should_roll_back_the_unit_of_work_if_there_were_errors()
        {
            _insideBehavior.Stub(b => b.Invoke(_input, _actionFunc)).Throw(new InvalidOperationException());

            typeof (InvalidOperationException).ShouldBeThrownBy(executeTest);

            _uow.AssertWasCalled(u=>u.Rollback());
        }
    }
}