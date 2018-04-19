using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant_Application.ActionEvents
{
    class ActionCommand : ICommand
    {
        public ActionCommand(Action<Object> action) : this(action, null)
        {

        }
        public ActionCommand(Action<Object> action, Predicate<Object> predicate)
        {
            if(action == null)
            {
                throw new ArgumentNullException("action", "Tanımlı bir Action<T> yok, zorunludur.");
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
