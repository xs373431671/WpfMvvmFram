using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram
{
    /// <summary>
    /// 弱委托
    /// 
    /// </summary>
    public class WeakDelegate
    {
        private WeakReference m_target;
        private MethodInfo m_method;
        public WeakDelegate(object methodSourceInstance, string methodName) :this(methodSourceInstance, methodSourceInstance.GetType().GetMethod(methodName))
        {

        }

        public WeakDelegate(object methodSourceInstance, MethodInfo method)
        {
            m_target = new WeakReference(methodSourceInstance);
            m_method = method;
        }

        /// <summary>
        /// 执行弱事件委托
        /// 使用方法：
        /// WeakDelegate d = new WeakDelegate(new ClassForDeclTest(), "PrintString");
        /// d.Invoke("abc");
        /// GC.Collect();
        /// d.Invoke("abc");
        /// </summary>
        /// <param name="args">方法需要的参数</param>
        /// <returns>返回值</returns>
        public object Invoke(params object[] args)
        {
            object target = m_target.Target;
            if (target != null) return m_method.Invoke(target, args);
            else return null;
        }

        /// <summary>
        /// 生成弱事件委托
        /// 使用方法：Func<string, int, string> d = w.ToDelegate() as Func<string, int, string>;
        /// string resutl1 = d.Invoke("wo", 2);
        /// </summary>
        /// <returns>Action or Func</returns>
        public Delegate ToDelegate()
        {
            ParameterExpression[] parExps = null;
            {
                ParameterInfo[] parInfos = m_method.GetParameters();
                parExps = new ParameterExpression[parInfos.Length];
                for (int i = 0; i < parExps.Length; ++i)
                {
                    parExps[i] = Expression.Parameter(parInfos[i].ParameterType, "p" + i);
                }
            }

            Expression target = Expression.Field(Expression.Constant(this), GetType().GetField("m_target", BindingFlags.Instance | BindingFlags.NonPublic));
            target = Expression.Convert(Expression.Property(target, "Target"), m_method.ReflectedType);

            Expression body =
            Expression.Condition(
            Expression.NotEqual(target, Expression.Constant(null)),
            Expression.Call(target, m_method, parExps),
            GetTypeDefaultExpression(m_method.ReturnType));

            return Expression.Lambda(body, parExps).Compile();
        }

        private static Expression GetTypeDefaultExpression(Type t)
        {
            if (t == typeof(void)) return Expression.Call(typeof(WeakDelegateHelper).GetMethod("EmptyFunc", BindingFlags.NonPublic | BindingFlags.Static));
            else if (t.IsClass) return Expression.Constant(null, t);
            else return Expression.Constant(t.InvokeMember(null, BindingFlags.CreateInstance, null, null, null));
        }
    }
}
