using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace OODB
{
    public class OOList<T> : OOValueGroup
    {
        public void Add(T v)
        {
            mList.Add(v);

            SetChanged();
        }

        void SetChanged()
        {
            if(OODBInstance.mWriteRecord)
            mChanged = true;
        } 

        public void Insert(int index, T v)
        {
            mList.Insert(index, v);
            SetChanged();
        }

        public void RemoveAt(int index)
        {

            mList.RemoveAt(index);
            SetChanged();
        }

        public void Remove(T v)
        {
            mList.Remove(v);
            SetChanged();
        }

        public void Clear()
        {
            mList.Clear();
            SetChanged();
        }

        public int Count { get { return mList.Count; } }

        public T this[int index]
        { 
            get { return mList[index]; } 
            set {   mList[index] = value; SetChanged();   } 
        }

        internal override void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder)
        {
            if (!mChanged) 
                return;

            updateBuilder = updateBuilder.Set(parentPath, ToBsonValue());
        }

        internal override BsonValue ToBsonValue()
        {
            BsonArray barray = new BsonArray();

            foreach (T curr in mList)
            {
                barray.Add(OODBValueType.ToBsonValue(curr));
            }
        
            return barray;
        }

        internal override void FromBsonValue(BsonValue v)
        {
            BsonArray barray = v as BsonArray;
            mList.Clear();

            for(int i=0;i<barray.Count;i++)
            {
                object rv = OODBValueType.FromBsonValue(typeof(T), barray[i]);
                mList.Add((T)rv);
            }
        }

        internal override void SetNoChanged()
        {
            mChanged = false; 
        }

        public virtual bool Contains(T k)
        {
            return mList.Contains(k);
        }

        internal bool mChanged = false;//list类型一但有变化需要全部更新

        List<T> mList = new List<T>(); 
    }
}
