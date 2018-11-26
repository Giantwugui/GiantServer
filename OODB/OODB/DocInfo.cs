using System;
using System.Collections.Generic;
using System.Reflection;

namespace OODB
{
    class IndexInfo
    {
        public string Name;
        public Dictionary<string, OODBIndexType> Keys = new Dictionary<string, OODBIndexType>();
        public bool IsUnique = false;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(IndexInfo i1, IndexInfo i2)
        {
            if((Object)i1==null)
            {
                if ((Object)i2 == null)
                    return true;
                else
                    return false;
            }

            //注意我们调用Equals来判断是否相等。而不是在自己的函数中判、断。这是因为如果在自己的函数中判断。比如有rec2=null的情况。如果是这种情况。我们要判断if(rec2==null) {…}。其中rec2==null也是调用一个等号运算符，这里面有一个递归的过程，造成了死循环。  
            return Object.Equals(i1, i2);
        }
        public static bool operator !=(IndexInfo i1, IndexInfo i2)
        {
            if ((Object)i1 == null)
            {
                if ((Object)i2 == null)
                    return false;
                else
                    return true;
            }

            return !Object.Equals(i1, i2);
        }

        public override bool Equals(object obj)
        {
            //判断与之比较的类型是否为null。这样不会造成递归的情况  
            if (obj == null) return false;

            if (GetType() != obj.GetType()) return false;
            IndexInfo idxobj = (IndexInfo)obj;

            if (idxobj.Name != this.Name) return false;

            if (idxobj.IsUnique != this.IsUnique) return false;

            if (idxobj.Keys.Count != this.Keys.Count) return false;

            foreach (KeyValuePair<string, OODBIndexType> curr in idxobj.Keys)
            {
                if (!this.Keys.ContainsKey(curr.Key)) return false;

                if (curr.Value != this.Keys[curr.Key]) return false;
            } 

            return true;
        }
    }

    class DocInfo
    {
        public DocInfo(Type type)
        {
            PropertyInfo[] Propertys = type.GetProperties();
            foreach (PropertyInfo curr in Propertys)
            {
                if (!OODBValueType.IsValueType(curr.PropertyType))
                    throw new Exception(String.Format("[{0}.{1}] 非法，属性[{2}]的类型是不被支持的", type.Name, curr.Name,curr.PropertyType));

                //缺省值读入
                {
                    object[] fdAttributes = curr.GetCustomAttributes(typeof(OOFieldAttribute), true);
                    if (fdAttributes == null || fdAttributes.Length < 1)
                        throw new Exception(String.Format("[{0}.{1}] 未定义字段特性", type.Name, curr.Name));

                    OOFieldAttribute fattr = fdAttributes[0] as OOFieldAttribute;
                    _FieldAttrs.Add(curr.Name, fattr.DefaultValue);

                    
                    //检查缺省值合法性
                    if (fattr.DefaultValue == null && !curr.PropertyType.IsValueType)//缺省值设置为空
                    {

                    }else
                    if( fattr.DefaultValue.GetType() != curr.PropertyType)
                        throw new Exception(String.Format("[{0}.{1}] 缺省值类型不正确", type.Name, curr.Name));
                }

                //索引信息读入
                {
                    object[] fieldIndexAttributes = curr.GetCustomAttributes(typeof(OOFieldIndexAttribute), true);

                    if (fieldIndexAttributes != null && fieldIndexAttributes.Length > 0)
                    {
                      
                        OOFieldIndexAttribute fielIndex = fieldIndexAttributes[0] as OOFieldIndexAttribute;
                        string idxName = String.IsNullOrEmpty(fielIndex.groupName) ? curr.Name : fielIndex.groupName;

                        IndexInfo indexObj;
                        if (!_FieldIndexs.ContainsKey(idxName))
                            _FieldIndexs.Add(idxName, new IndexInfo());

                        indexObj = _FieldIndexs[idxName];
                        indexObj.Name = idxName;
                        indexObj.Keys.Add(curr.Name, fielIndex.IndexType);

                        //如果是组合索引，任意一个设置成唯一，则组合索引为唯一索引
                        if (!indexObj.IsUnique) indexObj.IsUnique = fielIndex.Unique;
                    }
                }
            }
        }

        //字段，索引信息
        internal Dictionary<string, IndexInfo> _FieldIndexs = new Dictionary<string, IndexInfo>();

        //字段，缺省值
        internal Dictionary<string, object> _FieldAttrs = new Dictionary<string, object>();
    }
}
