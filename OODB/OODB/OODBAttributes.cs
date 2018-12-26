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
        Object mDefaultValue;

        public OOFieldAttribute(Object defaultValue) { mDefaultValue = defaultValue; }

        public Object DefaultValue { get { return mDefaultValue; } }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OOFieldIndexAttribute : Attribute
    {
        private bool mUnique = false;
        private string mGroupName = "";
        private OODBIndexType mIndexType = OODBIndexType.Asc;

        public OOFieldIndexAttribute(OODBIndexType indexType) { mIndexType = indexType; }

        public OODBIndexType IndexType { get { return mIndexType; } }

        /// <summary>
        /// 唯一索引
        /// </summary>
        public bool Unique { get { return mUnique; } set { mUnique = value; } }

        /// <summary>
        /// 组名，对于多字段索引，应设置一样的组名，组名即多维索引名
        /// </summary>
        public string GroupName { get { return mGroupName; } set { mGroupName = value; } }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OOTableAttribute : Attribute
    {
        String mDBNickName;

        public OOTableAttribute(string dbNickName)
        {
            mDBNickName = dbNickName;
        }

        public String DBNickName { get { return mDBNickName; } }
    }


   

}
