using Giant.Core;
using Giant.Net;
using Giant.Util;
using System;

namespace Server.Account
{
    public class User : Entity, IInitSystem<Session, string>
    {
        public Session Session { get; private set; }
        public string Account { get; private set; }

        public DateTime HeartBeatTime = TimeHelper.Now;

        public void Init(Session session, string account)
        {
            Session = session;
            Account = account;
        }

        public void Offline()
        {
            UserManagerComponent.Instance.Remove(Session.Id);
        }

        public void HeartBeat()
        {
            HeartBeatTime = TimeHelper.Now;
        }
    }
}
