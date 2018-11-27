using System;
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System.Collections;

namespace OODB
{

    public class OOTab : OODoc
    {
        public OOTab(OOConn conn)
        {
            Type type = this.GetType();
            mMongoCollection = GetMongoCollection(type, conn);
        }

        static MongoCollection<BsonDocument> GetMongoCollection(Type type, OOConn conn)
        {
            string dbNickName = OODBInstance.Single.m_TabInfosIndexByName[type.Name].DBNickName;
            return conn.Database.GetCollection(type.Name);
        }

        public long Count() { return mMongoCollection.Count(); }

        public void Save()
        {
            if (_id == ObjectId.Empty) //新纪录
            {
                //生成_id并采用完整存档的方式进行存档
                _id = ObjectId.GenerateNewId();

                //取得存档内容，并填充id
                BsonDocument doc = this.ToBsonValue() as BsonDocument;
                doc.Add("_id", _id);

                //存盘
                mMongoCollection.Insert(doc);
            }
            else //增量更新
            {
                //生成更新串
                UpdateBuilder updateBuild = new UpdateBuilder();
                this.BuildUpdateQuery("", updateBuild);

                //应用更新
                BsonDocument document = updateBuild.ToBsonDocument();
                if (document.ElementCount > 0)
                {
                    var queryName = Query.EQ("_id", _id);
                    mMongoCollection.Update(queryName, updateBuild);
                }
            }

            //清理所有变更
            this.SetNoChanged();
        }


        public static bool Delete<T>(OOConn conn, IMongoQuery query)
        {
            Type type = typeof(T);
            CheckTabType(type);
            MongoCollection mongoCollection = GetMongoCollection(type, conn);
            return !mongoCollection.Remove(query).HasLastErrorMessage;
        }

        public static IEnumerator Find<T>(OOConn conn, IMongoQuery query, int limit = 199999999, Dictionary<string, int> sortlist = null, FieldsDocument fds = null)
        {
            Type type = typeof(T);
            CheckTabType(type);

            MongoCollection mongoCollection = GetMongoCollection(type, conn);
            var frs = mongoCollection.FindAs<BsonDocument>(query);
            if (fds != null)
                frs.SetFields(fds);
            if (sortlist != null)
            {
                SortByDocument sortby = new SortByDocument();
                foreach (KeyValuePair<string, int> sitem in sortlist)
                {
                    if (!string.IsNullOrEmpty(sitem.Key))
                        sortby.Add(sitem.Key, sitem.Value);
                }
                frs.SetSortOrder(sortby);
            }
            frs.Limit = limit;
            foreach (BsonDocument book in frs)
            {
                Type[] ts = new Type[1] { typeof(OOConn) };

                object[] ps = new object[1] { conn };

                ConstructorInfo mi = type.GetConstructor(ts); //查询与指定参数匹配的构造函数

                OOTab newObj = mi.Invoke(ps) as OOTab;

                //改写期间禁用脏数据记录
                using (new WriteRecordDisabled())
                {
                    newObj.FromBsonValue(book);

                    //填充id 
                    {
                        BsonObjectId boid = (BsonObjectId)book.GetElement("_id").Value;
                        newObj._id = boid.Value;
                    }
                }

                yield return newObj;
            }
        }

        public static T FindFirst<T>(OOConn conn, IMongoQuery query) where T : class
        {
            IEnumerator it = Find<T>(conn, query);
            while (it.MoveNext())
            {
                return it.Current as T;
            }
            return default(T);
        }
        public static T FindPartFirst<T>(OOConn conn, IMongoQuery query, FieldsDocument fds = null) where T : class
        {
            IEnumerator it = Find<T>(conn, query, 199999999, null, fds);
            while (it.MoveNext())
            {
                return it.Current as T;
            }
            return default(T);
        }


        public static int FindCount<T>(OOConn conn, IMongoQuery query, int limit = 199999999, Dictionary<string, int> sortlist = null)
        {
            Type type = typeof(T);
            CheckTabType(type);

            MongoCollection mongoCollection = GetMongoCollection(type, conn);
            var frs = mongoCollection.FindAs<BsonDocument>(query);
            if (sortlist != null)
            {
                SortByDocument sortby = new SortByDocument();
                foreach (KeyValuePair<string, int> sitem in sortlist)
                {
                    if (!string.IsNullOrEmpty(sitem.Key))
                        sortby.Add(sitem.Key, sitem.Value);
                }
                frs.SetSortOrder(sortby);
            }
            frs.Limit = limit;

            return (int)frs.Count();

        }

        static void CheckTabType(Type type)
        {
            string n = type.Name;
            if (!OODBInstance.Single.m_TabInfosIndexByName.ContainsKey(n))
                throw new Exception(String.Format("类 {0} 不支持存档功能", n));
        }

        MongoCollection mMongoCollection;
    }

}
