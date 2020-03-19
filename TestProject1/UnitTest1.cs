using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestSolution;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<A> mock = new Mock<A>();
            mock.Setup(a => a.Do()).Returns("B");

            A a = mock.Object;
            
            Assert.AreEqual("A", a.Do());
        }
        
    }
}