using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refl
{
    public class A
    {
        public A? ChildA { get; set; }
        public B ChildB { get; set; }
        public C ChildC { get; set; }

        public A()
        {
            ChildA = null;
            ChildB = new B();
            ChildC = new C();
        }
    }
}
