using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram
{
    /// <summary>
    /// UI通知
    /// </summary>
    public class WpfNotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 普通通知
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 一次通知多个属性
        /// </summary>
        /// <param name="propertyNames"></param>
        protected virtual void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames != null)
                foreach (string name in propertyNames)
                {
                    if (!string.IsNullOrEmpty(name))
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }
        }

        /// <summary>
        /// 通过lambda表达式通知：格式this.RaisePropertyChanged(()=>oneProperty);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new Exception("propertyExpression can't be null!");
            }
            //获取lambd表达式的主体
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new Exception("propertyExpression is Wrong!");
            }
            //获取lambd主体的属性
            PropertyInfo propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new Exception("propertyExpression is Wrong!");
            }

            //获取属性的“公共”Get方法
            MethodInfo propGet = propInfo.GetGetMethod(true);
            if (propGet.IsStatic)
            {
                throw new Exception("The referentced property is a static property!");
            }
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

    }
}
