using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refl
{
    public class B
    {
        public B? ChildB { get; set; }
        public C ChildC { get; set; }

        public B()
        {
            ChildB = null;
            ChildC = new C();
        }
    }
}
