using System;

namespace OODB
{
    /// <summary>
    /// Collection信息
    /// </summary>
    class TabInfo 
    {  
        public TabInfo(Type type, string dbNickName) 
        {
            mName = type.Name;
            mDBNickName = dbNickName; 
        }

        readonly string mName;
        public string Name { get { return mName; } }

        readonly string mDBNickName;
        public string DBNickName { get { return mDBNickName; } }
    }



}
