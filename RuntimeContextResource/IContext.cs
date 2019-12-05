using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram.RuntimeContextResource
{
    public interface IContext
    {
        string Name { get; set; }
        /// <summary>
        /// 根据主键获取对应的ChildrenContext
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IContext this[string key] { get; }

        /// <summary>
        /// 增加子上下分
        /// </summary>
        /// <param name="childContext"></param>
        void AddChildContext(string key, IContext childContext);
        /// <summary>
        /// 父Context
        /// </summary>
        IContext ParentContext { get; }





        void RegisterType<TClass>(params object[] parameters);

        /// <summary>
        /// 依赖注入支注册
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        void RegisterType<TInterface, TClass>() where TClass : TInterface;




        /// <summary>
        /// 依赖注入支注册(是否为单例模式）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="isSingleton">是否为单例模式</param>
        void RegisterType<TInterface, TClass>(bool isSingleton) where TClass : TInterface;


        /// <summary>
        /// 依赖注入支注册（带参数）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="parameters">要传入的参数</param>
        void RegisterType<TInterface, TClass>(params object[] parameters) where TClass : TInterface;



        /// <summary>
        /// 依赖注入支注册(是否为单例模式,带参数）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="isSingleton">是否为单例模式</param>
        /// <param name="parameters">要传入的参数</param>
        void RegisterType<TInterface, TClass>(bool isSingleton, params object[] parameters) where TClass : TInterface;

        /// <summary>
        /// 依赖注入支注册（带标识名称）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="name">注册接口的标识名称</param>
        void RegisterType<TInterface, TClass>(string name) where TClass : TInterface;



        /// <summary>
        /// 依赖注入支注册（带标识名称,带参数）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="name">注册接口的标识名称</param>
        /// <param name="parameters">传入的参数</param>
        void RegisterType<TInterface, TClass>(string name, params object[] parameters) where TClass : TInterface;


        /// <summary>
        /// 依赖注入支注册（是否为单例，带标识名称,带参数）
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <param name="isSingleton">是否为单例模式</param>
        /// <param name="name">注册接口的标识名称</param>
        /// <param name="parameters">传入的参数</param>
        void RegisterType<TInterface, TClass>(bool isSingleton, string name, params object[] parameters) where TClass : TInterface;

        void RegisterSingle<TClass>(params object[] parameters);
        /// <summary>
        /// 注册实例对象
        /// </summary>
        /// <typeparam name="TInterface">实例对象类型或接口</typeparam>
        /// <param name="instance">实例对象</param>
        void ResisterInstance<TInterface>(TInterface instance);


        /// <summary>
        /// 是否注册了某个类型
        /// </summary>
        /// <typeparam name="TInterface">类型或接口</typeparam>
        /// <returns></returns>
        bool IsRegistered<TInterface>();


        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns></returns>
        T Get<T>();


        /// <summary>
        /// 获取指定类型与名称的实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="name">注册时的标识名</param>
        /// <returns></returns>
        T Get<T>(string name);


        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="handler">委托</param>
        void RegisterHandler(Delegate handler);

        /// <summary>
        /// 获取注册的委托
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        List<Delegate> GetHandler(Type handlerType);

        /// <summary>
        /// 移除已注册的委托
        /// </summary>
        /// <param name="handler"></param>
        void UnregisterHandler(Delegate handler);

        /// <summary>
        /// 移除某委托的所有示例
        /// </summary>
        /// <param name="handlerType"></param>
        void UnregisterAllHandler(Type handlerType);

        /// <summary>
        /// 发布委托（激发委托）
        /// </summary>
        /// <param name="handlerTYpe">委托类型</param>
        /// <param name="parameters">委托参数</param>
        /// <returns>委托的返回值(因为一个handlerType可能对应多个handler，所以返回值为List<Object></returns>
        List<object> PublishHandler(Type handlerType, params object[] parameters);
    }
}
