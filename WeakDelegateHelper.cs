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
    /// 弱事件(生成的委托单次调用上比WeakDelegate的ToDelegate要慢，但是多委托的话性能要好）
    /// 使用方法：
    /// Student stu = new Student();
    /// Func<string, int, string> d = WeakDelegateHelper.CreateDelegate(stu, "Say") as Func<string, int, string>;
    /// string resutl1 = d.Invoke("wo", 2);
    /// </summary>
    public static class WeakDelegateHelper
    {
        private static Dictionary<MethodInfo, Func<Func<object>, Delegate>> g_delegateCache = new Dictionary<MethodInfo, Func<Func<object>, Delegate>>();

        #region 工具方法
        /// <summary>
        /// 使用方法：
        /// Student stu = new Student();
        /// Func<string, int, string> d = WeakDelegateHelper.CreateDelegate(stu, "Say") as Func<string, int, string>;
        /// string resutl1 = d.Invoke("wo", 2);
        /// </summary>
        /// <param name="methodSourceInstance">方法所在的实例</param>
        /// <param name="methodInfo">方法名</param>
        /// <returns>Action or Func</returns>
        public static Delegate CreateDelegate(object methodSourceInstance, MethodInfo methodInfo)
        {
            Func<Func<object>, Delegate> d;
            if (!g_delegateCache.TryGetValue(methodInfo, out d))
            {
                d = GenerateDelegateImpl(methodInfo);
                g_delegateCache.Add(methodInfo, d);
            }
            WeakReference weakRef = new WeakReference(methodSourceInstance);
            return d(() => weakRef.Target);
        }
        /// <summary>
        /// 使用方法：
        /// Student stu = new Student();
        /// Func<string, int, string> d = WeakDelegateHelper.CreateDelegate(stu, "Say") as Func<string, int, string>;
        /// string resutl1 = d.Invoke("wo", 2);
        /// </summary>
        /// <param name="methodSourceInstance">方法所在的实例</param>
        /// <param name="methodInfo">方法名</param>
        /// <returns>Action or Func</returns>
        public static Delegate CreateDelegate(object methodSourceInstance, string methodName)
        {
            return methodSourceInstance==null?null:CreateDelegate(methodSourceInstance, methodSourceInstance.GetType().GetMethod(methodName));
        }
        #endregion


        private static void EmptyFunc() { }
        private static Expression GetTypeDefaultExpression(Type t)
        {
            if (t == typeof(void)) return Expression.Call(typeof(WeakDelegateHelper).GetMethod("EmptyFunc", BindingFlags.NonPublic | BindingFlags.Static));
            else if (t.IsClass) return Expression.Constant(null, t);
            else return Expression.Constant(t.InvokeMember(null, BindingFlags.CreateInstance, null, null, null));
        }
        private static Func<Func<object>, Delegate> GenerateDelegateImpl(MethodInfo methodInfo)
        {
            ParameterExpression[] parExps = null;
            {
                ParameterInfo[] parInfos = methodInfo.GetParameters();
                parExps = new ParameterExpression[parInfos.Length];
                for (int i = 0; i < parExps.Length; ++i)
                {
                    parExps[i] = Expression.Parameter(parInfos[i].ParameterType, "p" + i);
                }
            }

            ParameterExpression getObjExp = Expression.Parameter(typeof(Func<object>), "getObj");
            Expression getObjIvkExp = Expression.Convert(Expression.Invoke(getObjExp), methodInfo.ReflectedType);

            Expression innerBody =
            Expression.Condition(
            Expression.NotEqual(getObjIvkExp, Expression.Constant(null)),
            Expression.Call(getObjIvkExp, methodInfo, parExps),
            GetTypeDefaultExpression(methodInfo.ReturnType));

            LambdaExpression innerLambda = Expression.Lambda(innerBody, parExps);

            Expression<Func<Func<object>, Delegate>> e = Expression.Lambda<Func<Func<object>, Delegate>>
            (innerLambda, getObjExp);
            return e.Compile();
        }      
    }
}
