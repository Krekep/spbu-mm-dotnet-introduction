using System.Formats.Asn1;
using System.Reflection;

namespace Refl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            var kek = Utils.CreateCompiledGetter<A, int>("ChildB.ChildC.MyInt");
            Console.WriteLine("Compiled = " + kek(a));
        }
    }
}
