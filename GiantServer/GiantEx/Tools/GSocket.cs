using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using GiantCore;

namespace GiantCore
{
    public abstract class GSocket
    {
        public GSocket(Socket socket)
        {
            if (socket == null)
            {
                throw new Exception("socket can't be null !");
            }
            else
            {
                mIsSender = false;
                if (socket.SocketType == SocketType.Stream && socket.ProtocolType == ProtocolType.Tcp)
                {
                    mSocket = socket;
                    mPoint = socket.RemoteEndPoint as IPEndPoint;
                }
                else
                {
                    throw new Exception("nonsupport socket type ! curr socket type is :" + socket.SocketType);
                }
            }
        }

        public GSocket(string host, int port)
        {
            mIsSender = true;
            IPAddress[] address = Dns.GetHostAddresses(host);
            if (address.Length > 0 && AddressFamily.InterNetworkV6 == address[0].AddressFamily)
            {
                mSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            mPoint = new IPEndPoint(IPAddress.Parse(host), port);
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        public void ToStart()
        {
            if (mIsSender)
            {
                ToConn();
            }
            else
            {
                ToReceive();
            }
        }

        public void ToSend(GBuffer buffer)
        {
            mSocket.BeginSend(buffer.Content.ToArray(), 0, buffer.SendSize, SocketFlags.None, AsyncSended, buffer);
        }


        private void ToConn()
        {
            mSocket.BeginConnect(mPoint, AsyncConnected, null);
        }


        private void ToReceive()
        {
            SocketError error = SocketError.SocketError;
            try
            {
                mSocket.BeginReceive(mBuffer.TempBuffer, 0, mBuffer.TempBuffer.Length, SocketFlags.None, out error, AsyncReceive, null);
            }
            catch(Exception ex)
            {
                NotifyClosed(error);
                throw ex;
            }
        }

        private void AsyncConnected(IAsyncResult ar)
        {
            try
            {
                mSocket.EndConnect(ar);
                NotifyConnected(true);
                ToReceive();
            }
            catch (Exception ex)
            {
                NotifyConnected(false);
                throw ex;
            }
        }

        private void AsyncReceive(IAsyncResult ar)
        {
            SocketError error = SocketError.SocketError;
            try
            {
                int size = mSocket.EndReceive(ar, out error);
                if (size > 0)
                {
                    mBuffer.Flush(size);
                    List<byte[]> message = mBuffer.PopList();
                    foreach (var curr in message)
                    {
                        NotifyReceived(curr);
                    }
                }

                ToReceive();
            }
            catch (Exception ex)
            {
                NotifyClosed(error);
                throw ex;
            }
        }

        private void AsyncSended(IAsyncResult ar)
        {
            SocketError error = SocketError.Success;
            GBuffer buffer = ar.AsyncState as GBuffer;
            try
            {
                int sendSize = mSocket.EndSend(ar, out error);
                if (sendSize > 0)
                {
                    if (sendSize != mBuffer.SendSize)
                    {
                        ToSend(mBuffer);
                    }
                }
                else
                {
                    NotifyClosed(error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通知连接关闭
        /// </summary>
        protected abstract void NotifyClosed(SocketError error);

        /// <summary>
        /// 通知收到消息
        /// </summary>
        protected abstract void NotifyReceived(byte[] message);

        /// <summary>
        /// 通知连接结果
        /// </summary>
        /// <param name="isConnected"></param>
        protected abstract void NotifyConnected(bool isConnected);

        private Socket mSocket;
        private bool mIsSender = false;
        private IPEndPoint mPoint = null;
        private GBuffer mBuffer = new GBuffer();
    }

}
