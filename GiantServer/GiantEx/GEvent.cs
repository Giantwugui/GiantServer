using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace GiantCore
{
    public class GEvent
    {
        public GEvent()
        { 
        }

        public void Call_OnHandle(Session session, byte[] message)
        {
            if (message.Length > 0)
            {
                onHandle.Invoke(session, message);
            }
        }

        public void Call_Init()
        {
            if (onInit != null)
            {
                onInit.Invoke();
            }
        }

        public void Call_OnUpdate(float time)
        {
            if (onUpdate != null)
            {
                onUpdate.Invoke(time);
            }
        }

        public void Call_OnStartComplate()
        {
            if (onStartComplate != null)
            {
                onStartComplate.Invoke();
            }
        }

        //事件
        public event GEvent.OnInit onInit;
        public event GEvent.OnClosed onClose;
        public event GEvent.OnUpdate onUpdate;
        public event GEvent.OnHandle onHandle;
        public event GEvent.OnConnected onConnected;
        public event GEvent.OnReceiveMessage onReceiveMessage;
        public event GEvent.OnStartComplate onStartComplate;


        //委托
        public delegate void OnInit();
        public delegate void OnClosed();
        public delegate void OnStartComplate();


        public delegate void OnUpdate(float time);
        public delegate void OnConnected(bool success);
        public delegate void OnReceiveMessage(byte[] message);
        public delegate void OnHandle(Session session, byte[] message);
    }
}
