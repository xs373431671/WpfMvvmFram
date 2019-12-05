using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram.RuntimeContextResource
{
    public class HandlerCollection
    {
        private Dictionary<Type, List<Delegate>> m_HandlerCollection;

        public HandlerCollection()
        {
            m_HandlerCollection = new Dictionary<Type, List<Delegate>>();
        }

        public List<Delegate> this[Type handlerType]
        {
            get
            {
                if (handlerType != null && m_HandlerCollection.ContainsKey(handlerType))
                {
                    return m_HandlerCollection[handlerType];
                }
                return null;
            }
        }

        public void Add(Type handlerType, Delegate handler)
        {
            if (handlerType == null || handler == null)
            {
                throw new Exception("handlerType or handler is null");
            }
            if (m_HandlerCollection.ContainsKey(handlerType))
            {
                if (m_HandlerCollection[handlerType] == null)
                {
                    m_HandlerCollection[handlerType] = new List<Delegate>();
                }
                else if (m_HandlerCollection[handlerType] != null && !m_HandlerCollection[handlerType].Contains(handler))
                {
                    m_HandlerCollection[handlerType].Add(handler);
                }
            }
            else
            {
                m_HandlerCollection.Add(handlerType, new List<Delegate>() { handler });
            }
        }

        public void Remove(Type handlerType, Delegate handler)
        {
            if (handlerType == null || handler == null)
            {
                throw new Exception("handlerType or handler is null");
            }
            if (m_HandlerCollection.ContainsKey(handlerType) && m_HandlerCollection[handlerType] != null && m_HandlerCollection[handlerType].Contains(handler))
            {
                m_HandlerCollection[handlerType].Remove(handler);
            }
        }

        public bool IsExist(Type handlerType)
        {
            return m_HandlerCollection.ContainsKey(handlerType) ? true : false;
        }
    }
}
