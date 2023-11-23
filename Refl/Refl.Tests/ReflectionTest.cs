namespace Refl.Tests
{
    [TestClass]
    public class ReflectionTest
    {
        [TestMethod]
        public void TestCompiledSimpleIntGetter()
        {
            // ChildB.ChildC.MyInt
            A a = new A();
            var aGetter = Utils.CreateCompiledGetter<A, int>("ChildB.ChildC.MyInt");

            Assert.IsNotNull(aGetter);
            Assert.AreEqual(aGetter(a), a.ChildB.ChildC.MyInt);
        }

        [TestMethod]
        public void TestCompiledSimpleStringGetter()
        {
            // ChildB.ChildC.MyInt
            A a = new A();
            var aGetter = Utils.CreateCompiledGetter<A, string>("ChildB.ChildC.MyStr");

            Assert.IsNotNull(aGetter);
            Assert.AreEqual(aGetter(a), a.ChildB.ChildC.MyStr);
        }

        [TestMethod]
        public void TestCompiledSimpleBoolGetter()
        {
            // ChildB.ChildC.MyInt
            A a = new A();
            var aGetter = Utils.CreateCompiledGetter<A, bool>("ChildB.ChildC.IsLastClass");

            Assert.IsNotNull(aGetter);
            Assert.AreEqual(aGetter(a), a.ChildB.ChildC.IsLastClass);
        }

        [TestMethod]
        public void TestCompiledClassGetter()
        {
            // ChildB.ChildC.MyInt
            A a = new A();
            var aGetter = Utils.CreateCompiledGetter<A, B>("ChildB");

            Assert.IsNotNull(aGetter);
            Assert.AreEqual(aGetter(a), a.ChildB);
        }
    }
}