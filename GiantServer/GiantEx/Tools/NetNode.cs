using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace GiantCore
{
    public class NetNode : GSocket
    {
        public NetNode(Socket socket)
            : base(socket)
        {          
        }

        public void Send(byte[] message)
        {
            base.ToSend(new GBuffer(message));
        }

        public void ToStart()
        {
            base.ToStart();
        }

        protected override void NotifyClosed(SocketError error)
        {
            if (OnClosed != null)
            {
                OnClosed();
            }
        }

        protected override void NotifyConnected(bool isConnected)
        {
            if (OnConnected != null)
            {
                OnConnected(isConnected);
            }
        }

        protected override void NotifyReceived(byte[] message)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(message);
            }

            OnReceiveMessage ?.(message);
        }

        public GEvent.OnClosed OnClosed = null;
        public GEvent.OnConnected OnConnected = null;
        public GEvent.OnReceiveMessage OnReceiveMessage = null;
    }
}
