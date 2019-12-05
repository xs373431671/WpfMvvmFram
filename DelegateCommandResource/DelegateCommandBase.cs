using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmFram.DelegateCommandResource
{
    public abstract class DelegateCommandBase : Prism.Commands.DelegateCommandBase
    {
        //private readonly Action<object> m_ExecuteMethod;
        //private readonly Func<object, bool> m_CanExecuteMethod;
        //private bool m_IsActive;


        //public virtual event EventHandler IsActiveChanged;
        //public event EventHandler CanExecuteChanged
        //{
        //    add
        //    {
        //        DelegateCommand
        //    }
        //    remove
        //    {

        //    }
        //}




        //protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        //{
        //    if (executeMethod == null || canExecuteMethod == null)
        //    {
        //        throw new Exception("executeMethod or canExecuteMethod can't be null!");
        //    }
        //    this.m_ExecuteMethod = executeMethod;
        //    this.m_CanExecuteMethod = canExecuteMethod;
        //}

        //public bool IsActive
        //{
        //    get { return m_IsActive; }
        //    set
        //    {
        //        m_IsActive = value;
        //        IsActiveChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}


        //public bool CanExecute(object parameter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(object parameter)
        //{
        //    throw new NotImplementedException();
        
    }
}
