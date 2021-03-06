// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Exceptions;
using Unity.Tests;

namespace Unity.Container.Register.Tests
{
    [TestClass]
    public class MultipleConstructorTest
    {
        private IUnityContainer uc1 = new UnityContainer();

        /// <summary>
        /// Test with multiple constructors.
        /// </summary>
        [TestMethod]
        public void MultipleConstructorTestMethod()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ClassWithMultipleConstructor>();
            AssertHelper.ThrowsException<ResolutionFailedException>(() => container.Resolve<ClassWithMultipleConstructor>());
        }

        internal class ClassWithMultipleConstructor
        {
            private object constructorDependency;

            public ClassWithMultipleConstructor()
            {
            }

            public ClassWithMultipleConstructor(object constructorDependency)
            {
                this.constructorDependency = constructorDependency;
            }

            public ClassWithMultipleConstructor(string s)
            {
                constructorDependency = s;
            }

            public object ConstructorDependency
            {
                get { return constructorDependency; }
            }
        }
    }
}