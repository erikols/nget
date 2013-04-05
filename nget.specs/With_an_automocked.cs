using System;
using System.Linq.Expressions;
using Machine.Specifications;
using Moq;
using Moq.Language.Flow;
using StructureMap.AutoMocking;

namespace PS.Utilities.Specs
{
    [Subject(typeof (object))]
    public abstract class With_an_automocked<T> where T : class
    {
        Establish context = () =>
            {
                autoMocker = new MoqAutoMocker<T>();
            };

        protected static T ClassUnderTest
        {
            get { return autoMocker.ClassUnderTest; }
        }

        protected static Mock<TMock> GetTestDouble<TMock>()
            where TMock : class
        {
            return Mock.Get(autoMocker.Get<TMock>());
        }

        protected static void Inject<TMock>(TMock target)
        {
            autoMocker.Inject(target);
        }

        protected static void Verify<TMock>(Expression<Action<TMock>> expression) where TMock : class
        {
            GetTestDouble<TMock>().Verify(expression);
        }

        protected static void Verify<TMock>(Expression<Action<TMock>> expression, Times times) where TMock : class
        {
            GetTestDouble<TMock>().Verify(expression, times);
        }

        protected static void Stub<TMock, TResult>(TResult returnValue, Expression<Func<TMock, TResult>> expression) where TMock : class
        {
            GetTestDouble<TMock>().Setup(expression).Returns(returnValue);
        }
        
        protected static ISetup<TMock, TResult> Stub<TMock, TResult>(Expression<Func<TMock, TResult>> expression) where TMock : class
        {
            return GetTestDouble<TMock>().Setup(expression);
        }

        static AutoMocker<T> autoMocker;
    }
}