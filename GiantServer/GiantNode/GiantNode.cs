using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GiantCore;
using System.Net;
using System.Threading;

namespace GiantNode
{
    public partial class GiantServer : Form
    {
        public GiantServer()
        {
            mSingle = this;
            InitializeComponent();

            LogOut(LogType.Debug, "开始启动服务器");

            //初始化服务器各个模块
            Init(30199);
        }

        /// <summary>
        /// 初始化通讯
        /// </summary>
        void Init(int port)
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            listener.Start(5000);

            while (true)
            {
                //监听端口，并创建连接对象

                NetNode tempSocket = new NetNode(listener.AcceptSocket());
                tempSocket.OnReceiveMessage += OnReceiveMessage;
                tempSocket.ToStart();

                m_allListener.Add(tempSocket);
                Thread.Sleep(2);
            }

            //初始化插件事件
            PluginMain.InitPlugins();

            //启动完成事件
            PluginMain.OnStartComplate();

            LogOut(LogType.Debug, "服务器启动完成!");
        }


        private void OnReceiveMessage(byte[] message)
        { 
        }


        /// <summary>
        /// 退出
        /// </summary>
        private void GiantServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        public void LogOut(LogType type, string message)
        {
            listBox_log.Items.Add(message);

            //后续实现不同的日志级别显示
            
            //switch (type)
            //{
            //    case LogType.Debug:
            //        {
            //            int index = mListBox.Items.Add(message);
            //        }
            //        break;
            //    case LogType.Warning:
            //        {
            //            mListBox.Items.Add(message);
            //        }
            //        break;
            //    case LogType.Error:
            //        {
            //            mListBox.Items.Add(message);
            //        }
            //        break;
            //    case LogType.Crash:
            //        {
            //            mListBox.Items.Add(message);
            //        }
            //        break;
            //}
        }

        #region 属性

        public static GiantServer Single
         {
             get { return mSingle; }
         }

         static GiantServer mSingle = null;

         static List<NetNode> m_allListener = new List<NetNode>();

        #endregion

    }
}
