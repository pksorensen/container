// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Attributes;

namespace Unity.Tests.CollectionSupport
{
    public class TestClassWithDependencyTypeMethod
    {
        public TestClass[] Dependency { get; set; }

        [InjectionMethod]
        public void Injector(TestClass[] dependency)
        {
            Dependency = dependency;
        }
    }
}
