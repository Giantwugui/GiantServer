using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using MonoEX;

namespace MyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient client =new SocketClient();
            client.ConnectServer();
            System.Console.ReadLine();
        }
    }
        public partial class SocketClient
        {
            private Socket m_clientSocket;
            private byte[] m_receiveBuffer = new byte[1024];

            /// <summary>
            /// 连接服务器
            /// </summary>
            public void ConnectServer()
            {
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2333);

                try
                {
                    m_clientSocket.Connect(remoteEndPoint);
                    if (m_clientSocket.Connected)
                    {
                        btnSend_Click();
                        m_clientSocket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);

                    }
                }
                catch (Exception)
                {
                    m_clientSocket = null;
                }
            }

            /// <summary>
            /// 发送信息
            /// </summary>
            private void btnSend_Click()
            {
                QK_JsonValue_Map tempMap = new QK_JsonValue_Map();
                tempMap.addStrValue("n", "Docmd");
                tempMap.addStrValue("param1", "wg");
                tempMap.addStrValue("param2", "test");

                byte[] sendBuffer = Encoding.Unicode.GetBytes(tempMap.ToString());
                if (m_clientSocket != null)
                {
                    m_clientSocket.Send(sendBuffer);
                }
            }

            private void ReceiveCallBack(IAsyncResult ar)
            {
                try
                {
                    int REnd = m_clientSocket.EndReceive(ar);
                    string strReceiveData = Encoding.Unicode.GetString(m_receiveBuffer, 0, REnd);
                    this.HandleMessage(strReceiveData);
                    m_clientSocket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            /// <summary>
            /// 处理接收到的数据
            /// </summary>
            private void HandleMessage(string message)
            {
                message = message.Replace("/0", "");
                if (!string.IsNullOrEmpty(message))
                {
                    System.Console.WriteLine(message);
                }
            }

        }
    

}
