﻿using System;
using System.Collections.Generic;

namespace Giant.Net
{
    public class Session : IDisposable
    {
        private BaseProtocol baseProtocol;//通讯对象

        private Dictionary<ushort, Action<IMessage>> responseCallback = new Dictionary<ushort, Action<IMessage>>();//消息回调

        public NetworkService NetworkService { get; private set; }

        public uint Id { get; private set; }

        public Session(NetworkService networkService, BaseProtocol baseSession)
        {
            Id = baseSession.Id;

            NetworkService = networkService;
            this.baseProtocol = baseSession;

            this.baseProtocol.OnRead += OnRead;
            this.baseProtocol.OnError += OnError;
        }


        public void Call(IMessage message, Action<IMessage> callback)
        {
            BindResponse(message, callback);

            Transfer(message.MsgContent);
        }

        public void Transfer(byte[] message)
        {
            baseProtocol.Transfer(message);
        }


        public void Dispose()
        {
            baseProtocol.Dispose();

            //清空所有消息回调
            responseCallback.Clear();
        }

        /// <summary>
        /// 绑定回调
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        private void BindResponse(IMessage message, Action<IMessage> callback)
        {
            if (responseCallback.TryGetValue(message.Id, out Action<IMessage> action))
            {
                responseCallback.Remove(message.Id);
            }

            responseCallback.Add(message.Id, callback);
        }

        private void OnRead(byte[] message)
        {
            //消息id
            ushort messageId = BitConverter.ToUInt16(message);

            //回调类消息
            if (IsCallbackMessage(messageId))
            {
                if (responseCallback.TryGetValue(messageId, out var action))
                {
                    //action();
                    responseCallback.Remove(messageId);
                }
            }
            else //其他类型消息
            {
            }
        }

        private void OnError(object error)
        {
            switch (error)
            {
                case Exception ex:
                    {
                    }
                    break;
                case int errorCode:
                    {
                    }
                    break;
            }

            NetworkService.Remove(this.Id);
        }


        private static bool IsCallbackMessage(ushort messageId)
        {
            return messageId < 10000;
        }

    }
}