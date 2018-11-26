using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Collections;

namespace OODB
{

    public class OODictionary<TKey, TValue> : OOValueGroup, IEnumerable
    {
        public OODictionary()
        {
            _isNodeValue = typeof(OOValueGroup).IsAssignableFrom(typeof(TValue));
        }

        public int Count
        {
            get { return _Dictionary.Count; }
        }

        public virtual void Add(TKey k, TValue v)
        {
            _Dictionary.Add(k, v);
            RecordOP(k, DictionaryOP.AddOrEdit);
        }

        public virtual void Remove(TKey k)
        {
            _Dictionary.Remove(k);
            RecordOP(k, DictionaryOP.Del);
        }

        public void Clear()
        {
            foreach(var curr in _Dictionary)
                RecordOP(curr.Key, DictionaryOP.Del);

            _Dictionary.Clear();
        }

        public virtual TValue this[TKey key]
        {
            get { return _Dictionary[key]; }
            set
            {
                _Dictionary[key] = value;
                RecordOP(key, DictionaryOP.AddOrEdit);
            }
        }

        public virtual bool ContainsKey(TKey k)
        {
            return _Dictionary.ContainsKey(k);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }



        internal override void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder)
        {
            if (_OPs.Count < 1)
            {
                if (_isNodeValue && _Dictionary.Count > 0) //全部子对象构建命令
                {
                    if (!String.IsNullOrEmpty(parentPath))
                        parentPath += ".";

                    foreach (KeyValuePair<TKey, TValue> curr in _Dictionary)
                    {
                        string objPath = parentPath + OODBValueType.ToStringValue(curr.Key);
                        OOValueGroup nv = curr.Value as OOValueGroup;
                        nv.BuildUpdateCmd(objPath, updateBuilder);
                    }
                }

                return;
            }

            if (!String.IsNullOrEmpty(parentPath))
                parentPath += ".";

            foreach (KeyValuePair<TKey, DictionaryOP> curr in _OPs)
            {
                DictionaryOP op = curr.Value;

                string objPath = parentPath + OODBValueType.ToStringValue(curr.Key);
                if (op == DictionaryOP.AddOrEdit)
                {
                    BsonValue v;
                    if (_isNodeValue)
                    {
                        OOValueGroup nv = _Dictionary[curr.Key] as OOValueGroup;
                        v = nv.ToBsonValue();
                    }
                    else
                        v = OODBValueType.ToBsonValue(_Dictionary[curr.Key]);

                    updateBuilder = updateBuilder.Set(objPath, v);
                }
                else
                {
                    updateBuilder = updateBuilder.Unset(objPath);
                }
            }

            //所有对象都应调用BuildUpdateCmd
            if (_isNodeValue && _Dictionary.Count > 0)
            {
                foreach (KeyValuePair<TKey, TValue> curr in _Dictionary)
                {
                    if (_OPs.ContainsKey(curr.Key)) continue;//本次已经操作过，忽略执行

                    string objPath = parentPath + OODBValueType.ToStringValue(curr.Key);
                    OOValueGroup nv = curr.Value as OOValueGroup;
                    nv.BuildUpdateCmd(objPath, updateBuilder);
                }
            }
        }

        internal override void FromBsonValue(BsonValue v)
        {
            BsonDocument bDoc = v as BsonDocument;
            if (bDoc == null) return;

            if (_isNodeValue)
            {
                Type valueType = typeof(TValue);

                foreach (BsonElement currKV in bDoc)
                {
                    TKey key = (TKey)OODBValueType.FromStringValue(typeof(TKey), currKV.Name);
                    object vInstance = valueType.Assembly.CreateInstance(valueType.FullName);
                    OOValueGroup nv = vInstance as OOValueGroup;
                    nv.FromBsonValue(currKV.Value);
                    _Dictionary.Add(key, (TValue)vInstance);
                    _DBKeys.Add(key);
                }
            }
            else
            {
                foreach (BsonElement currKV in bDoc)
                {
                    TKey key = (TKey)OODBValueType.FromStringValue(typeof(TKey), currKV.Name);
                    object vInstance = OODBValueType.FromBsonValue(typeof(TValue), currKV.Value);
                    _Dictionary.Add(key, (TValue)vInstance);
                    _DBKeys.Add(key);
                }
            }
        }

        internal override BsonValue ToBsonValue()
        {
            BsonDocument bDoc = new BsonDocument();

            if (_isNodeValue)
            {
                foreach (KeyValuePair<TKey, TValue> curr in _Dictionary)
                {
                    string strkey = OODBValueType.ToStringValue(curr.Key);
                    OOValueGroup nv = curr.Value as OOValueGroup;
                    bDoc.Add(strkey, nv.ToBsonValue());
                }
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> curr in _Dictionary)
                {
                    string strkey = OODBValueType.ToStringValue(curr.Key);
                    bDoc.Add(strkey, OODBValueType.ToBsonValue(curr.Value));
                }
            }

            return bDoc;
        }

        internal override void SetNoChanged()
        {
            //所有内存中的key变为数据库中的key
            foreach (KeyValuePair<TKey, DictionaryOP> curr in _OPs)
            {
                if (curr.Value == DictionaryOP.AddOrEdit && !_DBKeys.Contains(curr.Key))
                    _DBKeys.Add(curr.Key);
            }

            //所有操作清除
            _OPs.Clear();


            //所有子对象设为未变更
            if (_isNodeValue)
            {
                foreach (KeyValuePair<TKey, TValue> curr in _Dictionary)
                {
                    OOValueGroup nv = curr.Value as OOValueGroup;
                    nv.SetNoChanged();
                }
            }
        }


        void RecordOP(TKey key, DictionaryOP op)
        {
            if (!OODBInstance.__WriteRecord) return;

            if (_OPs.ContainsKey(key))
            {
                if (_OPs[key] == op)
                    return;

                _OPs[key] = op;
            }
            else
                _OPs.Add(key, op);

            if (op == DictionaryOP.Del && !_DBKeys.Contains(key))//删除一个数据库中不存在的key,可以忽略掉
            {
                _OPs.Remove(key);
            }
        }

        enum DictionaryOP
        {
            AddOrEdit,
            Del
        }
        Dictionary<TKey, DictionaryOP> _OPs = new Dictionary<TKey, DictionaryOP>();//记录操作
        HashSet<TKey> _DBKeys = new HashSet<TKey>();//在数据库内存在的key
        Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();
        bool _isNodeValue;
    }


    public sealed class OODictionaryEx<TValue> : OODictionary<string, TValue>
    {
        public override TValue this[string key]
        {
            get { return base[TryToBase64String(key)]; }
            set { base[TryToBase64String(key)] = value; }
        }

        public override void Add(string k, TValue v)
        {
            base.Add(TryToBase64String(k), v);
        }
        public override bool ContainsKey(string k)
        {
            return base.ContainsKey(TryToBase64String(k));
        }
        public override IEnumerator GetEnumerator()
        {
            Dictionary<string, TValue> temp = new Dictionary<string, TValue>();
            IEnumerator tempEnumerator = base.GetEnumerator();
            while (tempEnumerator.MoveNext())
            {
                KeyValuePair<string, TValue> tempKV = (KeyValuePair<string, TValue>)tempEnumerator.Current;
                temp[TryFromBase64String(tempKV.Key)] = tempKV.Value;
            }
            return temp.GetEnumerator();
        }
        public override void Remove(string k)
        {
            base.Remove(TryToBase64String(k));
        }


        /// <summary>
        /// 试着转64位编码
        /// </summary>
        string TryToBase64String(string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// 试着64位解码
        /// </summary>
        string TryFromBase64String(string encodeString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodeString));
        }
    }
}
