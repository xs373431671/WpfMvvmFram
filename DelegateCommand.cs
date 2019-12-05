using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram
{
    public class DelegateCommand : Prism.Commands.DelegateCommand
    {

        public DelegateCommand(Action executeMethod) : base(executeMethod)
        {

        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {

        }
    }
    public class DelegateCommand<T> : Prism.Commands.DelegateCommand<T>
    {
        public DelegateCommand(Action<T> executeMethod) : base(executeMethod)
        {

        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {

        }

    }
}
