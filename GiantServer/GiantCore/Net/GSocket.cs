using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GiantCore
{
    public abstract class GSocket
    {
        public GSocket(Socket socket)
        {
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

        public GSocket(string host, int port)
        {
            mIsSender = true;

            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mPoint = new IPEndPoint(IPAddress.Parse(host), port);
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        public virtual void ToStart()
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
            }
            catch (Exception ex)
            {
                NotifyClosed(error);

                throw ex;
            }

            if (error == SocketError.Success)
            {
                ToReceive();
            }
            else
            {
                NotifyClosed(error);
            }
        }

        private void AsyncSended(IAsyncResult ar)
        {
            try
            {
                SocketError error = SocketError.Success;

                GBuffer buffer = ar.AsyncState as GBuffer;

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
        protected virtual void NotifyConnected(bool isConnected) { }

        private Socket mSocket;

        private bool mIsSender = false;

        private IPEndPoint mPoint = null;

        private GBuffer mBuffer = new GBuffer();
    }

}
