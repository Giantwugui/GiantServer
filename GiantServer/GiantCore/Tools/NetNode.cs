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
        }

        protected override void NotifyConnected(bool isConnected)
        {
        }

        protected override void NotifyReceived(byte[] message)
        {
        }
    }
}
