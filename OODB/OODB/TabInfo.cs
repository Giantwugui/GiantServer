using System;

namespace OODB
{
    /// <summary>
    /// 描述一个Collections
    /// </summary>
    class TabInfo 
    {  
        public TabInfo(Type type, string dbNickName) 
        {
            m_Name = type.Name;
            m_dbNickName = dbNickName; 
        }

        public string Name { get { return m_Name; } }
        public string dbNickName { get { return m_dbNickName; } } 
        string m_Name;
        string m_dbNickName;
    }



}
