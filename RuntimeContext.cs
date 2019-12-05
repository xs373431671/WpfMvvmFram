using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
using WpfMvvmFram.RuntimeContextResource;

namespace WpfMvvmFram
{
    public class RuntimeContext : IContext
    {
        private IUnityContainer m_UnityContainer;
        private HandlerCollection m_HandlerCollection;
        private Dictionary<string, IContext> m_Children;
        public string Name { get; set; }
        public RuntimeContext()
        {
            m_UnityContainer = new UnityContainer();
            this.ResisterInstance<IUnityContainer>(m_UnityContainer);

            m_HandlerCollection = new HandlerCollection();
            m_Children = new Dictionary<string, IContext>();
        }
        public RuntimeContext(string childContextName, IContext parentContext)
        {
            m_UnityContainer = parentContext.Get<IUnityContainer>().CreateChildContainer();
            m_HandlerCollection = new HandlerCollection();
            m_Children = new Dictionary<string, IContext>();
            parentContext.AddChildContext(childContextName, parentContext);
        }
        public IContext ParentContext
        {
            get;
            private set;
        }

        public void AddChildContext(string key, IContext childContext)
        {
            if (!m_Children.ContainsKey(key))
            {
                m_Children.Add(key, childContext);
                return;
            }
            else if (m_Children.ContainsKey(key) && m_Children[key] != null)
            {
                throw new Exception("the key is Exist");
            }
            else
            {
                m_Children[key] = childContext;
                return;
            }
        }

        public IContext this[string key]
        {
            get
            {
                if (!m_Children.ContainsKey(key))
                {
                    return null;
                }
                return m_Children[key];
            }
        }

        public void RegisterType<TClass>(params object[] parameters)
        {
            m_UnityContainer.RegisterType<TClass>(new InjectionConstructor(parameters));
        }
        public void RegisterSingle<TClass>(params object[] parameters)
        {

            m_UnityContainer.RegisterSingleton<TClass>(new InjectionConstructor(parameters));
        }


        public void RegisterType<TInterface, TClass>() where TClass : TInterface
        {
            m_UnityContainer.RegisterType<TInterface, TClass>();
        }

        public void RegisterType<TInterface, TClass>(bool isSingleton) where TClass : TInterface
        {
            if (isSingleton)
            {
                m_UnityContainer.RegisterSingleton<TInterface, TClass>();
            }
            else
            {
                m_UnityContainer.RegisterType<TInterface, TClass>();
            }
        }

        public void RegisterType<TInterface, TClass>(params object[] parameters) where TClass : TInterface
        {
            InjectionConstructor injectParameters = new InjectionConstructor(parameters);
            m_UnityContainer.RegisterType<TInterface, TClass>(injectParameters);

        }

        public void RegisterType<TInterface, TClass>(bool isSingleton, params object[] parameters) where TClass : TInterface
        {
            InjectionConstructor injectParameters = new InjectionConstructor(parameters);
            if (isSingleton)
            {
                m_UnityContainer.RegisterSingleton<TInterface, TClass>(injectParameters);
            }
            else
            {
                m_UnityContainer.RegisterType<TInterface, TClass>(injectParameters);
            }
        }

        public void RegisterType<TInterface, TClass>(string name) where TClass : TInterface
        {
            m_UnityContainer.RegisterType<TInterface, TClass>(name);
        }

        public void RegisterType<TInterface, TClass>(string name, params object[] parameters) where TClass : TInterface
        {
            m_UnityContainer.RegisterType<TInterface, TClass>(name, new InjectionConstructor(parameters));
        }

        public void RegisterType<TInterface, TClass>(bool isSingleton, string name, params object[] parameters) where TClass : TInterface
        {
            InjectionConstructor injectParameters = new InjectionConstructor(parameters);
            if (isSingleton)
            {
                m_UnityContainer.RegisterSingleton<TInterface, TClass>(name, injectParameters);
            }
            else
            {
                m_UnityContainer.RegisterType<TInterface, TClass>(name, injectParameters);
            }
        }

        public void ResisterInstance<TInterface>(TInterface instance)
        {
            m_UnityContainer.RegisterInstance<TInterface>(instance);
        }

        public bool IsRegistered<TInterface>()
        {
            return m_UnityContainer.IsRegistered<TInterface>();
        }

        public T Get<T>()
        {
            return m_UnityContainer.Resolve<T>();
        }

        public T Get<T>(string name)
        {
            return m_UnityContainer.Resolve<T>(name);
        }

        public void RegisterHandler(Delegate handler)
        {
            m_HandlerCollection.Add(handler.GetType(), handler);
        }

        public List<Delegate> GetHandler(Type handlerType)
        {
            if (handlerType != null && !m_HandlerCollection.IsExist(handlerType))
            {
                if (ParentContext != null)
                {
                    return ParentContext.GetHandler(handlerType);
                }
            }
            return m_HandlerCollection[handlerType];
        }

        public void UnregisterHandler(Delegate handler)
        {
            if (handler != null)
            {
                m_HandlerCollection.Remove(handler.GetType(), handler);
            }
        }

        public void UnregisterAllHandler(Type handlerType)
        {
            if (handlerType != null && m_HandlerCollection.IsExist(handlerType))
            {
                m_HandlerCollection[handlerType].Clear();
            }
        }

        public List<object> PublishHandler(Type handlerType, params object[] parameters)
        {
            List<object> returnValues = new List<object>();
            List<Delegate> handlers = GetHandler(handlerType);//获取handlerType对应的handler
            if (handlers != null && handlers.Count > 0)
            {
                foreach (Delegate handler in handlers)
                {
                    returnValues.Add(System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(handler, parameters));
                }
            }
            return returnValues;
        }


    }
}
