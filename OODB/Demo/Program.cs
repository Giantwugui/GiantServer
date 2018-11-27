using System;
using OODB;
using System.Collections;
using System.Threading;
using System.Configuration;
using MongoDB.Driver.Builders;

namespace demo
{
    class Pos : OODoc
    { 
        [OOField(1.5f)]
        public float X
        {
            get { return GetValue<float>(); }
            set { SetValue( value); }
        }

        [OOField(2.0f)]
        public float Y
        {
            get { return GetValue<float>(); }
            set { SetValue( value); }
        } 
    }

    class Hero : OODoc
    { 
        [OOField("")]
        public String OwnerPlayerID
        {
            get { return GetValue<String>(); }
            set { SetValue( value); }
        } 
    }

    enum SEX
    {
        Man =1,
        Woman =2
    }

    [OOTable("1111")]
    class Player : OOTab
    {
        public Player(OOConn conn) : base(conn) { }

        [OOField("")]
        [OOFieldIndex(OODBIndexType.Asc,Unique = true,GroupName="Key1_key2")]
        public String Key1
        {
            get
            {
                return GetValue<String>();
            }
            set { SetValue(value); }
        }

        [OOField("")]
        [OOFieldIndex(OODBIndexType.Asc, Unique = true, GroupName = "Key1_key2")]
        public String Key2
        {
            get
            {
                return GetValue<String>();
            }
            set { SetValue(value); }
        }


        [OOField(88.0f)]
        public float Exp
        {
            get {  return GetValue<float>();  }
            set { SetValue(value); }
        }
      
        [OOField(SEX.Man)]
        public SEX Sex
        {
            get {
                  return GetValue<SEX>();
            }
            set { SetValue(value); }
        }

        [OOField("")]
        public String Name
        {
            get
            {
                return GetValue<String>();
            }
            set { SetValue(value); }
        }

        [OOField(null)]
        public byte[] Bytes
        {
            get
            {
                return GetValue<byte[]>();
            }
            set { SetValue(value); }
        }

        public readonly Pos mapPos = new Pos();
        public readonly OOList<float> olist = new OOList<float>();
        public readonly OODictionary<int, Hero> heroList = new OODictionary<int, Hero>();
    };
     
    class Program
    {

        static void Main(string[] args)
        {
            Type stringType = typeof(String);
            if (!(new OODBInstance()).Init(System.Reflection.Assembly.GetExecutingAssembly()))
            {
                return;
            }

            using (OOConn conn = new OOConn( 
                ConfigurationManager.AppSettings["IP"], 
                int.Parse(ConfigurationManager.AppSettings["Port"]),
                ConfigurationManager.AppSettings["DBName"],
                ConfigurationManager.AppSettings["UserName"],
                ConfigurationManager.AppSettings["UserPass"]))
            {

                OODBInstance.Single.UpdateIndex("1111", conn);

                const string playerName = "wen";

                if (null == OOTab.FindFirst<Player>(conn, Query.EQ("Name", playerName)))
                {
                    Player playerDB = new Player(conn)
                    {
                        Name = playerName,
                        Key1 = "key1",
                        Key2 = "key2",
                    };

                    for (int i = 0; i < 5; i++)
                        playerDB.olist.Add(i);
                    playerDB.Sex = SEX.Woman;
                    playerDB.mapPos.X = 11;
                    playerDB.mapPos.Y = 76;


                    var tmp = new byte[10];
                    tmp[0] = 1;
                    tmp[1] = 2;
                    tmp[2] = 3;
                    tmp[3] = 5;
                    playerDB.Bytes = tmp;

                    {
                        Hero hr = new Hero();
                        hr.OwnerPlayerID = "001";

                        playerDB.heroList.Add(100, hr);
                    }

                    {
                        Hero hr = new Hero();
                        hr.OwnerPlayerID = "002";

                        playerDB.heroList.Add(101, hr);

                    }
                    playerDB.Save();
                }

                while (true)
                {
                    IEnumerator it = OOTab.Find<Player>(conn, Query.EQ("Name", playerName));
                    while (it.MoveNext())
                    {
                        Player playerObj = it.Current as Player;
                        var bbb = playerObj.Bytes;

                        if (playerObj.heroList.ContainsKey(100))
                            playerObj.heroList.Remove(100);

                        Console.WriteLine(string.Format("name:{0} key1:{1} key2:{2}", playerObj.Name, playerObj.Key1, playerObj.Key2));

                        playerObj.Save();
                    }
                    Thread.Sleep(0);
                }
            }
        }

       
    }
}
