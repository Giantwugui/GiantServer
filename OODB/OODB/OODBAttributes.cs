using System;

namespace OODB
{

    public enum OODBIndexType
    {
        Asc,//升序
        Desc,//降序
        //Geospatial,//2维地理位置，数组头两个变量为位置信息，xy顺序无关，仅支持 var x={x="",x=1,xxx}
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OOFieldAttribute : Attribute
    {
        public OOFieldAttribute(Object defaultValue) { m_defaultValue = defaultValue; }
        public Object DefaultValue { get { return m_defaultValue; } }
        Object m_defaultValue;
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OOFieldIndexAttribute : Attribute
    {
        public OOFieldIndexAttribute(OODBIndexType indexType) { m_indexType = indexType; }
        public OODBIndexType IndexType { get { return m_indexType; } }
        public string GroupName = "";//组名，对于多字段索引，应设置一样的组名，组名即多维索引名
        public bool Unique = false;//唯一索引
        OODBIndexType m_indexType;
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OOTableAttribute : Attribute
    {
        public OOTableAttribute(string dbNickName)
        {
            m_dbNickName = dbNickName;
        }

        public String DBNickName { get { return m_dbNickName; } }
        String m_dbNickName;
    }


   

}
