using System.Formats.Asn1;
using System.Reflection;

namespace Refl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.ChildA = new A();
            var getForClass = Utils.GenerateGetFuncForClass<A, object>(a);
            var setForClass = Utils.GenerateSetFuncForClass<A, object>(a);

            Console.WriteLine("Different paths for one class");
            Console.WriteLine("ChildB.ChildC.MyInt = " + getForClass("ChildB.ChildC.MyInt"));
            Console.WriteLine("ChildB.ChildC.MyStr = " + getForClass("ChildB.ChildC.MyStr"));
            Console.WriteLine("ChildB.ChildC.IsLastClass = " + getForClass("ChildB.ChildC.IsLastClass"));
            Console.WriteLine("ChildA.ChildB.ChildC.MyInt = " + getForClass("ChildA.ChildB.ChildC.MyInt"));

            setForClass("ChildB.ChildC.MyInt", 24);
            Console.WriteLine("After change value ChildB.ChildC.MyInt = " + getForClass("ChildB.ChildC.MyInt"));

            Console.WriteLine("\nSingle path for different classes");
            var getByPath = Utils.GenerateGetFuncByPath<A, int>("ChildB.ChildC.MyInt");
            var setByPath = Utils.GenerateSetFuncByPath<A, int>("ChildB.ChildC.MyInt");

            Console.WriteLine("Before change = " + getByPath(a));

            setByPath(a, 12);
            Console.WriteLine("After change = " + getByPath(a));

            A anotherA = new A();
            Console.WriteLine("Value for another A = " + getByPath(anotherA));
        }
    }
}
