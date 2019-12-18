﻿using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.App
{
    /// <summary>
    /// 消息处理的例子 由于使用组件方式开发，不在通讯层与实体层之间引入系统system层，以免过度设计
    /// </summary>
    [MessageHandler]
    class Handle_GateZ_Hero : MHandler<Msg_GateZ_Hero_Break, Msg_ZGate_Hero_Break>
    {
        public override Task Run(Session session, Msg_GateZ_Hero_Break request, Msg_ZGate_Hero_Break response)
        {
            Player player = PlayerManagerComponent.Instance.GetPlayer(request.Uid);
            //HeroSystem system = Scene.EventSystem.GetSystem<HeroSystem>();
            //system?.HeroBreak(player, request.HeroId);

            Hero hero = player?.GetComponent<HeroManagerComponent>().GetHero(request.HeroId);
            if (hero == null)
            {
                response.Error = ErrorCode.Fail;
                return Task.CompletedTask;
            }


            return Task.CompletedTask;
        }
    }
}
