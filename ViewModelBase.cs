using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfMvvmFram.RuntimeContextResource;
using WpfMvvmFram.ViewModelBaseResource;

namespace WpfMvvmFram
{
    public abstract class ViewModelBase : WpfNotificationObject, IViewModel
    {
        #region 构造函数
        public ViewModelBase(IView view)
        {
            if (view == null)
            {
                throw new Exception("view can't be null");
            }
            this.View = view;
            this.View.DataContext = this;
            RegisterViewLoadedAndUnLoaded();
            //RegisterWindowLoadedAndClosing();


        }
        public ViewModelBase(IView view, IContext context)
        {
            if (view == null)
            {
                throw new Exception("view can't be null");
            }
            this.View = view;
            this.View.DataContext = this;
            this.Context = context;
            RegisterViewLoadedAndUnLoaded();
            //RegisterWindowLoadedAndClosing();
        }
        #endregion



        #region 公共属性
        public IView View { get; }
        public IContext Context { get; set; }
        #endregion



        #region 在VM中操作View的一些方法
        public void WindowShellShowOrHide(bool isHide)
        {
            Window win = GetViewWindowShell();
            
            if (win != null)
            {
                if (isHide)
                {
                    win.Hide();
                }
                else
                {
                    win.Show();
                }
            }
        }

        #endregion





        #region 私有方法
        private void RegisterWindowClosing()
        {
            Window win = GetViewWindowShell();           
            win.Closing += this.WindowClosing;
        }
        private void RegisterViewLoadedAndUnLoaded()
        {
            UserControl uc = View as UserControl;
            uc.Loaded += this.ViewLoaded;
            uc.Unloaded += this.ViewUnloaded;
        }

        

        private Window GetViewWindowShell()
        {
            FrameworkElement uctl = this.View as FrameworkElement;
            while (uctl.Parent != null)
            {
                //在将最外层window转化为UserControl时会报错，会执行Catch
                uctl = uctl.Parent as FrameworkElement;
                if (uctl is Window)
                {
                    break;
                }
            }
            return uctl as Window;
        }
        #endregion






        #region 子类可以重写的方法
        public virtual void Dispose()
        {

        }

        public virtual void WindowClosing(object sender, CancelEventArgs e)
        {

        }

        //public virtual void WindowLoaded(object sender, RoutedEventArgs e)
        
        public virtual void ViewUnloaded(object sender, RoutedEventArgs e)
        {

        }

        public virtual void ViewLoaded(object sender, RoutedEventArgs e)
        {
            RegisterWindowClosing();
        }

        public virtual bool? ShowDialog(string tittle,bool isSingleView,bool isSetOwner,bool isCanResize,bool isMaxWindow)
        {
            UserControl uctl = this.View as UserControl;
            Window shell = new Window();
            shell.Content = uctl;
            shell.Width = uctl.Width;
            shell.Height = uctl.Height;
            shell.ResizeMode = isCanResize ? ResizeMode.CanResize : ResizeMode.NoResize;
            shell.WindowState = isMaxWindow?WindowState.Maximized: WindowState.Normal;
            shell.Title = tittle;
            return shell.ShowDialog();
        }
        #endregion

    }


    public class ViewModelBase<TView>:ViewModelBase where TView:IView
    {
        public ViewModelBase(TView view):base(view)
        {
             
        }
        public ViewModelBase(TView view, IContext context):base(view,context)
        {
            
        }
    }
}
