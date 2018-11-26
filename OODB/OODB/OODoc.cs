using System;
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Runtime.CompilerServices;

namespace OODB
{
    public abstract class OOValueGroup
    {
       internal abstract BsonValue ToBsonValue();

       internal abstract void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder);

       internal abstract void SetNoChanged();

       internal abstract void FromBsonValue(BsonValue v);
    }

    class MemValues<TValue>
    {
        internal Dictionary<string, TValue> _FieldValues = new Dictionary<string, TValue>();
        internal HashSet<string> _NeedUpdate = new HashSet<string>();

        internal void SetNoChanged()
        {
            _NeedUpdate.Clear();
        }

        internal void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder)
        {
            if (_NeedUpdate.Count < 1) return;

            if (!String.IsNullOrEmpty(parentPath))
                parentPath += ".";

            foreach (string k in _NeedUpdate)
            {
                updateBuilder = updateBuilder.Set(parentPath + k, OODBValueType.ToBsonValue(_FieldValues[k]) );
            }
        }
    }

    public class OODoc : OOValueGroup
    {
        internal ObjectId _id = ObjectId.Empty;

        internal MemValues<string> _MemString = new MemValues<string>();
        //internal MemValues<DateTime> _MemDateTime = new MemValues<DateTime>();
        internal MemValues<long> _MemLong = new MemValues<long>();
        internal MemValues<Int32> _MemInt32 = new MemValues<Int32>();
        internal MemValues<UInt32> _MemUInt32 = new MemValues<UInt32>();
        internal MemValues<float> _MemSingle = new MemValues<float>();
        internal MemValues<byte[]> _MemByteArray = new MemValues<byte[]>();
       
        public OODoc()
        {
            _docInfo = OODBInstance.Single.m_DocInfos[this.GetType().Name];
        }

        public string GetID() {  return _id.ToString(); }

        /// <summary>
        /// 获取字段值，手动传递字段名
        /// </summary> 
        valueType MGetValue<valueType>( string fieldName  )
        {

            object reValue = OODBValueType.GetFieldValue(fieldName,
                     typeof(valueType),
                     _MemString,
                     _MemLong,
                     _MemInt32,
                     _MemUInt32,
                     _MemSingle,
                     _MemByteArray
                     );

            if (reValue == null)
                reValue = _docInfo._FieldAttrs[fieldName];

            return (valueType)reValue;
        }

        
        /// <summary>
        /// 获取字段值，自动计算字段名
        /// </summary> 
        public valueType GetValue<valueType>([CallerMemberName] string fieldName = "")
        {
            return MGetValue<valueType>(fieldName); 
        }

        /// <summary>
        /// 设置字段值，自动计算字段名
        /// </summary> 
        public void SetValue<valueType>(valueType v, [CallerMemberName] string fieldName = "")
        {  
            MSetValue(v, fieldName);
        }

        /// <summary>
        /// 设置字段值，手动传递字段名
        /// </summary>
        void MSetValue<valueType>(valueType v,string fieldName )
        {
            OODBValueType.SetNeedChangedField(fieldName, v,
                _MemString,
                _MemLong,
                _MemInt32,
                _MemUInt32,
                _MemSingle,
                _MemByteArray
                );
        }

        internal override void SetNoChanged()
        {
            _MemString.SetNoChanged();
            _MemLong.SetNoChanged();
            _MemInt32.SetNoChanged();
            _MemUInt32.SetNoChanged();
            _MemSingle.SetNoChanged();
            _MemByteArray.SetNoChanged();

            //所有子对象设为未变更
            {
                Type type = this.GetType();

                //类和结构体
                foreach (FieldInfo curr in type.GetFields())
                {
                    var v = curr.GetValue(this);
                    if (v == null)
                    {
                        throw new Exception(String.Format("{0}->{1} 未实例化", type.Name, curr.Name));
                    }

                    OOValueGroup nv = v as OOValueGroup;
                    nv.SetNoChanged();
                }
            }
        }

        /// <summary>
        /// 生成更新命令
        /// </summary>
        /// <param name="parentPath">父路径 例 Player.Hero  其中Hero为当前类存储位</param>
        /// <returns></returns>
        internal override void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder)
        {
            _MemString.BuildUpdateCmd(parentPath, updateBuilder);
            _MemLong.BuildUpdateCmd(parentPath, updateBuilder);
            _MemInt32.BuildUpdateCmd(parentPath, updateBuilder);
            _MemUInt32.BuildUpdateCmd(parentPath, updateBuilder);
            _MemSingle.BuildUpdateCmd(parentPath, updateBuilder);
            _MemByteArray.BuildUpdateCmd(parentPath, updateBuilder);

            if (!String.IsNullOrEmpty(parentPath))
                parentPath += ".";
 
            //所有子对象生成更新命令
            {
                Type type = this.GetType();

                //类和结构体
                foreach (FieldInfo curr in type.GetFields())
                {
                    var v = curr.GetValue(this);
                    OOValueGroup nv = v as OOValueGroup;
                    nv.BuildUpdateCmd(parentPath+curr.Name, updateBuilder);
                }
            }
        }

        /// <summary>
        /// 序列化成完整的Bson文档
        /// </summary>
        /// <returns></returns>
        internal override BsonValue ToBsonValue()
        {
            BsonDocument doc = new BsonDocument();
           
            Type type = this.GetType();
            
            //普通成员
            foreach (PropertyInfo curr in type.GetProperties())
            {
                var v = curr.GetValue(this,null); 
                doc.Add(curr.Name, OODBValueType.ToBsonValue(v));
            }

            //类和结构体
            foreach (FieldInfo curr in type.GetFields())
            {
                var v = curr.GetValue(this);
                if (v == null)
                {
                    throw new Exception(String.Format("{0}->{1} 未实例化", type.Name,curr.Name));
                }
                OOValueGroup nv = v as OOValueGroup;
                doc.Add(curr.Name, nv.ToBsonValue());
            }
            
            return doc;
        }

        internal override void FromBsonValue(BsonValue _doc)
        {
            BsonDocument doc = _doc as BsonDocument;
            if (doc == null) return;

            Type type = this.GetType();

            //普通成员
            foreach (PropertyInfo curr in type.GetProperties())
            {
                if (!doc.Contains(curr.Name)) continue; 

                BsonElement el =  doc.GetElement(curr.Name);
                curr.SetValue(
                    this,
                    OODBValueType.FromBsonValue(curr.PropertyType,el.Value)
                    ); 
            }

            //类和结构体
            foreach (FieldInfo curr in type.GetFields())
            {
                if (!doc.Contains(curr.Name)) continue;

                var v = curr.GetValue(this);
                if (v == null)
                {
                    throw new Exception(String.Format("{0}->{1} 未实例化", type.Name, curr.Name));
                }

                BsonElement el = doc.GetElement(curr.Name);
                OOValueGroup nv = v as OOValueGroup;
                nv.FromBsonValue(el.Value);
            }
        }

        DocInfo _docInfo;
    };


}
