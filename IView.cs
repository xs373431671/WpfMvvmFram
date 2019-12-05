using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfMvvmFram
{
    public interface IView
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// UI线程
        /// </summary>
        Dispatcher Dispatcher { get; }
    }
}
