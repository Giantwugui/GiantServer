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
        public OOFieldAttribute(Object defaultValue) { mDefaultValue = defaultValue; }

        Object mDefaultValue;
        public Object DefaultValue { get { return mDefaultValue; } }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OOFieldIndexAttribute : Attribute
    {
        public OOFieldIndexAttribute(OODBIndexType indexType) { mIndexType = indexType; }

        OODBIndexType mIndexType;
        public OODBIndexType IndexType { get { return mIndexType; } }

        public bool Unique = false;//唯一索引

        public string GroupName = "";//组名，对于多字段索引，应设置一样的组名，组名即多维索引名
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OOTableAttribute : Attribute
    {
        public OOTableAttribute(string dbNickName)
        {
            mDBNickName = dbNickName;
        }

        String mDBNickName;
        public String DBNickName { get { return mDBNickName; } }
    }


   

}
