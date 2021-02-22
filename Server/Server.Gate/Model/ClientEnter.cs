using Giant.DB;
using Giant.Util;
using System;
using System.Collections.Generic;

namespace Server.Gate
{
    public class ClientEnter
    {
        public string Account { get; private set; }
        public int Token { get; private set; }
        public DateTime EntryTime { get; private set; }

        private Dictionary<int, PlayerInfo> characterList;
        public Dictionary<int, PlayerInfo> CharacterList => characterList;

        public ClientEnter(string account, int token)
        {
            Token = token;
            Account = account;
            EntryTime = TimeHelper.Now;
            characterList = new Dictionary<int, PlayerInfo>();
        }

        public void AddCharacter(PlayerInfo player)
        {
            characterList[player.Uid] = player;
        }
    }
}
