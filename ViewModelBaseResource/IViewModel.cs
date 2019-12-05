using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMvvmFram.RuntimeContextResource;

namespace WpfMvvmFram.ViewModelBaseResource
{
    public interface IViewModel : IDisposable
    {
        IView View { get; }

        IContext Context { get; set; }

        /// <summary>
        /// View加载完成后自动执行
        /// </summary>
        //void WindowLoaded(object sender, RoutedEventArgs e);

        /// <summary>
        /// View关闭时自动执行
        /// </summary>
        void WindowClosing(object sender, CancelEventArgs e);

        void ViewLoaded(object sender, RoutedEventArgs e);
        void ViewUnloaded(object sender, RoutedEventArgs e);

        bool? ShowDialog(string tittle, bool isSingleView, bool isSetOwner, bool isCanResize, bool isMaxWindow);
    }
}
