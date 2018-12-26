using System;
using System.Collections.Generic;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

using System.Reflection;
namespace OODB
{
    public class OODBInstance
    {
        public OODBInstance()
        {
            Single = this;
        }

        /// <summary>
        /// 初始化OODB
        /// </summary>
        public bool Init(System.Reflection.Assembly assembly)
        {
            try
            {
                //扫描
                foreach (Type type in assembly.GetTypes())
                { 
                    object[] attributes = type.GetCustomAttributes(typeof(OOTableAttribute), true);

                    for (int i = 0; i < attributes.Length; i++)
                    {
                        if (!(attributes[i] is OOTableAttribute att)) continue;

                        CheckDocClass(type, typeof(OOTab), null);

                        TabInfo cInfo = new TabInfo(type, att.DBNickName);

                        if (!mTabInfos.ContainsKey(att.DBNickName))
                            mTabInfos.Add(att.DBNickName, new Dictionary<string, TabInfo>());

                        mTabInfos[cInfo.DBNickName].Add(cInfo.Name, cInfo);
                        m_TabInfosIndexByName.Add(cInfo.Name, cInfo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                mLastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 自动增量构建索引，程序启动时由主数据库管理节点调用一次即可
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dbNickName"></param>
        public void UpdateIndex(string dbNickName, OOConn conn)
        {
            if (!mTabInfos.ContainsKey(dbNickName))
                throw new Exception(String.Format("UpdateIndex 未知的数据库别名 {0}", dbNickName));

            Dictionary<string, TabInfo> tabs = mTabInfos[dbNickName];
            foreach (KeyValuePair<string, TabInfo> kv in tabs)
            {
                TabInfo curr = kv.Value;

                MongoCollection mongoCollection = conn.Database.GetCollection(curr.Name);
                //读出全部索引
                Dictionary<string, IndexInfo> indexs = new Dictionary<string, IndexInfo>();
                {
                    GetIndexesResult indexResult = mongoCollection.GetIndexes();
                    foreach (MongoDB.Driver.IndexInfo currIndex in indexResult)
                    {
                        if (currIndex.Name == "_id_") continue;

                        IndexInfo nIdx = new IndexInfo
                        {
                            Name = currIndex.Name,
                            IsUnique = currIndex.IsUnique
                        };

                        for (int i = 0; i < currIndex.Key.ElementCount; i++)
                        {
                            BsonElement el = currIndex.Key.GetElement(i);
                            OODBIndexType itype = (int)el.Value >= 1 ? OODBIndexType.Asc : OODBIndexType.Desc;
                            nIdx.Keys.Add(el.Name, itype);
                        }

                        indexs.Add(nIdx.Name, nIdx);
                    }
                }

                //需要删除的索引
                HashSet<string> needDelIndexs = new HashSet<string>();

                //需要修改的索引
                HashSet<string> needEditIndexs = new HashSet<string>();

                //需要新建的索引
                HashSet<string> needCreateIndexs = new HashSet<string>();

                //分析索引
                DocInfo docInfo = mDocInfos[curr.Name];
                foreach (KeyValuePair<string, IndexInfo> indexKV in docInfo.mFieldIndexs)
                {
                    string indexName = indexKV.Key;
                    IndexInfo indexAttr = indexKV.Value;
                    if (indexs.ContainsKey(indexName))
                    {
                        //比对索引
                        if(indexs[indexName]!= indexAttr)
                            needEditIndexs.Add(indexName);
                    }
                    else //这是一个需要新建的索引 
                        needCreateIndexs.Add(indexName); 
                }

                foreach (KeyValuePair<string, IndexInfo> indexKV in indexs)
                {
                    string indexName = indexKV.Key;
                    if (!docInfo.mFieldIndexs.ContainsKey(indexName))
                        needDelIndexs.Add(indexName);//这是一个需要删除的索引 
                }

                //将处理结果引用到数据库
                {
                    //删除
                    foreach (string key in needDelIndexs)
                        mongoCollection.DropIndexByName(key);


                    //修改
                    foreach (string key in needEditIndexs)
                    {
                        mongoCollection.DropIndexByName(key);//删除索引
                        needCreateIndexs.Add(key);//添加到需要新建的key队列
                    }

                    //新建
                    foreach (string key in needCreateIndexs)
                    {
                        IndexInfo info = docInfo.mFieldIndexs[key];

                        //生成需要索引的字段信息
                        IndexKeysBuilder keysBuilder = new IndexKeysBuilder();
                        {

                            foreach (KeyValuePair<string, OODBIndexType> indexKey in info.Keys)
                            {
                                if (indexKey.Value == OODBIndexType.Asc)
                                    keysBuilder.Ascending(indexKey.Key);
                                else
                                    keysBuilder.Descending(indexKey.Key);
                            }
                        }

                        //创建索引
                        mongoCollection.CreateIndex(
                            keysBuilder,
                            new IndexOptionsBuilder().SetName(info.Name).SetUnique(info.IsUnique)
                            );
                    }
                }
            }
        }

        void CheckDocClass(Type type, Type baseType, Type excludedBaseType)
        {
            if (mDocInfos.ContainsKey(type.Name))
                return;//检查过了，跳过

            mDocInfos.Add(type.Name, new DocInfo(type));

            if (!baseType.IsAssignableFrom(type))
                throw new Exception(String.Format("[{0}] 非法，必须从 {1} 继承", type.Name, baseType.Name));

            if (excludedBaseType != null && excludedBaseType.IsAssignableFrom(type))
                throw new Exception(String.Format("[{0}] 非法，不能从 {1} 继承", type.Name, excludedBaseType.Name));



            FieldInfo[] fieldInfos = type.GetFields();
            foreach (FieldInfo curr in fieldInfos)
            {
                if (!OODBValueType.IsValueGroupType(curr.FieldType))
                    throw new Exception(String.Format("[{0}.{1}] 非法，此处该类型是不被支持的", type.Name, curr.Name));

                if (curr.FieldType.IsGenericType) //泛型
                {
                    Type[] TTypes = curr.FieldType.GetGenericArguments();
                    if (curr.FieldType.Name == typeof(OOList<>).Name)
                    {
                        Type _TValueType = TTypes[0];

                        if (!OODBValueType.IsValueType(_TValueType))
                            throw new Exception(String.Format("[{0}.{1}] 非法,只能使用基本数据类型作为泛型类型", type.Name, curr.Name));
                    }
                    else if (curr.FieldType.Name == typeof(OODictionary<,>).Name)
                    {
                        Type _TValueType = TTypes[1];
                        Type tKeyType = TTypes[0];
                        if (!OODBValueType.IsValueType(tKeyType))
                            throw new Exception(String.Format("OODoc [{0}] 非法，字段 {1} 的Key类型是不被支持的", type.Name, curr.Name));

                        {
                            bool isValueGroupType = OODBValueType.IsValueGroupType(_TValueType);
                            if (!isValueGroupType && !OODBValueType.IsValueType(_TValueType))
                                throw new Exception(String.Format("[{0}.{1}] 非法，泛类型是不被支持的", type.Name, curr.Name));

                            if (isValueGroupType)
                                CheckDocClass(_TValueType, typeof(OODoc), typeof(OOTab));
                        }
                    }
                    else
                        throw new Exception(String.Format("[{0}.{1}] 非法，使用了不支持的扩展类型", type.Name, curr.Name));
                }
                else
                    CheckDocClass(curr.FieldType, typeof(OODoc), typeof(OOTab));  //深度对字段进行检查

                if (!curr.IsInitOnly)
                    throw new Exception(String.Format("[{0}.{1}] 非法，必须使用readonly修饰符", type.Name, curr.Name));
            }
        }



        public static OODBInstance Single = null;

        string mLastError = "";
        public string LastError { get { return mLastError; } }

        //是否记录脏数据
        internal static bool mWriteRecord = true;

        //表名 TabInfo
        internal Dictionary<string, TabInfo> m_TabInfosIndexByName = new   Dictionary<string, TabInfo>();

        //记录检查过的类型，避免递归检查，导致死循环
        internal Dictionary<string, DocInfo> mDocInfos = new Dictionary<string, DocInfo>();

        //数据库别名 表名 TabInfo
        internal Dictionary<string, Dictionary<string, TabInfo>> mTabInfos = new Dictionary<string, Dictionary<string, TabInfo>>();
    }

    class WriteRecordDisabled:IDisposable
    {
        public WriteRecordDisabled()
        {
            OODBInstance.mWriteRecord = false;
        }

        public void Dispose()
        {
            OODBInstance.mWriteRecord = true;
        }
    }
}

