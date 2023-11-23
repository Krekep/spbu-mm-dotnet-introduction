using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refl
{
    public class C
    {
        public int MyInt { get; set; }
        public string MyStr { get; set; }
        public bool IsLastClass { get; set; }

        public C() 
        {
            MyInt = 42;
            MyStr = "Hello, World!";
            IsLastClass = true;
        }
    }
}
