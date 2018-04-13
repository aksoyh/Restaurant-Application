using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.ActionEvents
{
    class ActionCommand
    {
        private Func<object, object> p;

        public ActionCommand(Func<object, object> p)
        {
            this.p = p;
        }
    }
}
