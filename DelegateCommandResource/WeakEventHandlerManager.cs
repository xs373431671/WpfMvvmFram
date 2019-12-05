using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmFram.DelegateCommandResource
{
    internal static class WeakEventHandlerManager
    {
        public static void CallWeakReferenceHandlers(object sender,List<WeakReference>handlers)
        {
            if(handlers!=null)
            {
                EventHandler[] callees = new EventHandler[handlers.Count];
                int count = 0;
                foreach(var item in handlers)
                {
                    EventHandler handler = item.Target as EventHandler;
                    if(handler!=null)
                    {
                        callees[count] = handler;
                        count++;
                    }
                }
                foreach(var item in callees)
                {
                  
                }
            }
        }


        private static void CallHandler(object sender,EventHandler eventHandler)
        {
           
        }

    }
}
